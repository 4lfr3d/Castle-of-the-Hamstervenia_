using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;

public class PM : MonoBehaviourPunCallbacks
{
    
    public DATA data;

    #region COMPONENTES
        [HideInInspector]
        public Rigidbody2D RB { get; private set; }
        public Animator animator;
        private PlayerInputAction playerInputs;
        private InputAction movement;
        public Player photonPlayer;
        public int id;
        [HideInInspector]
    #endregion

    #region STATE PARAMETERS
        public bool IsFacingRight { get; private set; }   
        public bool IsGrounded { get; private set; }
        public bool IsWalled { get; private set; }
        public bool IsJumping { get; private set; }
        public bool IsWallJumping { get; private set; }
        public bool IsDashing { get; private set; }
    #endregion

    #region TIME PARAMETERS
        public float LastOnGroundTime { get; private set; }
        public float LastOnWallTime { get; private set; }
        public float LastOnWallRightTime { get; private set; }
        public float LastOnWallLeftTime { get; private set; }

        public float LastPressedJumpTime { get; private set; }
        public float LastPressedDashTime {get; private set; }
    #endregion

    #region JUMP
        private bool _isJumpCut;
        private bool _isJumpFalling;
    #endregion

    #region WALLJUMP
        private float _wallJumpStartTime;
        private int _lastWallJumpDir;
    #endregion

    #region DASH
        private int _dashesLeft;
        private bool _dashRefilling;
        private Vector2 _lastDashDir;
        private bool _isDashAttacking;
    #endregion

    #region INPUTS
        private Vector2 _moveInput;
    #endregion

    private void Awake(){
        RB = GetComponent<Rigidbody2D>();
        playerInputs = new PlayerInputAction();
    }

    private void Start()
    {
        SetGravityScale(data.gravityScale);
        IsFacingRight = true;
    }

