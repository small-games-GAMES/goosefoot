using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class FootMovement : MonoBehaviour
{
    Player player;
    public int playerNum;

    public GameManager gM;

    Rigidbody2D rB;
    public HingeJoint2D hJ;

    float speed;

    bool canKick = true;
    public float kickCooldown;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerNum);

        rB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInp = new Vector2(player.GetAxis("HXMove"), player.GetAxis("HYMove"));
        rB.velocity = moveInp * speed;

        if (player.GetButtonDown("Kick"))
        {
            Kick();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Goose")
        {
            gM.HWin();
        }
    }

    void Kick()
    {
        if(canKick == true)
        {
            canKick = false;

            StartCoroutine(footKick());
        }
    }

    IEnumerator footKick()
    {
        hJ.useMotor = true;

        yield return new WaitForSeconds(kickCooldown);

        canKick = true;
    }
}
