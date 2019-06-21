using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class LegMovement : MonoBehaviour
{
    Player player;
    public int playerNum;

    GameManager gM;
    public soundManager sm;

    Rigidbody2D rB;
    public HingeJoint2D hJF, hJB;
    JointMotor2D hingeMotor;
    public float maxAngVel;

    public Rigidbody2D footRB;

    public float speed;
    public float conSpeed; //speed when using a controller
    public float kSpeedMod; //the amount that the speed is multiplied by when kicking
    Vector2 moveInp;

    bool canKick = true;
    public bool isKicking = false;
    public float kickCooldown;

    public LineRenderer legRenderer; //line renderer stuff so the leg looks like it's one solid piece
    Vector3 origLegPos;

    bool conSwitch = false;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerNum);

        gM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        rB = GetComponent<Rigidbody2D>();

        origLegPos = legRenderer.GetPosition(0);


        if (conSwitch == false)
        {
            if (gM.useCon == true)
            {
                conSwitch = true;
                speed = conSpeed;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //anchors origin point of the leg line renderer and updates the end point of the upper leg as the knee moves (uses transformpoint because it's a child object)
        //This makes it look like the leg is all one piece
        legRenderer.SetPosition(0, origLegPos);
        legRenderer.SetPosition(1, transform.TransformPoint(Vector3.zero));

        if (gM.canInput == true)
        {
            moveInp = new Vector2(player.GetAxis("HXMove"), player.GetAxis("HYMove"));

            if (player.GetButtonDown("KickForward"))
            {
                Kick(footKickForward());
            }

            if (player.GetButtonDown("KickBackward"))
            {
                Kick(footKickBackward());
            }
        }

    }

    private void FixedUpdate()
    {
        //controls the movement of the leg
        if(isKicking == true)
        {
            rB.velocity = moveInp * speed * kSpeedMod;
        }
        else
        {
            rB.velocity = moveInp * speed;
        }

        clampFootVelocity();
    }

    void Kick(IEnumerator _insIEnumerator)
    {
        if(canKick == true)
        {
            canKick = false;

            StartCoroutine(_insIEnumerator);
        }
    }

    //turns on the motor for the joint that moves the leg forward and then turns it off after a bit
    IEnumerator footKickForward()
    {
        //SM.KICK SOUND
        hJF.useMotor = true;
        isKicking = true;

        yield return new WaitForSeconds(kickCooldown);

        hJF.useMotor = false;
        isKicking = false;
        footRB.velocity = new Vector2(0, 0);

        yield return new WaitForSeconds(kickCooldown / 2);

        canKick = true;
    }

    //turns on the motor for the joint that moves the leg backward and then turns it off after a bit
    IEnumerator footKickBackward()
    {
        //SM.KICK SOUND
        hJB.useMotor = true;
        isKicking = true;

        yield return new WaitForSeconds(kickCooldown);

        hJB.useMotor = false;
        isKicking = false;
        footRB.velocity = new Vector2(0, 0);

        yield return new WaitForSeconds(kickCooldown / 2);

        canKick = true;
    }

    //clamps the angular velocity of the foot
    void clampFootVelocity()
    {
        //if the foot's angular velocity is greater than the max angular velocity and it isn't kicking, it clamps to the positive max angular velocity
        if (footRB.angularVelocity > maxAngVel && isKicking == false)
        {
            footRB.angularVelocity = maxAngVel;
        }

        //if the foot's angular velocity is less than the negative max angular velocity and isn't kicking, it clamps to the negative max angular velocity
        else if (footRB.angularVelocity < -maxAngVel && isKicking == false)
        {
            footRB.angularVelocity = -maxAngVel;
        }

        //otherwise, it's just equal to its normal angular velocity
        else
        {
            footRB.angularVelocity = footRB.angularVelocity;
        }
    }
}
