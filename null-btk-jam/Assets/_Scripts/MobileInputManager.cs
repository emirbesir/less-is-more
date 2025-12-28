using UnityEngine;

/// <summary>
/// Singleton that handles mobile button inputs.
/// Attach this to a GameObject in the scene or use the prefab.
/// </summary>
public class MobileInputManager : MonoBehaviour
{
    public static MobileInputManager Instance { get; private set; }

    // Movement inputs
    public bool LeftHeld { get; private set; }
    public bool RightHeld { get; private set; }
    
    // Jump inputs
    public bool JumpDown { get; private set; }
    public bool JumpHeld { get; private set; }
    
    // Action inputs
    public bool KillPressed { get; private set; }
    public bool RestartPressed { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void LateUpdate()
    {
        // Reset one-frame inputs at the end of each frame
        JumpDown = false;
        KillPressed = false;
        RestartPressed = false;
    }

    #region Button Event Methods (Called by UI Buttons)

    // Left Button
    public void OnLeftButtonDown()
    {
        LeftHeld = true;
    }

    public void OnLeftButtonUp()
    {
        LeftHeld = false;
    }

    // Right Button
    public void OnRightButtonDown()
    {
        RightHeld = true;
    }

    public void OnRightButtonUp()
    {
        RightHeld = false;
    }

    // Jump Button
    public void OnJumpButtonDown()
    {
        JumpDown = true;
        JumpHeld = true;
    }

    public void OnJumpButtonUp()
    {
        JumpHeld = false;
    }

    // Kill Button
    public void OnKillButtonPressed()
    {
        KillPressed = true;
    }

    // Restart Button
    public void OnRestartButtonPressed()
    {
        RestartPressed = true;
    }

    #endregion

    /// <summary>
    /// Gets the horizontal input value (-1, 0, or 1) combining mobile and keyboard.
    /// </summary>
    public float GetHorizontalInput()
    {
        float mobileInput = 0f;
        if (LeftHeld) mobileInput -= 1f;
        if (RightHeld) mobileInput += 1f;
        
        // Combine with keyboard input
        float keyboardInput = Input.GetAxisRaw("Horizontal");
        
        // Return mobile if being used, otherwise keyboard
        if (mobileInput != 0f) return mobileInput;
        return keyboardInput;
    }

    /// <summary>
    /// Gets the vertical input value combining mobile and keyboard.
    /// </summary>
    public float GetVerticalInput()
    {
        // For now, just return keyboard vertical input
        // Add mobile vertical buttons if needed
        return Input.GetAxisRaw("Vertical");
    }

    /// <summary>
    /// Checks if jump was pressed this frame (mobile or keyboard).
    /// </summary>
    public bool GetJumpDown()
    {
        return JumpDown || Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.C);
    }

    /// <summary>
    /// Checks if jump is being held (mobile or keyboard).
    /// </summary>
    public bool GetJumpHeld()
    {
        return JumpHeld || Input.GetButton("Jump") || Input.GetKey(KeyCode.C);
    }

    /// <summary>
    /// Checks if kill was pressed this frame (mobile or keyboard).
    /// </summary>
    public bool GetKillPressed()
    {
        return KillPressed || Input.GetKeyDown(KeyCode.K);
    }

    /// <summary>
    /// Checks if restart was pressed this frame (mobile or keyboard).
    /// </summary>
    public bool GetRestartPressed()
    {
        return RestartPressed || Input.GetKeyDown(KeyCode.T);
    }
}
