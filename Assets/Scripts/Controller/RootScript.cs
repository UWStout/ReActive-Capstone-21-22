using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

///This is the root script that will hold multiple static classes and easy-to-access references for multiple objects.
public class RootScript : MonoBehaviour
{
    /// <summary>
    /// A static class for the Root Script which can be called from any script in the game.
    /// </summary>
    public static RootScript Root;

    /// <summary>
    /// A static class for the Input System that can be called from any script in the game.
    /// </summary>
    public static InputSystem Input;

    /// <summary>
    /// A static class for the Queue System.
    /// </summary>
    public static TimeQueue GlobalQueue;

    /// <summary>
    /// A static class for the Level Loading System.
    /// </summary>
    public static LevelLoader SceneLoader;

    /// <summary>
    /// The character move script
    /// </summary>
    public static CharacterMove CharMove;

    /// <summary>
    /// The Canvas GameObject that contains all the HUD elements.
    /// </summary>
    public static UIControl UIElements;

    /// <summary>
    /// The Game Manager object that handles game values.
    /// </summary>
    public static GameManager TheGameManager;

    /// <summary>
    /// The Player's Camera script
    /// </summary>
    public static TP_Cam PlayerCamera;

    /// <summary>
    /// The Player's Camera script
    /// </summary>
    public static SoundSystem SoundManager;

    /// <summary>
    /// The Player's Camera script
    /// </summary>
    public static MusicManager MusicManager;
    
    
    void Awake()
    {
        if (RootScript.Root == null)
        {
            DontDestroyOnLoad(gameObject);

            RootScript.Root = this;
            RootScript.GlobalQueue = GetComponent<TimeQueue>();
            RootScript.SceneLoader = GetComponent<LevelLoader>();
            RootScript.UIElements = FindObjectOfType<UIControl>();
            RootScript.SoundManager = FindObjectOfType<SoundSystem>();
            RootScript.MusicManager = FindObjectOfType<MusicManager>();
            //Test

            InitiateInputSystem();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Update()
    {
        
    }

    /// <summary>
    /// The function that configures the default values for the input system.
    /// </summary>
    public void InitiateInputSystem()
    {
        RootScript.Input = new InputSystem();

        //Keyboard / Mouse
        Input.MoveUp.AddDefaultBinding(Key.W);
        Input.MoveDown.AddDefaultBinding(Key.S);
        Input.MoveLeft.AddDefaultBinding(Key.A);
        Input.MoveRight.AddDefaultBinding(Key.D);

        Input.Jump.AddDefaultBinding(Key.Space);
        Input.Hook.AddDefaultBinding(Mouse.LeftButton);
        Input.Dash.AddDefaultBinding(Key.Shift);
        Input.Pause.AddDefaultBinding(Key.Escape);
        Input.Hud.AddDefaultBinding(Key.I);
        Input.Interact.AddDefaultBinding(Key.E);
        Input.PanicKill.AddDefaultBinding(Key.P);

        Input.LookLeft.AddDefaultBinding(Mouse.NegativeX);
        Input.LookRight.AddDefaultBinding(Mouse.PositiveX);

        Input.LookUp.AddDefaultBinding(Mouse.PositiveY);
        Input.LookDown.AddDefaultBinding(Mouse.NegativeY);
        Input.LookZoom.AddDefaultBinding(Mouse.RightButton);


        //Gamepad

        Input.LookUp.AddDefaultBinding(InputControlType.RightStickUp);
        Input.LookDown.AddDefaultBinding(InputControlType.RightStickDown);
        Input.LookLeft.AddDefaultBinding(InputControlType.RightStickLeft);
        Input.LookRight.AddDefaultBinding(InputControlType.RightStickRight);
        Input.LookZoom.AddDefaultBinding(InputControlType.LeftTrigger);


        Input.MoveUp.AddDefaultBinding(InputControlType.LeftStickUp);
        Input.MoveDown.AddDefaultBinding(InputControlType.LeftStickDown);
        Input.MoveLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
        Input.MoveRight.AddDefaultBinding(InputControlType.LeftStickRight);

        Input.Jump.AddDefaultBinding(InputControlType.Action1);
        Input.Hook.AddDefaultBinding(InputControlType.RightTrigger);
        Input.Dash.AddDefaultBinding(InputControlType.Action3);
        Input.Pause.AddDefaultBinding(InputControlType.Menu);
        Input.Hud.AddDefaultBinding(InputControlType.RightStickButton);
        Input.Interact.AddDefaultBinding(InputControlType.Action2);
        Input.PanicKill.AddDefaultBinding(InputControlType.Back);
    }
}

