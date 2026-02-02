using UnityEngine;

namespace WorldInteractionSystem.Runtime.Items
{
    /// <summary>
    /// Tekil anahtar tanýmý. "Her kapýnýn kendi anahtarý" için her kapý için ayrý asset oluþtur.
    /// </summary>
    [CreateAssetMenu(menuName = "WorldInteractionSystem/Items/Key Item", fileName = "SO_KeyItem")]
    public sealed class KeyItemSO : ScriptableObject
    {
        #region Inspector

        [SerializeField] private string m_DisplayName = "Key";

        // Asset referansý zaten unique; ama debug/log/serialize için ID da tutuyoruz.
        [SerializeField] private string m_KeyId = "";

        #endregion

        #region Properties

        public string DisplayName => m_DisplayName;

        public string KeyId => m_KeyId;

        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(m_KeyId))
                m_KeyId = System.Guid.NewGuid().ToString("N");
        }
#endif
    }
}
