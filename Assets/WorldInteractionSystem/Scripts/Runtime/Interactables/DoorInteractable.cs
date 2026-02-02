using UnityEngine;
using WorldInteractionSystem.Runtime.Core;
using WorldInteractionSystem.Runtime.Interactors;
using WorldInteractionSystem.Runtime.Items;

namespace WorldInteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Kapý: ToggleInteractable.
    /// Locked ise RequiredKey olmadan açýlmaz.
    /// </summary>
    public sealed class DoorInteractable : ToggleInteractable, IInteractionCheckable
    {
        private const string k_OpenText = "Open";
        private const string k_CloseText = "Close";
        private const string k_LockedText = "Locked";

        #region Inspector

        [Header("Door")]
        [SerializeField] private bool m_IsOpen = false;

        [Header("Lock")]
        [SerializeField] private bool m_IsLocked = true;
        [SerializeField] private KeyItemSO m_RequiredKey;

        [Tooltip("Kilit açýlýnca anahtar tüketilsin mi?")]
        [SerializeField] private bool m_ConsumeKeyOnUnlock = false;

        [Header("UI")]
        [SerializeField] private string m_OpenText = k_OpenText;
        [SerializeField] private string m_CloseText = k_CloseText;
        [SerializeField] private string m_LockedText = k_LockedText;

        #endregion

        #region IInteractionCheckable

        public InteractionCheckResult Check(InteractorContext context)
        {
            if (context.InteractorObject == null)
                return InteractionCheckResult.Blocked(InteractionBlockReason.Custom);

            if (!m_IsLocked)
                return InteractionCheckResult.Allowed();

            // Locked -> key var mý?
            return HasRequiredKey(context)
                ? InteractionCheckResult.Allowed()
                : InteractionCheckResult.Blocked(InteractionBlockReason.Locked);
        }

        #endregion

        public override bool CanInteract(InteractorContext context)
        {
            // Seçim/targeting tarafý Check() kullanýyorsa bile legacy uyumlu kalsýn.
            return Check(context).CanInteract;
        }

        public override string GetActionText(InteractorContext context)
        {
            if (m_IsLocked && !HasRequiredKey(context))
                return m_LockedText;

            return m_IsOpen ? m_CloseText : m_OpenText;
        }

        protected override void OnToggle(InteractorContext context)
        {
            // Güvenlik
            InteractionCheckResult check = Check(context);
            if (!check.CanInteract)
                return;

            if (m_IsLocked)
                UnlockWithOptionalConsume(context);

            m_IsOpen = !m_IsOpen;

            // TODO: anim/rotate/hinge burada
            Debug.Log($"Door toggled. IsOpen={m_IsOpen}", this);
        }

        private bool HasRequiredKey(InteractorContext context)
        {
            if (m_RequiredKey == null)
                return false;

            var inventory = context.InteractorObject.GetComponent<KeyInventory>();
            if (inventory == null)
                return false;

            return inventory.HasKey(m_RequiredKey);
        }

        private void UnlockWithOptionalConsume(InteractorContext context)
        {
            if (m_ConsumeKeyOnUnlock)
            {
                var inventory = context.InteractorObject.GetComponent<KeyInventory>();
                if (inventory != null)
                    inventory.RemoveKey(m_RequiredKey);
            }

            m_IsLocked = false;
        }
    }
}
