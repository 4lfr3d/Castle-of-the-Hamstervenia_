using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class GamepadCursor : MonoBehaviour
{
    private PlayerInputAction playerInputs;

    //[SerializeField] private PlayerInput playerInput;
    [SerializeField] private RectTransform cursorTransform;
    [SerializeField] private Canvas canvas; 
    [SerializeField] private float cursorSpeed = 1000f;
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private float padding = 35f;

    private string previousControlScheme = ""; 
    private const string gamepascheme = "Control";
    private const string mouseScheme = "Keyboard&Mouse";

    private Mouse currentMouse;

    private bool contorlSchema = false;


    private bool previousMouseState;
    private Mouse virtualMouse;
    private Camera mainCamara;

    void Awake(){
        playerInputs = new PlayerInputAction();
    }

/// It enables the virtual mouse, sets the main camera, sets the current mouse, adds the virtual mouse
/// to the input system, pairs the virtual mouse with the user, sets the position of the virtual mouse,
/// and adds a listener to the onAfterUpdate event.

    private void OnEnable(){

        mainCamara = Camera.main;

        currentMouse = Mouse.current;

        if(virtualMouse == null){
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if (!virtualMouse.added){
            InputSystem.AddDevice(virtualMouse);
        }

        //InputUser.PerformPairingWithDevice(virtualMouse, playerInputs.user);

        if(cursorTransform != null){
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
        //playerInput.controlsChangedEvent.AddListener(onControlsChange);
        playerInputs.Menú.Navigate.performed += onControlsChange;
        playerInputs.Menú.Navigate.Enable();
    }


/* Disabling the virtual mouse, removing the listener to the onAfterUpdate event, and disabling the
Navigate action. */
    private void OnDisable(){
        if (virtualMouse != null && virtualMouse.added) InputSystem.RemoveDevice(virtualMouse);
        InputSystem.onAfterUpdate -= UpdateMotion;
        playerInputs.Menú.Navigate.Disable();

        //playerInput.ControlsChangeEvent -= onControlsChange;
        //playerInput.controlsChangedEvent.RemoveListener(onControlsChange);
    }

/// We read the left stick value, multiply it by the cursor speed and the delta time, and then add it to
/// the current position of the virtual mouse. We then clamp the new position to the screen, and update
/// the position and delta values of the virtual mouse.
/// 
/// We also check if the A button is pressed, and if it is, we update the virtual mouse state to reflect
/// that.
/// 
/// Finally, we call the AnchorCursor function, which we'll look at next.
    private void UpdateMotion(){
        if(virtualMouse == null || Gamepad.current == null){
            return;
        }

        //movement
        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        deltaValue *= cursorSpeed * Time.deltaTime;


        Vector2 currentPosition = virtualMouse.position.ReadValue();

        Vector2 newPosition = currentPosition + deltaValue;

        newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
        newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);

        //clic button

        bool aButtonIsPressed = Gamepad.current.aButton.IsPressed();

        if(previousMouseState !=  aButtonIsPressed){
            virtualMouse.CopyState<MouseState>(out var mouseState);

            mouseState.WithButton(MouseButton.Left,aButtonIsPressed);
            InputState.Change(virtualMouse, mouseState);

            previousMouseState = aButtonIsPressed;

        }

        AnchorCursor(newPosition);

    }

/// It anchors the cursor to the canvas.
/// <param name="Vector2">The position of the cursor.</param>
    private void AnchorCursor(Vector2 position){
        Vector2 anchoredPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, canvas.renderMode
         == RenderMode.ScreenSpaceOverlay ? null : mainCamara, out anchoredPosition);
    }


/// If the current control scheme is the mouse scheme, then disable the cursor transform and enable the
/// mouse cursor. If the current control scheme is the gamepad scheme, then enable the cursor transform
/// and disable the mouse cursor
/// <param name="context">The context of the callback.</param>
    private void onControlsChange(InputAction.CallbackContext context){
        if(contorlSchema && previousControlScheme != mouseScheme){
            cursorTransform.gameObject.SetActive(false);
            Cursor.visible = false;
            InputState.Change(virtualMouse.position, currentMouse.position.ReadValue());
            AnchorCursor(currentMouse.position.ReadValue());
            previousControlScheme = gamepascheme;
            contorlSchema = true;
        }
        else if(!contorlSchema && previousControlScheme != gamepascheme){
            cursorTransform.gameObject.SetActive(true);
            Cursor.visible = true;
            currentMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
            previousControlScheme = mouseScheme;
            contorlSchema = false;
            //playerInputs.currentControlScheme == gamepascheme
        }
    }

}


