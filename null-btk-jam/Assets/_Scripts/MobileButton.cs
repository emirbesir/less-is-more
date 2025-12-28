using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/// <summary>
/// Helper component for mobile buttons to detect press and release events.
/// Attach this to each mobile control button.
/// </summary>
public class MobileButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ButtonType
    {
        Left,
        Right,
        Jump,
        Kill,
        Restart
    }

    [SerializeField] private ButtonType _buttonType;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        switch (_buttonType)
        {
            case ButtonType.Left:
                if (MobileInputManager.Instance != null)
                    MobileInputManager.Instance.OnLeftButtonDown();
                break;
            case ButtonType.Right:
                if (MobileInputManager.Instance != null)
                    MobileInputManager.Instance.OnRightButtonDown();
                break;
            case ButtonType.Jump:
                if (MobileInputManager.Instance != null)
                    MobileInputManager.Instance.OnJumpButtonDown();
                break;
            case ButtonType.Kill:
                // Find PlayerDeath dynamically to handle scene changes
                var playerDeath = FindFirstObjectByType<PlayerDeath>();
                if (playerDeath != null)
                    playerDeath.TriggerDeath();
                break;
            case ButtonType.Restart:
                // Find LevelRestart dynamically to handle scene changes
                var levelRestart = FindFirstObjectByType<LevelRestart>();
                if (levelRestart != null)
                    levelRestart.RestartLevel();
                break;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (MobileInputManager.Instance == null) return;
        
        switch (_buttonType)
        {
            case ButtonType.Left:
                MobileInputManager.Instance.OnLeftButtonUp();
                break;
            case ButtonType.Right:
                MobileInputManager.Instance.OnRightButtonUp();
                break;
            case ButtonType.Jump:
                MobileInputManager.Instance.OnJumpButtonUp();
                break;
            // Kill and Restart don't need OnPointerUp as they're one-shot actions
        }
    }
}
