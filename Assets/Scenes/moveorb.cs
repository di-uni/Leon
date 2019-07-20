using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveorb : MonoBehaviour
{
    public KeyCode moveL;
    public KeyCode moveR;
    public KeyCode spacebar;
    public float horizVel = 0;
    public int laneNum = 2;
    public string controlLocked = "n";
    public static float ZVel = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().velocity = new Vector3(horizVel, GM.vertVel, ZVel);

        if ((Input.GetKeyDown(moveL)) && (laneNum > 1) &&(controlLocked == "n"))
        {
            horizVel = -2;
            StartCoroutine(stopSlide());
            laneNum -= 1;
            controlLocked = "y";
        }
        else if ((Input.GetKeyDown(moveR)) && (laneNum < 3) && (controlLocked == "n"))
        {
            horizVel = 2;
            StartCoroutine(stopSlide());
            laneNum += 1;
            controlLocked = "y";
        }

        if (Input.GetKeyDown(spacebar)) {
            ZVel = 0;
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            ZVel = 4;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "lethal")
        {
            Destroy(gameObject);
        }
        if (other.gameObject.name == "Capsule")
        {
            Destroy(other.gameObject);
        }
        if (other.gameObject.name == "holedamge") {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "rampbottomtrig")
        {
            GM.vertVel = 2;
        }
        if (other.gameObject.name == "ramptoptrig")
        {
            GM.vertVel = 0;
        }
    }

    IEnumerator stopSlide()
    {
        yield return new WaitForSeconds(.5f);
        horizVel = 0;
        controlLocked = "n";
    }
}
