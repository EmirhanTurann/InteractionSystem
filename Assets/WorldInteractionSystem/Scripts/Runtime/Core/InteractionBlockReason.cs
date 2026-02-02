namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Etkileþimin neden engellendiðini ifade eden standart sebepler.
    /// String yerine enum kullanýlýr (UI lokalizasyonu UI katmanýnda yapýlýr).
    /// </summary>
    public enum InteractionBlockReason
    {
        None = 0,

        /// <summary>Interactor null/invalid.</summary>
        InvalidInteractor = 1,

        /// <summary>Mesafe yetersiz.</summary>
        OutOfRange = 2,

        /// <summary>Görüþ hattý yok.</summary>
        NoLineOfSight = 3,

        /// <summary>Cooldown devam ediyor.</summary>
        Cooldown = 4,

        /// <summary>Kilitli/anahtar lazým.</summary>
        Locked = 5,

        /// <summary>Nesne meþgul (baþkasý kullanýyor / iþlem sürüyor).</summary>
        Busy = 6,

        /// <summary>Projeye özel kural.</summary>
        Custom = 99
    }
}
