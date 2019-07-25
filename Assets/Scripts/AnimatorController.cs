using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator anim;
    bool isSit = false;
    private float startTime;
    private float animationDuration = 3.0f;
    void Start()
    {
        anim = GetComponent<Animator>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime < animationDuration) {
            return ;
        }
        
        if (Input.GetMouseButtonDown (0)){
            if (Input.mousePosition.y < Screen.height / 4 && Input.mousePosition.x > Screen.width / 3 && Input.mousePosition.x < 2 * Screen.width / 3 && !isSit)
            {
                anim.SetTrigger("StartSit");
                isSit = true;
                print("animator sit");
            }

            else if (Input.mousePosition.y < Screen.height / 4 && Input.mousePosition.x > Screen.width / 3 && Input.mousePosition.x < 2 * Screen.width / 3 && isSit)
            {
                anim.SetTrigger("StandUp");
                isSit = false;
                print("animator stand up");
            }
        }
    }
}