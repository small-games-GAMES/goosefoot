using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class gooseMovement : MonoBehaviour
{

    public float moveH, moveV;
    public float speed;

    public Rigidbody2D rb2d;

    public int playerId;
    public Player player;

    public bool canHonk;
    public float honkCooldown;
    public GameObject honkCollider;

    public soundManager sm;


    // Start is called before the first frame update
    void Start()
    {

        player = ReInput.players.GetPlayer(playerId);

        //locks cursor in place and makes it invisible
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        honkCollider.SetActive(false);

        canHonk = true;
        
    }

    // Update is called once per frame
    void Update()
    {

        getPlayerInput();

        if (player.GetButtonDown("Honk") && canHonk)
        {

            StartCoroutine(honk());

        }
        
    }

    private void FixedUpdate()
    {

        movePlayer();

    }

    //gets player input for the Goose's body
    void getPlayerInput()
    {

        moveH = player.GetAxisRaw("GXMove");
        moveV = player.GetAxisRaw("GYMove");

    }

    //actually moves the Goose's body
    void movePlayer()
    {

        rb2d.velocity = new Vector2(moveH * speed, moveV * speed);

    }

    //turns on the honk collider for a little bit and then turns it off again
    IEnumerator honk()
    {

        canHonk = false;
        sm.playHonk();

        //turns on a collider for the honk
        honkCollider.SetActive(true);

        //waits for the sound to play
        yield return new WaitForSeconds(sm.honk.length);

        //turns off collider for honk
        honkCollider.SetActive(false);

        //waits a cooldown before the goose is able to honk again
        yield return new WaitForSeconds(honkCooldown);

        canHonk = true;

    }

}
