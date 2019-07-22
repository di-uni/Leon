using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 moveVector;
    private int count = 0;
    private float speed =8.0f;
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
        moveVector.z= speed;
    }

    // Update is called once per frame
    void Update()
    {

        if (isDead)
            return;
        if (Time.time - startTime < animationDuration) {
            controller.Move((Vector3.forward)*speed*Time.deltaTime);
            return;
        }
        if(GameObject.FindGameObjectWithTag("Player").transform.position.y<-30) {
            Death();
        }

        if(controller.isGrounded) {
            verticalVelocity = -0.5f;
        }
        else {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        // change for touchscreen
        if(moveVector.z != 0)
        {
            moveVector.x = Input.GetAxisRaw("Horizontal")*speed;
            moveVector.y = verticalVelocity;
        }
        if(Input.GetKeyDown(KeyCode.X) && moveVector.z != 0)
        {        
            moveVector = Vector3.zero;
        }
        else if(Input.GetKeyDown(KeyCode.X) && moveVector.z == 0)
        {
            moveVector.z= speed;
        }
        
        
        controller.Move(moveVector * Time.deltaTime);

        
    }
    public void SetSpeed(float modifier)
    {
        speed = 5.0f +modifier;
    }
    
    //충돌, 숨을떄 쓴다
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "Enemy")
            Death();
        
        if(hit.gameObject.tag=="Empty"){
            //Debug.Log("sdfsjlfjs");
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