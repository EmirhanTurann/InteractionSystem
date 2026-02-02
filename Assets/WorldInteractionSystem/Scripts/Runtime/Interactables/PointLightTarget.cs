using UnityEngine;
using WorldInteractionSystem.Runtime.Core;

namespace WorldInteractionSystem.Runtime.Interactables
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Light))]
    public sealed class PointLightTarget : MonoBehaviour, ISwitchTarget
    {
        [SerializeField] private Light m_Light;

        private void Reset()
        {
            m_Light = GetComponent<Light>();
            Debug.Log("[PointLightTarget] Reset() - Light bound.", this);
        }

        private void Awake()
        {
            if (m_Light == null)
                m_Light = GetComponent<Light>();

            Debug.Log("[PointLightTarget] Awake() - Ready.", this);
        }

        public void SetActive(bool isActive, InteractorContext context)
        {
            if (m_Light == null)
            {
                Debug.LogError("[PointLightTarget] Light reference is NULL.", this);
                return;
            }

            m_Light.enabled = isActive;
            Debug.Log($"[PointLightTarget] SetActive={isActive} by {(context.InteractorObject != null ? context.InteractorObject.name : "Unknown")}", this);
        }
    }
}
