/*
    Created by: Dawnosaur
    Modified by: ArgoTitan AKA Alfredo
*/
using UnityEngine;

[CreateAssetMenu(menuName = "Player Info")] //Create a new PlayerData object by right clicking in the project menu then Create/Player/Player Info and drag onto the player

public class PlayerMovementData : ScriptableObject
{
    [Header("Gravity")]
    [HideInInspector] public float gravityStrength; //Downwards force (gravity) needed for the desired jumpHeight and jumpTimetoApex
    [HideInInspector] public float gravityScale; //Strength of the player's gravity as a multiplier of gravity (set in ProjectSettings/Physiscs2D).
    
    [Space(5)]
    public float fallGravityMult; //Multiplier to the player's gravity when falling
    public float maxFallSpeed; //Maxium fall speed (terminal velocity) of the player when falling.
    [Space(5)]
    public float fastFallGravityMult; //Larger multiplier to the player's gravity shen they are falling and a downwards input is pressed.
    public float maxFastFallSpeed; //Maxium fall speed(terminal velocity) of the player when performing faster fall.

    [Space(20)]

    [Header("Run")]
    public float runMaxSpeed; //Target speed we want the player to reach.
    public float runAcceleration; //The speed at which our player accelerate to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
    [HideInInspector] public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
    public float runDecceleration; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none
    [HideInInspector] public float runDeccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
    [Space(5)]
    [Range(0f,1)] public float accelInAir; //Multipliers applied to acceleration rate when airbone.
    [Range(0f,1)] public float deccelInAir;
    [Space(5)]
    public bool doConserveMomentum = false;

    [Space(20)]

    [Header("Jump")]
    public float jumpHeight; //Height of the player's jump.
    public float jumpTimeToApex; //Time between applying the jump force and reaching the desired jump height these values also control the player's gravity and jump force
    [HideInInspector] public float jumpForce; //The actual force applied (upwards) to the player's when they jump.

    [Header("Both Jumps")]
    public float jumpCutGravityMult; //Multiplier to increase gravity if the player releases the jump button while still jumping
    [Range(0f,1)] public float jumpHangGravityMult; //Reduces gravity while close to the apex (desired max height) of the jump.
    public float jumpHangTimeThreshhold; //Speeds (close to 0) where the player will experience extra "jump hang". The player's velocity.y is closest to 0 at the jump's apex (think of the gradient of a parabola or quadratic function)
    [Space(0.5f)]
    public float jumpHangAccelerationMult;
    public float jumpHangMaxSpeedMult;

    [Header("Wall Jump")]
    public Vector2 wallJumpForce; //The actual force (this time set by us) applied to the player when wall jumping.
    [Space(5)]
    [Range(0f,1f)] public float wallJumpRunLerp; //Reduces the effect of player's movement while wall jumping.
    [Range(0f,1.5f)] public float wallJumpTime; //Time after wall jumping the player's movement is slowed for.
    public bool doTurnOnWallJump = false; //Player will rotate to face wall jumping direction.

    [Space(20)]

    [Header("Assists")]
    [Range(0f,0.5f)] public float coyoteTime; //Grace period after falling off a plataform, when you can still jump.
    [Range(0f,0.5f)] public float jumpInputBufferTime; //Grace period after pressing jump where a jump will automatically performed onche the requirement (eg. being grounded) are met.

    //Unity callback, called when the inspector updates
    private void OnValidate(){

        //Calculate gravity stength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2)
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        //Calculate the rigidbody's gravity scale(ie: gravity strength relative to unity's gravity value, see Project Settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;

        //Calculate are run acceleration & deceleration forces using formula: amount = ((1/Time.fixedDeltaTime) * acceleration / runMaxSpeed)
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDeccelAmount) / runMaxSpeed;

        //Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToApex)
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

        #region Variable Ranges
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
        runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
        #endregion
    }
}
