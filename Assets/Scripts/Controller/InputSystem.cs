using InControl;


/// <summary>
/// The Input System class for the game, based on InControl.
/// </summary>
public class InputSystem : PlayerActionSet
{
    // Move Values
    public PlayerTwoAxisAction Move;
    public PlayerOneAxisAction MoveHorizontal;
    public PlayerOneAxisAction MoveVertical;
    public PlayerAction MoveUp;
    public PlayerAction MoveDown;
    public PlayerAction MoveLeft;
    public PlayerAction MoveRight;

    // Look Value
    public PlayerTwoAxisAction Look;
    public PlayerOneAxisAction LookHorizontal;
    public PlayerOneAxisAction LookVertical;
    public PlayerAction LookUp;
    public PlayerAction LookDown;
    public PlayerAction LookLeft;
    public PlayerAction LookRight;
    public PlayerAction LookZoom;


    // Action Values

    public PlayerAction Jump;
    public PlayerAction Hook;
    public PlayerAction Dash;
    public PlayerAction Pause;
    public PlayerAction Hud;
    public PlayerAction Interact;
    public PlayerAction PanicKill;

    public InputSystem()
    {
        MoveUp = CreatePlayerAction("Move Up");
        MoveDown = CreatePlayerAction("Move Down");
        MoveLeft = CreatePlayerAction("Move Left");
        MoveRight = CreatePlayerAction("Move Right");

        LookUp = CreatePlayerAction("Look Up");
        LookDown = CreatePlayerAction("Look Down");
        LookLeft = CreatePlayerAction("Look Left");
        LookRight = CreatePlayerAction("Look Right");
        LookZoom = CreatePlayerAction("Look Zoom");

        Jump = CreatePlayerAction("Jump");
        Hook = CreatePlayerAction("Hook");
        Dash = CreatePlayerAction("Dash");
        Pause = CreatePlayerAction("Pause");
        Hud = CreatePlayerAction("Hud");
        Interact = CreatePlayerAction("Interact");
        PanicKill = CreatePlayerAction("Panic Kill");

        MoveHorizontal = CreateOneAxisPlayerAction(MoveLeft, MoveRight);
        MoveVertical = CreateOneAxisPlayerAction(MoveDown, MoveUp);

        Move = CreateTwoAxisPlayerAction(MoveLeft, MoveRight, MoveDown, MoveUp);

        LookHorizontal = CreateOneAxisPlayerAction(LookLeft, LookRight);
        LookVertical = CreateOneAxisPlayerAction(LookDown, LookUp);

        Look = CreateTwoAxisPlayerAction(LookLeft, LookRight, LookDown, LookUp);
        
    }

    /// <summary>
    /// Get the currently used device.
    /// </summary>
    /// <returns>InputDeviceClass enum for last used device</returns>
    public InputDeviceClass GetInputDeviceClass()
    {
        return this.ActiveDevice.DeviceClass;
    }

}
