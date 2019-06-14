using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class TitleFoot : MonoBehaviour
{
    Player player;
    public int playerNum;

    public soundManager sm;

    public GameObject leg;
    public GameObject honk;

    public HingeJoint2D hJF, hJB;
    JointMotor2D hingeMotor;
    public float maxAngVel;

    Rigidbody2D footRB;

    bool isHonking = false;

    bool canKick = true;
    public bool isKicking = false;
    public float kickCooldown;

    public LineRenderer legRenderer; //line renderer stuff so the leg looks like it's one solid piece
    public LineRenderer calfRenderer;
    public Transform ankleTransform;
    Vector3 origLegPos;

    private void Start()
    {
        player = ReInput.players.GetPlayer(playerNum);

        footRB = GetComponent<Rigidbody2D>();

        origLegPos = legRenderer.GetPosition(0);
    }

    // Update is called once per frame
    void Update()
    {
        updateCalf();
        legRenderer.SetPosition(0, origLegPos);
        legRenderer.SetPosition(1, leg.transform.TransformPoint(Vector3.zero));

        if (player.GetButtonDown("KickForward"))
        {
            print("kick input");
            Kick(footKickForward());
        }

        if (player.GetButtonDown("KickBackward"))
        {
            print("kick input");
            Kick(footKickBackward());
        }

        if (player.GetButtonDown("Honk"))
        {
            if (isHonking == false)
            {
                isHonking = true;
                StartCoroutine(honkTime());
            }
        }
    }

    void updateCalf()
    {
        calfRenderer.SetPosition(0, leg.gameObject.transform.TransformPoint(Vector3.zero));
        calfRenderer.SetPosition(1, ankleTransform.TransformPoint(Vector3.zero));

        clampFootVelocity();
    }

    void Kick(IEnumerator _insIEnumerator)
    {
        if (canKick == true)
        {
            canKick = false;

            StartCoroutine(_insIEnumerator);
            print("kick called");
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
        print("kicked");
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
        print("kicked");
    }

    IEnumerator honkTime()
    {
        //SM.HONK
        honk.SetActive(true);
        yield return new WaitForSeconds(sm.honk.length);
        honk.SetActive(false);

        isHonking = false;
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
