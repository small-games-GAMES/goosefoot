using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class LegMovement : MonoBehaviour
{
    Player player;
    public int playerNum;

    public GameManager gM;

    Rigidbody2D rB;
    public HingeJoint2D hJF, hJB;
    JointMotor2D hingeMotor;

    public Rigidbody2D footRB;

    public float speed;
    Vector2 moveInp;

    bool canKick = true;
    public float kickCooldown;

    public LineRenderer legRenderer;
    Vector3 origLegPos;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerNum);

        rB = GetComponent<Rigidbody2D>();
        //hJ = GetComponent<HingeJoint2D>();

        origLegPos = legRenderer.GetPosition(0);

    }

    // Update is called once per frame
    void Update()
    {
        moveInp = new Vector2(player.GetAxis("HXMove"), player.GetAxis("HYMove"));

        //anchors origin point of the leg line renderer and updates the end point of the upper leg as the knee moves (uses transformpoint because it's a child object)
        //This makes it look like the leg is all one piece
        legRenderer.SetPosition(0, origLegPos);
        legRenderer.SetPosition(1, transform.TransformPoint(Vector3.zero));

        if (player.GetButtonDown("KickForward"))
        {
            Kick(footKickForward());
        }

        if (player.GetButtonDown("KickBackward"))
        {

            Kick(footKickBackward());

        }
    }

    //you always want to put physics stuff in FixedUpdate!
    private void FixedUpdate()
    {

        rB.velocity = moveInp * speed;

    }

    //this would signify if the knee hit the goose the human wins rather than the foot hitting the goose
    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Goose")
        {
            gM.HWin();
        }
    }*/

    void Kick(IEnumerator _insIEnumerator)
    {
        if(canKick == true)
        {
            canKick = false;

            StartCoroutine(_insIEnumerator);
        }
    }

    IEnumerator footKickForward()
    {

        hJF.useMotor = true;

        yield return new WaitForSeconds(kickCooldown);

        hJF.useMotor = false;
        footRB.velocity = new Vector2(0, 0);

        yield return new WaitForSeconds(kickCooldown / 2);

        canKick = true;
    }

    IEnumerator footKickBackward()
    {

        hJB.useMotor = true;

        yield return new WaitForSeconds(kickCooldown);

        hJB.useMotor = false;
        footRB.velocity = new Vector2(0, 0);

        yield return new WaitForSeconds(kickCooldown / 2);

        canKick = true;
    }

}
