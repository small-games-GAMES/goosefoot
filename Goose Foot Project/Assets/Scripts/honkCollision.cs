using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class honkCollision : MonoBehaviour
{
    public GameManager gm;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "body")
        {
            gm.GWin();
        }
    }
}
