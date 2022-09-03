/*
    Created by: @DawnosaurDev at youtube.com/c/DawnosaurStudios
    Modified by: ArgoTitan AKA Alfredo Angulo
*/
using UnityEngine;

[CreateAssetMenu(menuName = "Player Data")] //Create a new PlayerData object by right clicling in the Project Menu then Create/Player/Player Data and drag onto the player

public class REALplayerMovementData : ScriptableObject
{

    [Header("Gravity")]
    [HideInInspector] public float gravityStrength; //Downwards force (gravity) needed for the desired jumpHeight and jumpTimetoApex.
    [HideInInspector] public float gravityScale; //Strength of the player's gravity as a multiplier of gravity (set in projectsettings/Physics2D).

    [Space(5)]
    public float fallGravityMult; //Multiplier to the player's gravityScale when falling
    public float maxFallSpeed; //Maxium fall speed (terminal velocity) of the player when falling
    [Space(5)]
    public float fastFallGravityMult; //Larger multiplier to the player's gravityScale when they are falling and a downwards input is pressed.
    public float maxFastFallSpeed; //Maxium fall speed (terminal velocity) of the player when performing a faster fall.

    [Space(20)]

    [Header("Run")]
    public float runMaxSpeed; //Target speed we want the player to reach
    public float runAcceleration; //The speed at wich our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
    [HideInInspector] public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player
    public float runDecceleration; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
    [HideInInspector] public float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player
    [Space(5)]
    [Range(0f, 1)] public float accelInAir; //Multipliers applied to acceleration rate when airborne
    [Range(0f, 1)] public float deccelInAir;
    [Space(5)]
    public bool doConserveMomentum = False;

    [Space(20)]

    [Header("Jump")]
    public float jumpHeight; //Height of the player's jump
    public float jumpTimetoApex; //Time between applying the jump force and reaching the desired jump height. These values also control the player's gravity and jump force
    [HideInInspector] public float jumpForce; //The actual force applied (upwards) to the player when they jump

    [Header("Both Jumps")]
    public float jumpCutGravityMult; //Multiplier to increse gravity if the player releases the jump button while still jump
    [Range(0f, 1)] public float jumpGravityMult; //Reduces gravity whule close to the apex (desired max height) of the jump
    public float jumpHangTimeTreshhold; //speeds (close to 0) where the players will experience extra "jump hang". the player's velocity.y is closest to 0 at the jump's apex (think of the gradient of a parabola or quadratic function)
    [Space(0.5f)]
    public float jumpHangAccelerationMult;
    public float jumpHangMaxSpeedMult;

    [Header("Wall Jump")]
    public Vector3 WallJumpForce; //The actual force (this time set by us) applied to the player when wall jumping
    [Space(5)]
    [Range(0f, 1f)] public float wallJumpRunLerp; //Reduces the effect of player's movement while wall jumping
    [Range(0f, 1.5f)] public float wallJumpTime; //Time after wall jumping the player's movemente is slowed for
    public bool doTurnOnWallJump; //Player will rotate to face wall jumping direction

    [Space(20)]

    [Header("Slide")]
    public float slideSpeed;
    public float slideAccel;

    [Header("Assists")]
    [Range(0.01f, 0.5f)] public float coyoteTime; //Grace period after falling off a platform, where you can still jump
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime; //Grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met.
    
    [Space(20)]

    [Header("Dash")]
    public int dashAmount;
    public float dashSpeed;
    public float dashSleepTime; //Duration for which the game freezes when we press dash but before we read directional input and apply a force
    [Space(5)]
    public float dashAttackTime;
    [Space(5)]
    public float dashEndTime; //Time after you finish the initial drag phase, smoothing the transition back to idle (or any standard state)
    public Vector3 dashEndSpeed; //Slows down player, makes dash fell more responsive (used in celeste)
    [Range(0f, 1f)] public float dashEndRunLerp; //Slows the afect to player movement while dashing
    [Space(5)]
    [Range(0.01f, 0.5f)] public float dashInputBufferTime;

    private void OnValidate(){
        
        //Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2)
        gravityStrength = -(2 * jumpHeight) / (jumpTimetoApex * jumpTimetoApex);
        
        //Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;
        
        //Calculate are run accleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        //Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimetoApex;

        #region Variable Ranges
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
        runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
        #endregion

    }

}
