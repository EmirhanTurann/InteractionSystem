namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Etkileþime girilebilen tüm nesneler için bir kontrat tanýmlar.
    /// Core sistemler, interactable nesnelerle yalnýzca bu interface üzerinden iletiþim kurar.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Etkileþimin hangi türde olacaðýný tanýmlar (Instant, Hold, Toggle).
        /// </summary>
        InteractionType Type { get; }

        /// <summary>
        /// Etkileþimcinin (Oyuncu,NPC) þu anda bu nesneyle
        /// etkileþime girip giremeyeceðini belirler.
        /// </summary>
        /// <param name="context">Etkileþimci hakkýnda bilgi.</param>
        bool CanInteract(InteractorContext context);

        /// <summary>
        /// Etkileþim UI'ýnda kullanýlacak aksiyon metnini döndürür.
        /// Bu metin, input tuþ bilgisi UI katmanýnda birleþtirilir.
        /// </summary>
        /// <param name="context">Etkileþimci hakkýnda bilgi.</param>
        string GetActionText(InteractorContext context);

        /// <summary>
        /// Etkileþim tetiklendiðinde interactor tarafýndan çaðrýlýr.
        /// Hold etkileþimlerinde genellikle basýlý tutma tamamlandýktan sonra çaðrýlýr.
        /// </summary>
        /// <param name="context">Etkileþimci hakkýnda bilgi.</param>
        void Interact(InteractorContext context);
    }
}
