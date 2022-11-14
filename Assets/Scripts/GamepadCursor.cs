using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;

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

    private bool controlSchema = false;

    public PauseMenu menu;

    private bool previousMouseState;
    private Mouse virtualMouse;
    private Camera mainCamara;

    void Awake(){
        cursorTransform = GameObject.Find("Cursor").GetComponent<RectTransform>();
        canvas = GameObject.Find("UIGame").GetComponent<Canvas>();
        canvasRectTransform = GameObject.Find("UIGame").GetComponent<RectTransform>();

        playerInputs = new PlayerInputAction();
        Cursor.visible = false;
    }

    private void Update() {
        playerInputs.Menú.Navigate.performed += onControlsChange;
        playerInputs.Menú.Navigate.Enable();
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

        InputUser user = default(InputUser);

        InputUser.PerformPairingWithDevice(virtualMouse, user);

        if(cursorTransform != null){
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
        //playerInput.controlsChangedEvent.AddListener(onControlsChange);

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

/// We read the right stick value, multiply it by the cursor speed and the delta time, and then add it to
/// the current position of the virtual mouse. We then clamp the new position to the screen, and update
/// the position and delta values of the virtual mouse.
/// 
/// We also check if the West button is pressed, and if it is, we update the virtual mouse state to reflect
/// that.
/// 
/// Finally, we call the AnchorCursor function, which we'll look at next.
    private void UpdateMotion(){
        if(virtualMouse == null || Gamepad.current == null){
            return;
        }

        //movement
        Vector2 deltaValue = Gamepad.current.rightStick.ReadValue();
        deltaValue *= cursorSpeed * Time.deltaTime;


        Vector2 currentPosition = virtualMouse.position.ReadValue();

        Vector2 newPosition = currentPosition + deltaValue;

        newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
        newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);

        cursorTransform.transform.position = newPosition;

        //clic button
        bool aButtonIsPressed = Gamepad.current.xButton.IsPressed();

        if(previousMouseState !=  aButtonIsPressed){
            Mouse.current.CopyState<MouseState>(out var mouseState);

            mouseState = mouseState.WithButton(MouseButton.Left,aButtonIsPressed);
            InputState.Change(Mouse.current, mouseState);

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




/// <param name="context">The context of the action.</param>
    private void onControlsChange(InputAction.CallbackContext context){

    if(SceneManager.GetActiveScene().name != "StartMenu" ){
        if(menu.GameIsPaused){
            Cursor.visible = false;
            Debug.Log("pausa");
            cursorTransform.gameObject.SetActive(true);
            previousControlScheme = gamepascheme;
        }
        else{
            Cursor.visible = true;            
            cursorTransform.gameObject.SetActive(false);
            previousControlScheme = mouseScheme;
        }
    }
    else{
        Cursor.visible = true;
        cursorTransform.gameObject.SetActive(false);
    }
        currentMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
    }

}


