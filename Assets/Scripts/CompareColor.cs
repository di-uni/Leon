using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompareColor : MonoBehaviour
{
    GameObject gameManager;

    TileManager tileManager= new TileManager();
    Material material;
    // Start is called before the first frame update
    void Start()
    {
        material=gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("space")){
            Debug.Log(material.color+"this is color");
        }
    }
}
