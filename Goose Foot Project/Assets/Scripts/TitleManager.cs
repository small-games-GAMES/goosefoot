using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    GameManager gM;

    public TMP_Text title;
    public TMP_Text tUsing, controls, sPrompt; //control scheme and swtich prompt
    public string usingKeys, usingCon; //tells the player which one they're using

    bool conSwitch = false;

    // Start is called before the first frame update
    void Start()
    {
        gM = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        TitleFadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TitleFadeIn()
    {
        StartCoroutine(conPrompt());
    }

    IEnumerator conPrompt()
    {
        if (title.alpha < 1)
        {
            title.alpha += 0.005f;
            yield return new WaitForSeconds(0.005f);
            TitleFadeIn();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);

            tUsing.enabled = true;
            controls.enabled = true;
            sPrompt.enabled = true;

            gM.canInput = true;
        }
    }

    public void CSwitch()
    {
        if(conSwitch == false)
        {
            conSwitch = true;
            controls.text = usingCon;
        }
        else if(conSwitch == true)
        {
            conSwitch = false;
            controls.text = usingKeys;
        }
    }
}
