/*
    Created by: Dawnosaur
    Modified by: ArgoTitan AKA Alfredo
*/
using System.Collections;
using UnityEngine;

public class PlayerMovemente : MonoBehaviour
{
    //Scriptable object which holds all the player's movement parameters. If you don't want to use it.
    //Just paste in all the parameters, though you will need to manuly change all reference in this script.

    //HOW TO: to add the scriptable object, right-click in the project window -> create -> Player Info
    //Next, drag it into the slot in player movement on your player.

    public PlayerMovementData Data;

    #region Variables

        //components
        public Rigidbody RB { get; private set; }

        //Variables control the various actions the player's can perform at any time.
        //These fields which can be public allowing for other scripts to read them.
        //but can only be privately written to.
        public bool IsFacingRight { get; private set; }
        public bool IsJumping { get; private set; }
        public bool IsWallJumping { get; private set; }
        
        //Timers (also all fields, could be private and a method returning a bool could be used)
        public float LastOnGroundTime { get; private set; }
        public float LastOnWallTime { get; private set; }
        public float LastOnWallRightTime { get; private set; }
        public float LastOnWallLeftTime { get; private set; }

        //Jump
        private bool _isJumpCut;
        private bool _isJumpFalling;

        //Wall Jump
        private float _wallJumpStartTime;
        private int _lastWallJumpDir;

        private Vector2 _moveInput;
        public float LastPressedJumpTime { get; private set; }

        //Set all these up in the inspector
        [Header("Checks")]
        [SerializeField] private Transform _groundCheckPoint;
        //Size of groundCheck depends in the size of your character generally you want them slightly small than width (for ground) and height (for the wall check)
        [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
        [Space(5)]
        [SerializeField] private Transform _frontWallCheckPoint;
        [SerializeField] private Transform _backWallCheckPoint;
        [SerializeField] private Vector2 _WallCheckSize = new Vector2(0.5f, 1f);

        [Header("Layers & Tags")]
        [SerializeField] private LayerMask _groundLayer;

    #endregion

    private void Awake(){
        RB = GetComponent<Rigidbody>();
    }

    private void Start(){
        SetGravityScale();
        IsFacingRight = true;
    }

    private void Update(){
        #region TIMERS
            LastOnGroundTime -= Time.deltaTime;
            LastOnWallTime -= Time.deltaTime;
            LastOnWallLeftTime -= Time.deltaTime;
            LastOnWallRightTime -= Time.deltaTime;

            LastPressedJumpTime -= Time.deltaTime;
        #endregion

        #region INPUT HANDLER
            _moveInput.x = Input.GetAxisRaw("Horizontal");
            _moveInput.y = Input.GetAxisRaw("Vertical");

            if(_moveInput.x != 0){
                CheckDirectionToFace(_moveInput.x > 0 );
            }

            if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.J)){
                OnJumpInput();
            }

