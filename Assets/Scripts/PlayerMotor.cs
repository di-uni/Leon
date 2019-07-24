using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 moveVector;
    private int count = 0;
    private float speed =5.0f;
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
        // dead when fall
        if(GameObject.FindGameObjectWithTag("Player").transform.position.y<-20) {
            Death();
        }
        // gravity
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
            if (Input.GetMouseButton (0)){
                if ((Input.mousePosition.x > Screen.width / 2 && Input.mousePosition.y > Screen.height /4 && Input.mousePosition.y < 3* Screen.height /4) || (Input.mousePosition.x > 2* Screen.width / 3 && Input.mousePosition.y < Screen.height /4)){
                    FindObjectOfType<AudioManager>().Play("move");
                    moveVector.x = speed;
                }
                else if ((Input.mousePosition.x < Screen.width / 2 && Input.mousePosition.y > Screen.height / 4 && Input.mousePosition.y < 3* Screen.height /4) || (Input.mousePosition.x < Screen.width / 3 && Input.mousePosition.y < Screen.height /4))
                {
                    FindObjectOfType<AudioManager>().Play("move");
                    moveVector.x = -speed;
                }
            }
        }

        if (Input.GetMouseButtonDown (0)){
            if(Input.mousePosition.y < Screen.height / 4 && Input.mousePosition.x > Screen.width / 3 && Input.mousePosition.x < 2 * Screen.width / 3 && moveVector.z != 0)
            {        
                FindObjectOfType<AudioManager>().Play("stop");
                moveVector = Vector3.zero;
            }
            else if(Input.mousePosition.y < Screen.height / 4 && Input.mousePosition.x > Screen.width / 3 && Input.mousePosition.x < 2 * Screen.width / 3 && moveVector.z == 0)
            {
                moveVector.z= speed;
            }
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
        if(hit.collider.gameObject.tag == "Spaceman" || hit.gameObject.tag == "Spaceman"){
            FindObjectOfType<AudioManager>().Play("crash");
            Death();
            Debug.Log("hit");
        }
        
        if(hit.gameObject.tag=="Empty"){
            //Debug.Log("sdfsjlfjs");
                Color blockcolor = hit.gameObject.GetComponentInParent<MeshRenderer>().material.color;
                //Debug.Log(blockcolor);
                ColorPicker colorPicker = new ColorPicker();
                colorPicker.CompareColor(blockcolor);
            }        
    }


    public void Death()
    {
        isDead = true;
        FindObjectOfType<AudioManager>().Stop("bgm");
        FindObjectOfType<AudioManager>().Play("gameover");
        GetComponent<Score>().OnDeath();
    }

    
}