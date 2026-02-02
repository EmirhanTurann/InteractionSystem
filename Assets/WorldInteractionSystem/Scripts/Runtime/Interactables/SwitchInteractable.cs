using UnityEngine;
using WorldInteractionSystem.Runtime.Core;

namespace WorldInteractionSystem.Runtime.Interactables
{
    public sealed class SwitchInteractable : ToggleInteractable
    {
        private const string k_ActionOn = "Turn On";
        private const string k_ActionOff = "Turn Off";

        [Header("State")]
        [SerializeField] private bool m_IsOn;

        [Header("Targets")]
        [SerializeField] private MonoBehaviour[] m_TargetBehaviours;

        [Header("UI")]
        [SerializeField] private string m_ActionTextOn = k_ActionOn;
        [SerializeField] private string m_ActionTextOff = k_ActionOff;

        public override bool CanInteract(InteractorContext context) => true;

        public override string GetActionText(InteractorContext context)
        {
            return m_IsOn ? m_ActionTextOff : m_ActionTextOn;
        }

        protected override void OnToggle(InteractorContext context)
        {
            m_IsOn = !m_IsOn;

            Debug.Log($"[SwitchInteractable] Toggled: isOn={m_IsOn} targets={(m_TargetBehaviours == null ? 0 : m_TargetBehaviours.Length)}", this);

            if (m_TargetBehaviours == null || m_TargetBehaviours.Length == 0)
            {
                Debug.LogWarning("[SwitchInteractable] No targets assigned.", this);
                return;
            }

            for (int i = 0; i < m_TargetBehaviours.Length; i++)
            {
                MonoBehaviour mb = m_TargetBehaviours[i];

                if (mb == null)
                {
                    Debug.LogWarning($"[SwitchInteractable] Target index {i} is NULL.", this);
                    continue;
                }

                Debug.Log($"[SwitchInteractable] Target[{i}] = {mb.GetType().Name} on '{mb.name}'", mb);

                if (mb is ISwitchTarget target)
                {
                    target.SetActive(m_IsOn, context);
                    Debug.Log($"[SwitchInteractable] -> called ISwitchTarget.SetActive({m_IsOn}) on '{mb.name}'", mb);
                }
                else
                {
                    Debug.LogWarning($"[SwitchInteractable] '{mb.name}' does NOT implement ISwitchTarget. (Wrong component or duplicate interface definition)", mb);
                }
            }
        }
    }
}
