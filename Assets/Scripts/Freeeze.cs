using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeeze : MonoBehaviour
{
    Vector3 FreezePosition;
    // Start is called before the first frame update
    void Start()
    {
        FreezePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = FreezePosition;
    }
}
