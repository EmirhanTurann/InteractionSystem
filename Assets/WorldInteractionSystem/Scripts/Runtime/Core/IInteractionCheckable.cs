namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Etkileþimin neden engellendiðini de raporlayabilen interactable ek kontratý.
    /// IInteractable bozulmaz; runner/UI bu interface'i varsa kullanýr.
    /// </summary>
    public interface IInteractionCheckable
    {
        /// <summary>
        /// Etkileþim uygunluðunu ve engel sebebini döndürür.
        /// </summary>
        InteractionCheckResult Check(InteractorContext context);
    }
}
