using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseLookAt : MonoBehaviour
{
    Quaternion facing; //makes sure the honk is always pointing towards the human

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
