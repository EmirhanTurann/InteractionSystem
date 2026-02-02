using UnityEngine;
using WorldInteractionSystem.Runtime.Core;

namespace WorldInteractionSystem.Runtime.Interactables
{
    public abstract class HoldInteractable : MonoBehaviour, IInteractable, IInteractionCheckable
    {
        #region Fields

        private bool m_IsHoldActive;

        #endregion

        #region IInteractable Implementation (Explicit)

        InteractionType IInteractable.Type => InteractionType.Hold;

        bool IInteractable.CanInteract(InteractorContext context) => Check(context).CanInteract;

        string IInteractable.GetActionText(InteractorContext context) => GetActionText(context);

        void IInteractable.Interact(InteractorContext context) => Interact(context);

        #endregion

        #region IInteractionCheckable Implementation (Explicit)

        InteractionCheckResult IInteractionCheckable.Check(InteractorContext context) => Check(context);

        #endregion

        #region Public API

        public abstract float GetHoldDuration(InteractorContext context);

        public abstract string GetActionText(InteractorContext context);

        public virtual InteractionCheckResult Check(InteractorContext context)
        {
            if (context.InteractorObject == null)
                return InteractionCheckResult.Blocked(InteractionBlockReason.InvalidInteractor);

            if (!CanInteract(context))
                return InteractionCheckResult.Blocked(InteractionBlockReason.Custom);

            float duration = GetHoldDuration(context);
            if (duration <= 0f)
                return InteractionCheckResult.Blocked(InteractionBlockReason.Custom);

            return InteractionCheckResult.Allowed();
        }

        /// <summary>
        /// Eski uyumluluk için býrakýldý. Tercihen <see cref="Check"/> override edilmeli.
        /// </summary>
        public abstract bool CanInteract(InteractorContext context);

        /// <summary>
        /// Hold için tek entry point: BeginInteract.
        /// </summary>
        public void Interact(InteractorContext context)
        {
            BeginInteract(context);
        }

        public void BeginInteract(InteractorContext context)
        {
            if (m_IsHoldActive)
                return;

            InteractionCheckResult result = Check(context);
            if (!result.CanInteract)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"{name}: BeginInteract blocked. Reason={result.BlockReason} Remaining={result.RemainingTime}", this);
#endif
                return;
            }

            m_IsHoldActive = true;
            OnHoldStarted(context);
        }

        public void CancelInteract(InteractorContext context)
        {
            if (!m_IsHoldActive)
                return;

            m_IsHoldActive = false;
            OnHoldCanceled(context);
        }

        public void CompleteInteract(InteractorContext context)
        {
            if (!m_IsHoldActive)
                return;

            InteractionCheckResult result = Check(context);
            if (!result.CanInteract)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"{name}: CompleteInteract blocked. Reason={result.BlockReason} Remaining={result.RemainingTime}", this);
#endif
                m_IsHoldActive = false;
                OnHoldCanceled(context);
                return;
            }

            m_IsHoldActive = false;
            OnHoldCompleted(context);
        }

        #endregion

        #region Protected

        protected virtual void OnHoldStarted(InteractorContext context) { }

        protected virtual void OnHoldCanceled(InteractorContext context) { }

        protected abstract void OnHoldCompleted(InteractorContext context);

        #endregion
    }
}
