namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Etkileþim türlerini belirtir.
    /// </summary>
    public enum InteractionType
    {
        /// <summary>
        /// Tek basýþ ile anýnda gerçekleþen etkileþim.
        /// </summary>
        Instant = 0,

        /// <summary>
        /// Basýlý tutma gerektiren etkileþim.
        /// </summary>
        Hold = 1,

        /// <summary>
        /// Aç/Kapa gibi ikili durumlu etkileþim.
        /// </summary>
        Toggle = 2
    }
}