    private void Update()
    {
        /* Debug.Log("Grounded: " + IsGrounded);
        Debug.Log("Walled: " + IsWalled); */
        Vector2 a=movement.ReadValue<Vector2>();
        #region TIMERS
            LastOnGroundTime -= Time.deltaTime;
            LastOnWallTime -= Time.deltaTime;
            LastOnWallRightTime -= Time.deltaTime;
            LastOnWallLeftTime -= Time.deltaTime;

            LastPressedJumpTime -= Time.deltaTime;
            LastPressedDashTime -= Time.deltaTime;

        #endregion

        #region COLLISION CHECK
            if(!IsDashing && !IsJumping){
                if(IsGrounded == true){
                    LastOnGroundTime = data.coyoteTime;
                }
                if(IsWalled == true){
                    LastOnWallRightTime = data.coyoteTime;
                }
                if(IsWalled == true){
                    LastOnWallLeftTime = data.coyoteTime;
                }

                LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
            }
            
        #endregion

        #region JUMP CHECKS
            if(IsJumping && RB.velocity.y < 0){
                IsJumping = false;

                if(!IsWallJumping){
                    _isJumpFalling = true;
                    animator.SetBool("isFalling",true);
                }
            }
            if(IsWallJumping && Time.time - _wallJumpStartTime > data.wallJumpTime){
                IsWallJumping = false;
            }
            if(LastOnGroundTime > 0 && !IsJumping && !IsWallJumping){
                _isJumpCut = false;

                if(!IsJumping){
                    _isJumpFalling = false;
                    animator.SetBool("isFalling", false);
                }
            }

            if(!IsDashing){
                if(CanWallJump() && LastPressedJumpTime > 0){
                    IsWallJumping = true;
                    IsJumping = false;
                    _isJumpCut = false;
                    _isJumpFalling = false;
                    _wallJumpStartTime = Time.time;
                    _lastWallJumpDir = (LastOnWallRightTime >0) ? -1 : -1;

                    WallJump(_lastWallJumpDir);
                }
            }
        
        #endregion

        #region DASH
            CanDash();
        #endregion

        #region URAVITY
            if(!_isDashAttacking){
                if(RB.velocity.y < 0 && a.y < 0 ){
                SetGravityScale(data.gravityScale * data.fastFallGravityMult);

                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -data.maxFastFallSpeed));
                }
                else if(_isJumpCut){
                    SetGravityScale(data.gravityScale * data.jumpCutGravityMult);
                    RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -data.maxFallSpeed));
                }
                else if((IsJumping || IsWallJumping || _isJumpFalling) && (Mathf.Abs(RB.velocity.y) < data.jumpHangTimeTreshold)){
                    SetGravityScale(data.gravityScale * data.jumpHangGravityMult);
                }
                else if(RB.velocity.y < 0){
                    SetGravityScale(data.gravityScale * data.fallGravityMult);
                    RB.velocity = new Vector2 (RB.velocity.x, Mathf.Max(RB.velocity.y, -data.maxFallSpeed));
                }
                else{
                    SetGravityScale(data.gravityScale);
                }
            }
            
        #endregion

    }    

    private void OnEnable(){
        movement = playerInputs.Player.Movement;
        playerInputs.Enable();

        playerInputs.Player.Jump.performed += DoJump;
        playerInputs.Player.Jump.Enable();

        playerInputs.Player.Dash.performed += DoDash;
        playerInputs.Player.Dash.Enable();
    }

    private void OnDisable(){
        movement.Disable();
        playerInputs.Player.Jump.Disable();
        playerInputs.Player.Dash.Disable();
    }

    private void FixedUpdate(){
        Vector2 a=movement.ReadValue<Vector2>();
        if(!IsDashing){
            if(IsWallJumping){
                Run(data.wallJumpRunLerp, a);
            }
            else{
                Run(1, a);
            }
        }
        else if(_isDashAttacking){
            Run(data.dashEndRunLerp, a);
        }
        
    }

    private void DoJump(InputAction.CallbackContext context){
        if(CanJump()){
            IsJumping = true;
            IsWallJumping = false;
            _isJumpCut = false;
            _isJumpFalling = false;
            Jump();
        }
    }

    private void DoDash(InputAction.CallbackContext context){
        if(CanDash()){
            Sleep(data.dashSleepTime);

            if(_moveInput != Vector2.zero){
                _lastDashDir = _moveInput;
            }
            else{
                _lastDashDir = IsFacingRight ? Vector2.right : Vector2.left;
            }
            

            IsDashing = true;
            IsJumping = false;
            IsWallJumping = false;
            _isJumpCut = false;

            StartCoroutine(nameof(StartDash), _lastDashDir);
        }
    }

    #region INPUT CALLBACKS
        public void OnJumpUpInput(){
            if(CanJumpCut() || CanWallJumpCut()){
                _isJumpCut = true;
            }
        }
    #endregion

    #region GENERAL METHODS
        public void SetGravityScale(float scale){
            RB.gravityScale = scale;
        }
        private void Sleep(float duration){
            StartCoroutine(nameof(PerformSleep),duration);
        }
        private IEnumerator PerformSleep(float duration){
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1;
        }
    #endregion

    #region RUN METHODS
        private void Run(float lerpAmount, Vector2 a){
            float targetSpeed = a.x * data.runMaxSpeed;

            if(a.x != 0){
                CheckDirectionToFace(a.x > 0);
            }

            targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

            #region CALCULATE ACCEL
                float accelRate;
                if(LastOnGroundTime > 0){
                    accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount : data.runDeccelAmount;
                }
                else{
                    accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount * data.accelInAir : data.runDeccelAmount * data.deccelInAir;
                }
            #endregion

            #region ADD BONUS JUMP APEX ACCELERATION
                if((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < data.jumpHangTimeTreshold){
                    accelRate *= data.jumpHangAccelerationMult;
                    targetSpeed *= data.jumpHangMaxSpeedMult;
                }
            #endregion

            float speedDif = targetSpeed - RB.velocity.x;
            float movement = speedDif * accelRate;

            RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

            animator.SetFloat("speed",Mathf.Abs(movement));
            if(Mathf.Abs(movement)>0.1){
                SoundManager.instance.MovementSFX();
                animator.SetBool("isRunning",true);
            }
            else{
                animator.SetBool("isRunning",false);
            }

        }

        private void Turn(){
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            IsFacingRight = !IsFacingRight;
        }

    #endregion

    #region JUMP METHODS
        private void Jump(){
            LastPressedJumpTime = 0;
            LastOnGroundTime = 0;

            #region PERFORM JUMP
                float force = data.jumpForce;
                if(RB.velocity.y < 0){
                    force -= RB.velocity.y;
                }

                RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);


                if(force > 0){
                    SoundManager.instance.JumpSFX();
                    animator.SetTrigger("isJumping");
                }
                else {
                    animator.ResetTrigger("isJumping");
                }

            #endregion
        }

        private void WallJump(int dir){
            LastPressedJumpTime = 0;
            LastOnGroundTime = 0;
            LastOnWallRightTime = 0;
            LastOnWallLeftTime = 0;

            #region PERFORM WALL JUMP
                Vector2 force = new Vector2 (data.wallJumpForce.x, data.wallJumpForce.y);
                force.x *= dir;

                if(Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x)){
                    force.x -= RB.velocity.x;
                }
                if(RB.velocity.y < 0){
                    force.y -= RB.velocity.y;
                }

                RB.AddForce(force, ForceMode2D.Impulse);
            #endregion
        }
    #endregion

    #region DASH METHODS
        private IEnumerator StartDash(Vector2 dir){
            animator.SetBool("isDash", true);
            SoundManager.instance.TaroDash();
            LastOnGroundTime = 0;
            LastPressedDashTime = 0;

            float startTime = Time.time;

            _dashesLeft --;
            _isDashAttacking = true;

            SetGravityScale(0);

            while(Time.time - startTime <= data.dashAttackTime){
                RB.velocity = dir.normalized * data.dashSpeed;
                yield return null;
            }

            startTime = Time.time;

            _isDashAttacking = false;

            SetGravityScale(data.gravityScale);
            RB.velocity = data.dashEndSpeed * dir.normalized;

            /*while(Time.time - startTime <= data.dashEndTime){
                yield return null;
                animator.SetTrigger("isDash");
            }*/

            animator.SetBool("isDash", false);
            IsDashing = false;
        }

        private IEnumerator RefillDash(int amount){
            _dashRefilling = true;
            yield return new WaitForSeconds(data.dashRefillTime);
            _dashRefilling = false;
            _dashesLeft = Mathf.Min(data.dashAmount, _dashesLeft + 1);
        }
    #endregion

    #region CHECK METHODS

        public void CheckDirectionToFace(bool isMovingRight){
            if(isMovingRight != IsFacingRight){
                Turn();
            }
        }

        private bool CanJump(){
            return LastOnGroundTime > 0 && !IsJumping;
        }

        private bool CanWallJump(){
            return LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping || (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
        }

        private bool CanJumpCut(){
            return IsJumping && RB.velocity.y > 0;
        }

        private bool CanWallJumpCut(){
            return IsWallJumping && RB.velocity.y > 0;
        }

        private bool CanDash(){
            if(!IsDashing && _dashesLeft < data.dashAmount && LastOnGroundTime > 0 && !_dashRefilling){
                StartCoroutine(nameof(RefillDash),1);
            }

            return _dashesLeft > 0;
        }

        private void OnCollisionEnter2D(Collision2D other){
            if(other.collider.CompareTag("Ground")){
                IsGrounded = true;
                animator.SetBool("isGrounded",true);
            }
            if(other.collider.CompareTag("Wall")){
                IsWalled = true;
                animator.SetBool("isWalled",true);
            }
        }

        private void OnCollisionExit2D(Collision2D other){
            if(other.collider.CompareTag("Ground")){
                IsGrounded=false;
                animator.SetBool("isGrounded",false);
            }
            if(other.collider.CompareTag("Wall")){
                IsWalled = false;
                animator.SetBool("isWalled",false);
            }
        }
    #endregion

    #region MULTIPLAYER
        /*
            <summary>
                Inicializar la informacion del player actual
            </summary>
            <param name="player">Data de player</param>
        */
        public void Init(Player player){
            photonPlayer = player; //Asignar el player actual
            id = player.ActorNumber; //Guardar el id del player
            GameController.instance.players[id-1] = this; //Asignarlo a la lista de player dentro del game controller

            if(!photonView.IsMine){
                RB.isKinematic = true;
            }
        }
    #endregion

}
