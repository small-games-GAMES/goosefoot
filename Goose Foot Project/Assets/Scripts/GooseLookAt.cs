using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseLookAt : MonoBehaviour
{
    Quaternion facing;

    // Start is called before the first frame update
    void Start()
    {
        facing = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = facing;
    }
}
