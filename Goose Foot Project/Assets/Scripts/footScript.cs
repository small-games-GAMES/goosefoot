using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footScript : MonoBehaviour
{
    /* Notes for later
     * make sure the calf is its own object, or else there won't be any collision on it (make life easier on yourself)
     * to add a collider to the linerenderer: https://answers.unity.com/questions/768997/how-can-i-add-a-collider-to-my-line-renderer-scrip.html  use the first answer
     */

    public LineRenderer calfRenderer;

    GameManager gM;
    public LegMovement lm;

    public Transform ankleTransform;

    public BoxCollider2D calfCol;

    private void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        updateCalf();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "gooseHead" && lm.isKicking)
        {
            gM.HWin();
        }
    }

    //updates the calf line renderer based on the position of the knee and foot (uses transformpoint because it's a child object)
    //This also helps the leg look like one piece
    void updateCalf()
    {
        calfRenderer.SetPosition(0, lm.gameObject.transform.TransformPoint(Vector3.zero));
        calfRenderer.SetPosition(1, ankleTransform.TransformPoint(Vector3.zero));

        /*
         * 
         * uses solution from Swati Patel: http://www.theappguruz.com/blog/add-collider-to-line-renderer-unity
         * VVVVVVVVVV
         * 
         */

        //finds the size of the collider based on the length of the calf
        calfCol.size = new Vector2(Vector2.Distance(lm.gameObject.transform.TransformPoint(Vector3.zero), ankleTransform.TransformPoint(Vector3.zero)), 1);

        //gets midpoint between the knee and the ankle and places the collider there
        calfCol.transform.position = (lm.gameObject.transform.TransformPoint(Vector3.zero) + ankleTransform.TransformPoint(Vector3.zero)) / 2;

        // Following lines calculate the angle between startPos and endPos
        float angle = (Mathf.Abs(lm.gameObject.transform.TransformPoint(Vector3.zero).y - ankleTransform.TransformPoint(Vector3.zero).y) / Mathf.Abs(Mathf.Abs(lm.gameObject.transform.TransformPoint(Vector3.zero).x - ankleTransform.TransformPoint(Vector3.zero).x)));


        //if the vertical start position is less than that of the end position and horizontal starting position is greater than that of the end position and vice versa, the angle is multiplied by -1
        if ((lm.gameObject.transform.TransformPoint(Vector3.zero).y < ankleTransform.TransformPoint(Vector3.zero).y && lm.gameObject.transform.TransformPoint(Vector3.zero).x > ankleTransform.TransformPoint(Vector3.zero).x) || (ankleTransform.TransformPoint(Vector3.zero).y < lm.gameObject.transform.TransformPoint(Vector3.zero).y && ankleTransform.TransformPoint(Vector3.zero).x > lm.gameObject.transform.TransformPoint(Vector3.zero).x))
        {
            angle *= -1;
        }

        //converts the angle to radians and then converts that to degrees (atan returns arc-tangent of angle in radians [big boi trigonometry])
        angle = Mathf.Rad2Deg * Mathf.Atan(angle);

        //rotates the collider accordingly
        calfCol.transform.Rotate(0, 0, angle);

    }
}
