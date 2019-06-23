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
    public BoxCollider2D legCollider;
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
        updateLeg();

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

    public void updateLeg()
    {

        legRenderer.SetPosition(1, transform.TransformPoint(Vector3.zero));

        /*
         * 
         * uses solution from Swati Patel: http://www.theappguruz.com/blog/add-collider-to-line-renderer-unity
         * VVVVVVVVVV
         * 
         */

        //finds the size of the collider based on the length of the leg
        legCollider.size = new Vector2(Vector2.Distance(origLegPos, transform.TransformPoint(Vector3.zero)), 1);

        //gets midpoint between the body and the knee and places the collider there
        legCollider.transform.position = (origLegPos + transform.TransformPoint(Vector3.zero)) / 2;

        // Following lines calculate the angle between startPos and endPos (makes a right triangle) (height / width)
        float angle = (Mathf.Abs(origLegPos.y - transform.TransformPoint(Vector3.zero).y) / Mathf.Abs(Mathf.Abs(origLegPos.x - transform.TransformPoint(Vector3.zero).x)));


        //if the vertical start position is less than that of the end position and horizontal starting position is greater than that of the end position and vice versa, the angle is multiplied by -1
        if ((origLegPos.y < transform.TransformPoint(Vector3.zero).y && origLegPos.x > transform.TransformPoint(Vector3.zero).x) || (transform.TransformPoint(Vector3.zero).y < origLegPos.y && transform.TransformPoint(Vector3.zero).x > origLegPos.x))
        {
            angle *= -1;
        }

        //converts angle to radians and then converts that to degrees (atan returns arc-tangent of angle in radians [big boi trigonometry])
        //atan creates an angle based on the length of a side (in this case, the big boi hypotenuse)
        angle = Mathf.Rad2Deg * Mathf.Atan(angle);

        //rotates the collider accordingly
        legCollider.transform.rotation = Quaternion.Euler(0,0,angle);


    }

}
