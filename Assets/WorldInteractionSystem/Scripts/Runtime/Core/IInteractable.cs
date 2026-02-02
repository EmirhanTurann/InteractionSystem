namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Etkileþime girilebilen tüm nesneler için temel kontrat.
    /// Core sistemler interactable nesnelerle yalnýzca bu arayüz üzerinden iletiþim kurar.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Etkileþim türünü belirtir.
        /// </summary>
        InteractionType Type { get; }

        /// <summary>
        /// Etkileþim þu anda mümkün mü?
        /// (Detay sebep gerekiyorsa <see cref="IInteractionCheckable"/> kullanýlmalýdýr.)
        /// </summary>
        bool CanInteract(InteractorContext context);

        /// <summary>
        /// UI'da gösterilecek aksiyon metni.
        /// </summary>
        string GetActionText(InteractorContext context);

        /// <summary>
        /// Etkileþimi gerçekleþtirir.
        /// </summary>
        void Interact(InteractorContext context);
    }
}
