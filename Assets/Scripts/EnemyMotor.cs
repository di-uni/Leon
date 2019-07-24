using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMotor : MonoBehaviour
{
private CharacterController controller;
    private Vector3 moveVector;
    private int count = 0;
    private float speed =10.0f;
    private float verticalVelocity = 0.0f;
    private float gravity = 12.0f;
    private float animationDuration = 4.0f;
    private float startTime;
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
        if (Time.time - startTime < animationDuration) {
            moveVector.z =0;
            return;
        }
        // dead when fall
        // if(GameObject.FindGameObjectWithTag("Enemy").transform.position.y<-20) {
        //     Destroy(gameObject);
        // }
        // gravity
        if(controller.isGrounded) {
            verticalVelocity = -0.5f;
        }
        else {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        // change for touchscreen
        float playerz =GameObject.FindGameObjectWithTag("Player").transform.position.z;
        float enemy1z = GameObject.Find("Enemy").transform.position.z;
        float enemy2z = GameObject.Find("Enemy2").transform.position.z;
        float enemy3z = GameObject.Find("Enemy3").transform.position.z;
        
        if(enemy1z-playerz>Random.Range(64,128)){
            //music start
            FindObjectOfType<AudioManager>().Play("enemy");
            GameObject.Find("Enemy").transform.position=new Vector3(0,1,playerz-64);
            GameObject.Find("Enemy2").transform.position=new Vector3(1,1,playerz-64);
            GameObject.Find("Enemy3").transform.position=new Vector3(-1,1,playerz-64);
        }
        moveVector.z = speed;
        controller.Move(moveVector * Time.deltaTime);
        
    }
    public void SetSpeed(float modifier)
    {
        speed = 10.0f +modifier;
    }
    
    //충돌, 숨을떄 쓴다
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {     
        if(hit.gameObject.tag == "Player") {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Score>().OnDeath();
        }
    } 
}