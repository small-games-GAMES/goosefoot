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

    public GameManager gm;
    public LegMovement lm;

    public Transform ankleTransform;

    // Update is called once per frame
    void Update()
    {
        updateCalf();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "gooseHead" && lm.isKicking)
        {
            gm.HWin();
        }
    }

    //updates the calf line renderer based on the position of the knee and foot (uses transformpoint because it's a child object)
    //This also helps the leg look like one piece
    void updateCalf()
    {
        calfRenderer.SetPosition(0, lm.gameObject.transform.TransformPoint(Vector3.zero));
        calfRenderer.SetPosition(1, ankleTransform.TransformPoint(Vector3.zero));
    }
}
