using UnityEngine;
using WorldInteractionSystem.Runtime.Core;

namespace WorldInteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Basýlý tutarak açýlan sandýk (chest) interactable'ý.
    /// Hold süresi dolunca sandýk açýlýr ve tekrar etkileþime kapatýlýr.
    /// </summary>
    public sealed class ChestHoldInteractable : HoldInteractable
    {
        private const float k_DefaultHoldDuration = 2.0f;
        private const string k_OpenText = "Open";
        private const string k_OpenedText = "Opened";
        private const string k_LockedText = "Locked";

        private enum ChestState
        {
            Closed = 0,
            Opened = 1,
            Locked = 2
        }

        [Header("Config")]
        [SerializeField] private float m_HoldDuration = k_DefaultHoldDuration;

        [Header("State")]
        [SerializeField] private ChestState m_State = ChestState.Closed;

        public override float GetHoldDuration(InteractorContext context)
        {
            return m_HoldDuration;
        }

        public override bool CanInteract(InteractorContext context)
        {
            if (context.InteractorObject == null)
                return false;

            // Açýlmýþ veya kilitliyse etkileþim yok
            return m_State == ChestState.Closed;
        }

        public override string GetActionText(InteractorContext context)
        {
            return m_State switch
            {
                ChestState.Closed => k_OpenText,
                ChestState.Opened => k_OpenedText,
                ChestState.Locked => k_LockedText,
                _ => k_LockedText
            };
        }

        protected override void OnHoldStarted(InteractorContext context)
        {
            // Ýstersen burada SFX / anim baþlangýcý koyarsýn
            // Debug.Log("Chest hold started");
        }

        protected override void OnHoldCanceled(InteractorContext context)
        {
            // Ýstersen burada cancel feedback verirsin
            // Debug.Log("Chest hold canceled");
        }

        protected override void OnHoldCompleted(InteractorContext context)
        {
            m_State = ChestState.Opened;

            Debug.Log($"Chest opened by {context.InteractorObject.name}");

            // TODO (case için opsiyonel):
            // - Animator trigger
            // - Loot spawn
            // - Inventory add
            // - SFX / VFX
        }

        /// <summary>
        /// Dýþ sistemlerin (quest, key vb.) sandýðý kilitleyip açabilmesi için.
        /// </summary>
        public void SetLocked(bool isLocked)
        {
            if (isLocked)
            {
                m_State = ChestState.Locked;
            }
            else if (m_State == ChestState.Locked)
            {
                m_State = ChestState.Closed;
            }
        }
    }
}
