using System;
using UnityEngine;

namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Etkileþimi baþlatan kimse (Oyuncu,NPC) hakkýnda baðlamsal bilgi taþýr.
    /// Interactable nesnelerin interactor'ý doðrudan aramasýný engeller.
    /// </summary>
    public readonly struct InteractorContext
    {
        /// <summary>
        /// Etkileþimi baþlatan nesnenin transform bilgisi.
        /// </summary>
        public Transform InteractorTransform { get; }

        /// <summary>
        /// Etkileþimi baþlatan GameObject bilgisi.
        /// </summary>
        public GameObject InteractorObject { get; }

        /// <exception cref="ArgumentNullException">
        /// Parametrelerden herhangi biri null ise fýrlatýlýr.
        /// </exception>
        public InteractorContext(Transform interactorTransform, GameObject interactorObject)
        {
            InteractorTransform = interactorTransform
        ? interactorTransform
        : throw new ArgumentNullException(nameof(interactorTransform));

            InteractorObject = interactorObject
                ? interactorObject
                : throw new ArgumentNullException(nameof(interactorObject));
        }
    }
}
