using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float horizontalMove;
    public float verticalMove;
    public CharacterController player;
    private Vector3 playerInput;

    public float playerSpeed;
    private Vector3 movePlayer;

    public Camera mainCamera;
    private Vector3 camForward;
    private Vector3 camRight;

    public float gravity = 9.8f;
    public float fallVelocity;

    public float jumpForce;

    public bool isOnSlope = false;
    private Vector3 hitNormal;
    public float SlideVelocity;
    public float slopeForDown;




    // Start 
    void Start()
    {
        player = GetComponent<CharacterController>();
    }

    // Update 
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        playerInput = new Vector3(horizontalMove, 0, verticalMove);
        playerInput = Vector3.ClampMagnitude(playerInput, 1);

        camDirection();

        movePlayer = playerInput.x * camRight + playerInput.z * camForward;

        movePlayer = movePlayer * playerSpeed;
        
        player.transform.LookAt(player.transform.position + movePlayer);

        SetGravity();
        PlayerSkills();

        player.Move(movePlayer *  Time.deltaTime);

        Debug.Log(player.velocity.magnitude);
    }

    public void camDirection()
    {
        camForward = mainCamera.transform.forward;
        camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;

        camForward = camForward.normalized;
        camRight = camRight.normalized;
    }
    public void SetGravity()
    {
        if (player.isGrounded)
        {
            fallVelocity = -gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        else
        {
            fallVelocity -= gravity * Time.deltaTime;
            movePlayer.y = fallVelocity;
        }
        SlideDown();
    }

    //FUNCION PARA HABILIDADES
    public void PlayerSkills()
    {
        if (player.isGrounded && Input.GetButtonDown("Jump"))
        {
            fallVelocity = jumpForce;
            movePlayer.y = fallVelocity;
        }
    }

    //FUNCION OBJETO CHOCA CON OTROS
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitNormal = hit.normal;
    }

    //FUNCION COMPARA SI EL JUGADOR ESTA O NO EN UNA RAMPA
    public void SlideDown()
    {
        //isOnSlope = angulo >= angulo maximo del charactercontroller;
        isOnSlope = Vector3.Angle(Vector3.up, hitNormal) >= player.slopeLimit;

        if (isOnSlope) {
            movePlayer.x += ((1f - hitNormal.y) * hitNormal.x) * SlideVelocity;
            movePlayer.z += ((1f- hitNormal.y) * hitNormal.z) * SlideVelocity;

            movePlayer.y += slopeForDown;
        
        }
    }

}
