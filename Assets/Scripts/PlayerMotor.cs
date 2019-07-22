using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 moveVector;
    
    private float speed =6.0f;
    private float verticalVelocity = 0.0f;
    private float gravity = 12.0f;
    private float animationDuration = 3.0f;
    private float startTime;
    private bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        if (isDead)
            return;
        if (Time.time - startTime < animationDuration) {
            controller.Move(Vector3.forward*speed*Time.deltaTime);
            return;
        }
        moveVector = Vector3.zero;

        if(controller.isGrounded) {
            verticalVelocity = -0.5f;
        }
        else {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        moveVector.x = Input.GetAxisRaw("Horizontal")*speed;
        moveVector.y = verticalVelocity;
        moveVector.z=speed;
        controller.Move(moveVector * Time.deltaTime);
    }
    public void SetSpeed(float modifier)
    {
        speed = 5.0f +modifier;
    }
    
    //충돌, 숨을떄 쓴다
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.point.z > transform.position.z+ 0.1f && hit.gameObject.tag == "Enemy")
            Death();

        if(hit.gameObject.tag=="Empty"){
            if(Input.GetKeyDown("space")){
                Color blockcolor = hit.gameObject.GetComponentInParent<MeshRenderer>().material.color;
                //Debug.Log(blockcolor);
                ColorPicker colorPicker = new ColorPicker();
                colorPicker.CompareColor(blockcolor);
            }
        }    
    }


    private void Death()
    {
        isDead = true;
        GetComponent<Score>().OnDeath();
    }
}
