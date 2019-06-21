using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class honkCollision : MonoBehaviour
{
    GameManager gM;

    private void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "body")
        {
            gM.GWin();
        }
    }
}
