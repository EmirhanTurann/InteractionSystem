using UnityEngine;
using WorldInteractionSystem.Runtime.Core;
using WorldInteractionSystem.Runtime.Interactors;
using WorldInteractionSystem.Runtime.Items;

namespace WorldInteractionSystem.Runtime.Interactables
{
    public sealed class KeyInteractable : InstantInteractable, IInteractionCheckable
    {
        private const string k_DefaultActionText = "Pick Up Key";

        [Header("Key")]
        [SerializeField] private KeyItemSO m_Key;

        [Header("UI")]
        [SerializeField] private string m_ActionText = k_DefaultActionText;

        public override string GetActionText(InteractorContext context)
        {
            return m_ActionText;
        }

        public override bool CanInteract(InteractorContext context)
        {
            return Check(context).CanInteract;
        }

        public InteractionCheckResult Check(InteractorContext context)
        {
            if (context.InteractorObject == null)
            {
                Debug.LogWarning($"[KeyInteractable] Blocked: context.InteractorObject is null ({name})", this);
                return InteractionCheckResult.Blocked(InteractionBlockReason.Custom);
            }

            if (m_Key == null)
            {
                Debug.LogWarning($"[KeyInteractable] Blocked: m_Key is NULL on '{name}'. Assign a KeyItemSO in Inspector.", this);
                return InteractionCheckResult.Blocked(InteractionBlockReason.Custom);
            }

            var inv = context.InteractorObject.GetComponent<KeyInventory>();
            if (inv == null)
            {
                Debug.LogWarning($"[KeyInteractable] Blocked: KeyInventory NOT FOUND on interactor '{context.InteractorObject.name}'. Add KeyInventory component.", context.InteractorObject);
                return InteractionCheckResult.Blocked(InteractionBlockReason.Custom);
            }

            return InteractionCheckResult.Allowed();
        }

        protected override void OnInteract(InteractorContext context)
        {
            var inv = context.InteractorObject != null
                ? context.InteractorObject.GetComponent<KeyInventory>()
                : null;

            if (inv == null)
            {
                Debug.LogError("[KeyInteractable] Interact failed: KeyInventory missing on interactor.", this);
                return;
            }

            if (m_Key == null)
            {
                Debug.LogError("[KeyInteractable] Interact failed: m_Key is NULL.", this);
                return;
            }

            inv.AddKey(m_Key);
            Debug.Log($"[KeyInteractable] Picked up: {m_Key.DisplayName} ({m_Key.KeyId})", this);

            Destroy(gameObject);
        }
    }
}