            if(Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.J)){
                OnJumpUpInput();
            }
         #endregion

         #region COLLISION CHECKS
         if(!IsJumping){
            if(Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) && !IsJumping) { //checks if set box overlaps with ground
                LastOnGroundTime = Data.coyoteTime; //if so set the lastGrounded to coyoteTime
            }
            
            //Right Wall Check
            if(((Physics2D.OverlapBox(_frontWallCheckPoint.position, _WallCheckSize, 0, _groundLayer) && IsFacingRight) || (Physics2D.OverlapBox(_backWallCheckPoint.position, _WallCheckSize, 0, _groundLayer) && !IsFacingRight)) && !IsWallJumping){
                LastOnWallRightTime = Data.coyoteTime;
            }

            //Left Wall Check
            if(((Physics2D.OverlapBox(_frontWallCheckPoint.position, _WallCheckSize, 0, _groundLayer) && !IsFacingRight) || (Physics2D.OverlapBox(_backWallCheckPoint.position, _WallCheckSize, 0, _groundLayer) && IsFacingRight)) && !IsWallJumping){
                LastOnWallLeftTime = Data.coyoteTime;
            }
         }
         #endregion

         #region JUMP CHECKS
            if(IsJumping && RB.velocity.y <0){
                IsJumping = false;

                if(!IsWallJumping){
                    _isJumpFalling = true;
                }
            }
            
            if(IsWallJumping && Time.time - _wallJumpStartTime > Data.wallJumpTime){
                IsWallJumping = false;
            }

            if(LastOnGroundTime > 0 && !IsJumping && !IsWallJumping){
                _isJumpCut = false;

                if(!IsJumping){
                    _isJumpFalling = false;
                }
            }

            //Jump
            if(CanJump() && LastPressedJumpTime > 0){
                IsJumping = true;
                IsWallJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;
                Jump();
            }

            //WALL JUMP
            else if(CanWallJump() && LastPressedJumpTime > 0){
                IsWallJumping = true;
                IsJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;
                _wallJumpStartTime = Time.time;
                _lastWallJumpDir = (LastOnWallRightTime >0) ? -1: 1;

                WallJump(_lastWallJumpDir);
            }
         #endregion

         #region GRAVITY
            //Higher gravity if we've released the jump input or are falling
            if(RB.velocity.y < 0 && _moveInput.y < 0){
                //Much higher gravity if holding down
                SetGravityScale();
                //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else if(_isJumpCut){
                //Higher gravity if jump button released
                SetGravityScale();
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else if((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshhold){
                SetGravityScale();
            }
            else if(RB.velocity.y < 0){
                //Higher gravity if falling
                SetGravityScale();
                //caps maximum fall speed, so when falling over large distance we don't accelerate to insanely high speed
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
            }
            else{
                //Default gravity if standing on a platform or moving upwards;
                SetGravityScale();
            }
         #endregion
    }

    private void FixedUpdate(){
        //Handle run
        if(IsWallJumping){
            Run(Data.wallJumpRunLerp);
        }
        else{
            Run(1);
        }
    }

    #region INPUT CALLBACKS

        //Methods which whandle input detected in Update()
        public void OnJumpInput(){
            LastPressedJumpTime = Data.jumpInputBufferTime;
        }

        public void OnJumpUpInput(){
            if(CanJumpCut() || CanWallJumpCut()){
                _isJumpCut = true;
            }
        }

    #endregion

    #region GENERAL METHODS
        public void SetGravityScale(){
            RB.useGravity = true;
        }
    #endregion

    #region RUN METHODS
        private void Run(float lerpAmount){
            //Calculate the direction we want to move in and our desired velocity
            float targetSpeed = _moveInput.x * Data.runMaxSpeed;
            //we can reduce are control using Lerp() this smooths changes to are direction and speed
            targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

            #region Calculate accelRate
                float accelRate;

                //Gets an acceleration value based on if we are accelerating (includes turning)
                //or trying to decelerate(Stop). As well as applying a multiplier if we're air borne.
                if(LastOnGroundTime > 0){
                    accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
                }
                else{
                    accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
                }
            #endregion

            #region Add Bonus Jump Apex Acceleration
                //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
                if((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshhold){
                    accelRate *= Data.jumpHangAccelerationMult;
                    targetSpeed *= Data.jumpHangMaxSpeedMult;
                }
            #endregion

            #region Conserve Momentum
                //we won't slow the player down if they are moving in their desired direction but at a greater speed than their maxspeed
                if(Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0 ){
                    //prevent any decelration form happening, or in other words conseve are current momentum
                    //you could experiment with allowing for the player to slightly increase their speed whilst in the "state"
                    accelRate = 0;
                }
            #endregion

            //calculate difference between current velocity and desired velocity
            float speedDif = targetSpeed - RB.velocity.x;
            //Calculate force along x-axis to apply to the player
            float movement = speedDif * accelRate;

            //Conserve this to a vector and apply to rigidbody
            RB.AddForce(movement * Vector2.right, ForceMode.Force);

            /*
                * For those interested here is what AddForce() will do
		        * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		        * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
            */
        }
        private void Turn(){

            //store scale and flips the player along the x axis.
            Vector3 scale = transform.localScale;
            scale.x *= 1;
            transform.localScale = scale;

            IsFacingRight = !IsFacingRight;
        }
    #endregion

    #region JUMP METHODS
        private void Jump(){
            
            //Ensures we can't call Jump multiple times from one press
            LastPressedJumpTime = 0;
            LastOnGroundTime = 0;

            #region Perform Jump
                //we increase the force applied if we are falling
                //this means we'll always feel like we jump the same amount
                //(setting the player's Y velocity to 0 beforehand will likely work the same, but i find this more elegant :D)
                float force = Data.jumpForce;
                if(RB.velocity.y < 0 ){
                    force -= RB.velocity.y;
                }

                RB.AddForce(Vector2.up * force, ForceMode.Impulse);
            #endregion
        }

        private void WallJump(int dir){
            //Ensures we can't call wall Jump multiple times from one press
            LastPressedJumpTime = 0;
            LastOnGroundTime = 0;
            LastOnWallLeftTime = 0;
            LastOnWallRightTime = 0;

            #region Perform Wall Jump
                Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
                force.x *= dir; //Apply force in opposite direction of wall

                if(Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x)){
                    force.x -= RB.velocity.y;
                }
                if(RB.velocity.y < 0){
                    force.y -= RB.velocity.y;
                }

                //Unlike in the run we want to use the Impulse mode.
                //The default mode will apoly force instantly ignoring mass
                RB.AddForce(force, ForceMode.Impulse);

            #endregion
        }
    #endregion

    #region CHECK METHODS
        public void CheckDirectionToFace(bool isMovingRight){
            if (isMovingRight != IsFacingRight){
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
    #endregion

    #region EDITOR METHODS
    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_frontWallCheckPoint.position, _WallCheckSize);
        Gizmos.DrawWireCube(_backWallCheckPoint.position, _WallCheckSize);
    }
    #endregion
}
