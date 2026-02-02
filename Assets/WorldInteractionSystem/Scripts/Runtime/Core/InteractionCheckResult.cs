namespace WorldInteractionSystem.Runtime.Core
{
    /// <summary>
    /// Etkileþim uygunluðunun sonucu.
    /// UI ve runner, sebebe göre davranýþ gösterebilir.
    /// </summary>
    public readonly struct InteractionCheckResult
    {
        #region Properties

        public bool CanInteract { get; }
        public InteractionBlockReason BlockReason { get; }

        /// <summary>
        /// Cooldown gibi durumlarda kalan süre (saniye). Kullanýlmýyorsa 0.
        /// </summary>
        public float RemainingTime { get; }

        #endregion

        #region Constructors

        public InteractionCheckResult(bool canInteract, InteractionBlockReason blockReason, float remainingTime = 0f)
        {
            CanInteract = canInteract;
            BlockReason = blockReason;
            RemainingTime = remainingTime;
        }

        #endregion

        #region Factory

        public static InteractionCheckResult Allowed()
        {
            return new InteractionCheckResult(true, InteractionBlockReason.None, 0f);
        }

        public static InteractionCheckResult Blocked(InteractionBlockReason reason, float remainingTime = 0f)
        {
            return new InteractionCheckResult(false, reason, remainingTime);
        }

        #endregion
    }
}
