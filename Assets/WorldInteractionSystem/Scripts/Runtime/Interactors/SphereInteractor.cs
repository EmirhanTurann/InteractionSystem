using UnityEngine;
using WorldInteractionSystem.Runtime.Core;
using WorldInteractionSystem.Runtime.Interactables;

namespace WorldInteractionSystem.Runtime.Interactors
{
    public sealed class SphereInteractor : MonoBehaviour
    {
        #region Inspector

        [Header("Scan")]
        [SerializeField] private float m_Range = 2.0f;
        [SerializeField] private LayerMask m_InteractableMask;

        [Header("Context")]
        [Tooltip("Görüş/aim kaynağı. Player'da camera pivot, NPC'de head olabilir. ÖNERİLİR.")]
        [SerializeField] private Transform m_ViewTransform;

        [Header("Targeting")]
        [SerializeField] private bool m_UseViewAngle = true;

        [Range(1f, 180f)]
        [SerializeField] private float m_MaxViewAngle = 70f;

        [SerializeField] private bool m_RequireLineOfSight = true;

        [SerializeField] private LayerMask m_OcclusionMask;

        [Header("Scoring")]
        [Min(0f)]
        [SerializeField] private float m_DistanceWeight = 1.0f;

        [Min(0f)]
        [SerializeField] private float m_AngleWeight = 1.0f;

        [Header("Input")]
        [SerializeField] private KeyCode m_InteractKey = KeyCode.E;

        [Header("Debug")]
        [SerializeField] private bool m_DrawDebug = true;

        [Tooltip("Etkileşim denemesinde adayların neden elendiğini loglar.")]
        [SerializeField] private bool m_DebugLogs = false;

        #endregion

        #region Constants

        private const int OverlapBufferSize = 16;
        private const float MinVectorSqr = 0.0001f;

        #endregion

        #region Fields

        private readonly Collider[] m_Overlap = new Collider[OverlapBufferSize];

        private HoldInteractable m_ActiveHold;
        private float m_HoldElapsed;
        private float m_HoldDuration;
        private InteractorContext m_HoldContext;

        private IInteractable m_LastTarget;
        private InteractionCheckResult m_LastTargetCheck;

        #endregion

        #region Properties

        public float HoldProgress01
        {
            get
            {
                if (m_ActiveHold == null || m_HoldDuration <= 0f)
                    return 0f;

                return Mathf.Clamp01(m_HoldElapsed / m_HoldDuration);
            }
        }

        public IInteractable LastTarget => m_LastTarget;

        public InteractionCheckResult LastTargetCheck => m_LastTargetCheck;

        #endregion

        #region Unity

        private void Reset()
        {
            if (m_ViewTransform == null)
                m_ViewTransform = transform;
        }

        private void Update()
        {
            if (m_ActiveHold != null)
            {
                UpdateHold();
                return;
            }

            if (!Input.GetKeyDown(m_InteractKey))
                return;

            if (m_DrawDebug)
                DebugDrawSphere(transform.position, m_Range);

            var context = new InteractorContext(gameObject, m_ViewTransform);

            if (!TryFindBest(context, out IInteractable best, out InteractionCheckResult bestCheck))
            {
                if (m_DebugLogs)
                    Debug.Log($"{name}: No target found (no candidates in range/angle/LOS).", this);

                // UI cache temizle
                m_LastTarget = null;
                m_LastTargetCheck = default;
                return;
            }

            // UI/debug cache
            m_LastTarget = best;
            m_LastTargetCheck = bestCheck;

            if (m_DebugLogs)
                Debug.Log($"{name}: Target = {best} | type={best.Type} | can={bestCheck.CanInteract} reason={bestCheck.BlockReason}", this);

            // Eğer hedef blocked ise Interact çağırmak yerine sadece feedback ver.
            // (UI zaten LastTargetCheck üzerinden "Locked" vs. gösterebilir.)
            if (!bestCheck.CanInteract)
                return;

            if (best is HoldInteractable hold)
            {
                BeginHold(hold, context);
                return;
            }

            best.Interact(context);
        }

        #endregion

        #region Hold Flow

        private void BeginHold(HoldInteractable hold, InteractorContext context)
        {
            float duration = hold.GetHoldDuration(context);
            if (duration <= 0f)
            {
                if (m_DebugLogs)
                    Debug.LogWarning($"{name}: Hold duration invalid: {duration} (hold={hold.name})", this);
                return;
            }

            m_ActiveHold = hold;
            m_HoldContext = context;
            m_HoldElapsed = 0f;
            m_HoldDuration = duration;

            m_ActiveHold.BeginInteract(context);

            if (m_DebugLogs)
                Debug.Log($"{name}: Hold started on {hold.name} duration={duration}", this);
        }

        private void UpdateHold()
        {
            if (Input.GetKeyUp(m_InteractKey))
            {
                if (m_DebugLogs)
                    Debug.Log($"{name}: Hold canceled (key up).", this);

                m_ActiveHold.CancelInteract(m_HoldContext);
                ClearHold();
                return;
            }

            if (!Input.GetKey(m_InteractKey))
            {
                if (m_DebugLogs)
                    Debug.Log($"{name}: Hold canceled (key not held).", this);

                m_ActiveHold.CancelInteract(m_HoldContext);
                ClearHold();
                return;
            }

            m_HoldElapsed += Time.deltaTime;

            if (m_HoldElapsed >= m_HoldDuration)
            {
                if (m_DebugLogs)
                    Debug.Log($"{name}: Hold completed.", this);

                m_ActiveHold.CompleteInteract(m_HoldContext);
                ClearHold();
            }
        }

