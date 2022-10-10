using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Run : MonoBehaviour
{
    public DATA data;

    #region COMPONENTES
        public Rigidbody2D RB { get; private set; }
        public Animator animator;
    #endregion

    #region STATE PARAMETERS
        public bool IsFacingRight { get; private set; }
        public bool IsGrounded { get; private set; }
    #endregion

    #region TIME PARAMETERS
        public float LastOnGroundTime { get; private set; }
    #endregion

    #region INPUTS
        private Vector2 _moveInput;
    #endregion

    private void Awake(){
        RB = GetComponent<Rigidbody2D>();
    }

    private void Start(){
        SetGravityScale(data.gravityScale);
        IsFacingRight = true;
    }

    private void Update(){
        #region Timers
            LastOnGroundTime -= Time.deltaTime;
        #endregion
    }

    public void Run(InputAction.CallbackContext context){
        if(context.performed){
            
        }
        Debug.Log("RUN");

    }

    

    #region CHECK METHODS
        public void CheckDirectionToFace(bool isMovingRight){
            if(isMovingRight != IsFacingRight){
                Turn();
            }
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
}
