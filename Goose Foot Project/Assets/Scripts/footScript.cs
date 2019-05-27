using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footScript : MonoBehaviour
{

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
        if (collision.gameObject.tag == "Goose")
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
