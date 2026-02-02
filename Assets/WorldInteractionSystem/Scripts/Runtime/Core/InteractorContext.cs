using System;
using UnityEngine;

namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Etkileþimi baþlatan varlýk hakkýnda baðlamsal bilgi taþýr.
    /// Interactable nesnelerin interactor'ý doðrudan aramasýný engeller.
    /// </summary>
    public readonly struct InteractorContext
    {
        #region Fields

        private readonly GameObject m_InteractorObject;
        private readonly Transform m_ViewTransform;

        #endregion

        #region Constructors

        /// <summary>
        /// Yeni bir <see cref="InteractorContext"/> oluþturur.
        /// </summary>
        /// <param name="interactorObject">Etkileþimi baþlatan GameObject.</param>
        /// <param name="viewTransform">
        /// Etkileþim için görüþ/aim kaynaðý. Player'da camera pivot olabilir; NPC'de head transform olabilir.
        /// Null olabilir.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="interactorObject"/> null ise.</exception>
        public InteractorContext(GameObject interactorObject, Transform viewTransform = null)
        {
            if (interactorObject == null)
                throw new ArgumentNullException(nameof(interactorObject));

            m_InteractorObject = interactorObject;
            m_ViewTransform = viewTransform;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Etkileþimi baþlatan GameObject.
        /// </summary>
        public GameObject InteractorObject => m_InteractorObject;

        /// <summary>
        /// Etkileþimci transform'u.
        /// </summary>
        public Transform InteractorTransform => m_InteractorObject.transform;

        /// <summary>
        /// Etkileþim için görüþ/aim kaynaðý (opsiyonel).
        /// </summary>
        public Transform ViewTransform => m_ViewTransform;

        #endregion
    }
}
