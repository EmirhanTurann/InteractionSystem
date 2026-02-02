using WorldInteractionSystem.Runtime.Core;

namespace WorldInteractionSystem.Runtime.Interactables
{
    /// <summary>
    /// Switch tarafýndan tetiklenebilen hedef kontratý.
    /// (Light, Door, Platform vs.)
    /// </summary>
    public interface ISwitchTarget
    {
        void SetActive(bool isActive, InteractorContext context);
    }
}
