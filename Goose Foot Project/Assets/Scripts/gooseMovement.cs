using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class gooseMovement : MonoBehaviour
{
    public int playerId;
    public Player player;

    GameManager gM;
    public soundManager sm;

    public Rigidbody2D rb2d;

    public float moveH, moveV;
    public float speed;
    public float conSpeed;

    public SpriteRenderer honkSR;
    public BoxCollider2D honkCollider;
    public bool canHonk;
    public float honkCooldown;

    bool conSwitch = false;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);

        gM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        //locks cursor in place and makes it invisible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        honkCollider.enabled = false;

        if (conSwitch == false)
        {
            if (gM.useCon == true)
            {
                conSwitch = true;
                speed = conSpeed;
            }
        }

        canHonk = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gM.canInput == true)
        {
            //gets player input for the Goose's body
            moveH = player.GetAxisRaw("GXMove");
            moveV = player.GetAxisRaw("GYMove");

            if (player.GetButtonDown("Honk") && canHonk)
            {
                StartCoroutine(honk());
            }
        }
    }

    private void FixedUpdate()
    {
        //actually moves the Goose's body
        rb2d.velocity = new Vector2(moveH * speed, moveV * speed);
    }

    //turns on the honk collider for a little bit and then turns it off again
    IEnumerator honk()
    {
        canHonk = false;
        sm.playHonk();

        //turns on a collider for the honk
        honkSR.enabled = true;
        honkCollider.enabled = true;

        //waits for the sound to play
        yield return new WaitForSeconds(sm.honk.length);

        //turns off collider for honk
        honkCollider.enabled = false;
        honkSR.enabled = false;

        //waits a cooldown before the goose is able to honk again
        yield return new WaitForSeconds(honkCooldown);

        canHonk = true;
    }
}
