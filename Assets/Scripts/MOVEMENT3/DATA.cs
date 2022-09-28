using UnityEngine;

    [CreateAssetMenu(menuName = "PlayerData")]

public class DATA : ScriptableObject
{

    [Header("Gravity")]
    [HideInInspector] public float gravityStrenght;
    [HideInInspector] public float gravityScale;

    [Space(5)]
    public float fallGravityMult;
    public float maxFallSpeed;
    [Space(5)]
    public float fastFallGravityMult;

    public float maxFastFallSpeed;

    [Space(20)]

    [Header("Run")]
    public float runMaxSpeed;
    public float runAcceleration;
    [HideInInspector] public float runAccelAmount;
    public float runDecceleration;
    [HideInInspector] public float runDeccelAmount;
    [Space(10)]
    [Range(0.01f,1)] public float accelInAir;
    [Range(0.01f,1)] public float deccelInAir;
    public bool doConserveMomentum = false;

    [Header("Jump")]
    public float jumpHeight;
    public float jumpTimeToApex;
    [HideInInspector] public float jumpForce;

    [Header("Both Jumps")]
    public float jumpCutGravityMult;
    [Range(0f,1)] public float jumpHangGravityMult;
    public float jumpHangTimeTreshold;
    [Space(0.5f)]
    public float jumpHangAccelerationMult;
    public float jumpHangMaxSpeedMult;

    [Header("Wall Jump")]
    public Vector2 wallJumpForce;
    [Space(5)]
    [Range(0f, 1f)] public float wallJumpRunLerp;
    [Range(0f, 1.5f)] public float wallJumpTime;
    
    [Space(20)]

    [Header("Assists")]
    [Range(0.01f, 0.5f)] public float coyoteTime;
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime;

    [Space(20)]

    [Header("Dash")]
    public int dashAmount;
    public float dashSpeed;
    public float dashSleepTime;
    [Space(5)]
    public float dashAttackTime;
    [Space(5)]
    public float dashEndTime;
    public Vector2 dashEndSpeed;
    [Range(0f,1f)] public float dashEndRunLerp;
    [Space(5)]
    public float dashRefillTime;
    [Space(5)]
    [Range(0.01f, 0.5f)] public float dashInputBufferTime;

    private void OnValidate(){

        gravityStrenght = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        gravityScale = gravityStrenght / Physics2D.gravity.y;
        
        runAccelAmount = (50 * runAcceleration ) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration ) / runMaxSpeed;

        jumpForce = Mathf.Abs(gravityStrenght) * jumpTimeToApex;

        #region VARIABLE RANGES
            runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
            runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
        #endregion
    }
}