        private void ClearHold()
        {
            m_ActiveHold = null;
            m_HoldElapsed = 0f;
            m_HoldDuration = 0f;
            m_HoldContext = default;
        }

        #endregion

        #region Target Selection

        private bool TryFindBest(InteractorContext context, out IInteractable best, out InteractionCheckResult bestCheck)
        {
            best = null;
            bestCheck = default;

            // Blocked fallback
            IInteractable bestBlocked = null;
            InteractionCheckResult bestBlockedCheck = default;

            float bestScore = float.MaxValue;
            float bestBlockedScore = float.MaxValue;

            Transform view = context.ViewTransform != null ? context.ViewTransform : context.InteractorTransform;
            Vector3 origin = view.position;

            Vector3 forward = view.forward;
            if (forward.sqrMagnitude < MinVectorSqr)
                forward = context.InteractorTransform.forward;

            int count = Physics.OverlapSphereNonAlloc(
                transform.position,
                m_Range,
                m_Overlap,
                m_InteractableMask,
                QueryTriggerInteraction.Collide);

            if (m_DebugLogs)
                Debug.Log($"{name}: Overlap count={count}", this);

            if (count <= 0)
                return false;

            for (int i = 0; i < count; i++)
            {
                Collider col = m_Overlap[i];
                if (col == null)
                    continue;

                // Daha toleranslı candidate arama
                IInteractable candidate =
                    col.GetComponent<IInteractable>() ??
                    col.GetComponentInParent<IInteractable>() ??
                    col.transform.root.GetComponentInChildren<IInteractable>();

                if (candidate == null)
                {
                    if (m_DebugLogs)
                        Debug.Log($"{name}: Reject '{col.name}' -> no IInteractable found.", this);
                    continue;
                }

                // Kapı-proof hedef noktası: ClosestPoint
                Vector3 targetPos = col.ClosestPoint(origin);
                Vector3 toTarget = targetPos - origin;
                float sqrDistance = toTarget.sqrMagnitude;

                if (sqrDistance <= MinVectorSqr)
                    continue;

                float angle01 = 0f;
                if (m_UseViewAngle)
                {
                    float angle = Vector3.Angle(forward, toTarget);
                    if (angle > m_MaxViewAngle)
                    {
                        if (m_DebugLogs)
                            Debug.Log($"{name}: Reject '{candidate}' -> angle {angle:0.0} > {m_MaxViewAngle:0.0}.", this);
                        continue;
                    }

                    angle01 = Mathf.Clamp01(angle / m_MaxViewAngle);
                }

                if (m_RequireLineOfSight)
                {
                    if (!HasLineOfSight(origin, targetPos, col, out string losHitInfo))
                    {
                        if (m_DebugLogs)
                            Debug.Log($"{name}: Reject '{candidate}' -> LOS blocked ({losHitInfo}).", this);
                        continue;
                    }
                }

                // Score
                float distance01 = Mathf.Clamp01(sqrDistance / (m_Range * m_Range));
                float score = (distance01 * m_DistanceWeight) + (angle01 * m_AngleWeight);

                // Check
                InteractionCheckResult check = GetCheckResult(candidate, context);

                if (check.CanInteract)
                {
                    if (m_DebugLogs)
                        Debug.Log($"{name}: Candidate OK '{candidate}' score={score:0.000}.", this);

                    if (score < bestScore)
                    {
                        bestScore = score;
                        best = candidate;
                        bestCheck = check;
                    }
                }
                else
                {
                    // Fallback: blocked candidate seçilebilir (Locked gibi)
                    if (m_DebugLogs)
                        Debug.Log($"{name}: Candidate BLOCKED '{candidate}' reason={check.BlockReason} score={score:0.000}.", this);

                    if (score < bestBlockedScore)
                    {
                        bestBlockedScore = score;
                        bestBlocked = candidate;
                        bestBlockedCheck = check;
                    }
                }
            }

            // Önce interactable hedef, yoksa blocked hedef
            if (best != null)
                return true;

            if (bestBlocked != null)
            {
                best = bestBlocked;
                bestCheck = bestBlockedCheck;
                return true;
            }

            return false;
        }

        private bool HasLineOfSight(Vector3 origin, Vector3 target, Collider targetCollider, out string hitInfo)
        {
            hitInfo = "clear";

            Vector3 dir = target - origin;
            float distance = dir.magnitude;
            if (distance <= 0.0001f)
                return true;

            dir /= distance;

            if (Physics.Raycast(origin, dir, out RaycastHit hit, distance, m_OcclusionMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider == null)
                    return true;

                if (hit.collider.transform.root != targetCollider.transform.root)
                {
                    hitInfo = $"{hit.collider.name} (root={hit.collider.transform.root.name})";
                    return false;
                }

                hitInfo = $"hit self '{hit.collider.name}'";
            }

            return true;
        }

        private static InteractionCheckResult GetCheckResult(IInteractable candidate, InteractorContext context)
        {
            if (candidate is IInteractionCheckable checkable)
                return checkable.Check(context);

            return candidate.CanInteract(context)
                ? InteractionCheckResult.Allowed()
                : InteractionCheckResult.Blocked(InteractionBlockReason.Custom);
        }

        #endregion

        #region Debug

        private static void DebugDrawSphere(Vector3 center, float radius)
        {
            Debug.DrawLine(center + Vector3.right * radius, center - Vector3.right * radius);
            Debug.DrawLine(center + Vector3.forward * radius, center - Vector3.forward * radius);
            Debug.DrawLine(center + Vector3.up * radius, center - Vector3.up * radius);
        }

        #endregion
    }
}
