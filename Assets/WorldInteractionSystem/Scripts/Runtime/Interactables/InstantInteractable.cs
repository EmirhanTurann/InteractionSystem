using UnityEngine;
using WorldInteractionSystem.Runtime.Core;

namespace WorldInteractionSystem.Runtime.Interactables
{
    public abstract class InstantInteractable : MonoBehaviour, IInteractable, IInteractionCheckable
    {
        #region Constants

        private const float BlockedLogCooldownSeconds = 0.5f;

        #endregion

        #region Fields

        private float m_LastBlockedLogTime;

        #endregion

        #region IInteractable Implementation (Explicit)

        InteractionType IInteractable.Type => InteractionType.Instant;

        bool IInteractable.CanInteract(InteractorContext context) => Check(context).CanInteract;

        string IInteractable.GetActionText(InteractorContext context) => GetActionText(context);

        void IInteractable.Interact(InteractorContext context) => Interact(context);

        #endregion

        #region IInteractionCheckable Implementation (Explicit)

        InteractionCheckResult IInteractionCheckable.Check(InteractorContext context) => Check(context);

        #endregion

        #region Public API

        /// <summary>
        /// UI aksiyon metni.
        /// </summary>
        public abstract string GetActionText(InteractorContext context);

        /// <summary>
        /// Detaylý check. Alt sýnýflar bunu override ederek block reason döndürmeli.
        /// </summary>
        public virtual InteractionCheckResult Check(InteractorContext context)
        {
            if (context.InteractorObject == null)
                return InteractionCheckResult.Blocked(InteractionBlockReason.InvalidInteractor);

            return CanInteract(context)
                ? InteractionCheckResult.Allowed()
                : InteractionCheckResult.Blocked(InteractionBlockReason.Custom);
        }

        /// <summary>
        /// Eski uyumluluk için býrakýldý. Tercihen <see cref="Check"/> override edilmeli.
        /// </summary>
        public abstract bool CanInteract(InteractorContext context);

        public void Interact(InteractorContext context)
        {
            InteractionCheckResult result = Check(context);
            if (!result.CanInteract)
            {
#if UNITY_EDITOR
                LogBlocked(result);
#endif
                return;
            }

            OnInteract(context);
        }

        #endregion

        #region Protected

        protected abstract void OnInteract(InteractorContext context);

#if UNITY_EDITOR
        private void LogBlocked(InteractionCheckResult result)
        {
            float now = Time.unscaledTime;
            if (now - m_LastBlockedLogTime < BlockedLogCooldownSeconds)
                return;

            m_LastBlockedLogTime = now;
            Debug.LogWarning($"{name}: Interact blocked. Reason={result.BlockReason} Remaining={result.RemainingTime}", this);
        }
#endif

        #endregion
    }
}
