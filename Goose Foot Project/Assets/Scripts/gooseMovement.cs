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


    // Start is called before the first frame update
    void Start()
    {

        player = ReInput.players.GetPlayer(playerId);
        Cursor.visible = false;
        
    }

    // Update is called once per frame
    void Update()
    {

        getPlayerInput();
        
    }

    private void FixedUpdate()
    {

        movePlayer();

    }

    void getPlayerInput()
    {

        moveH = player.GetAxisRaw("GXMove");
        moveV = player.GetAxisRaw("GYMove");

    }

    void movePlayer()
    {

        rb2d.velocity = new Vector2(moveH * speed, moveV * speed);

    }

}
