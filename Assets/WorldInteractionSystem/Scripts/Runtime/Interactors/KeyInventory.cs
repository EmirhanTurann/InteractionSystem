using System.Collections.Generic;
using UnityEngine;
using WorldInteractionSystem.Runtime.Items;

namespace WorldInteractionSystem.Runtime.Interactors
{
    /// <summary>
    /// Interactor (genelde Player) üzerinde duran anahtar envanteri.
    /// </summary>
    public sealed class KeyInventory : MonoBehaviour
    {
        #region Fields

        private readonly HashSet<KeyItemSO> m_Keys = new HashSet<KeyItemSO>();

        #endregion

        #region Public Methods

        public bool HasKey(KeyItemSO key)
        {
            if (key == null)
                return false;

            return m_Keys.Contains(key);
        }

        public void AddKey(KeyItemSO key)
        {
            if (key == null)
                return;

            m_Keys.Add(key);
        }

        public void RemoveKey(KeyItemSO key)
        {
            if (key == null)
                return;

            m_Keys.Remove(key);
        }

        #endregion
    }
}
