using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    GameManager gM;

    public GameObject players;

    public TMP_Text title;
    public TMP_Text tUsing, controls, sPrompt; //control scheme and swtich prompt
    public TMP_Text hInst, gInst;
    public string usingKeys, usingCon; //tells the player which one they're using
    public string hKeys, hCon, gKeys, gCon;

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
        else if (tUsing.alpha < 1)
        {
            yield return new WaitForSeconds(0.0015f);
            tUsing.alpha += 0.005f;
            controls.alpha += 0.005f;
            sPrompt.alpha += 0.005f;
            TitleFadeIn();
        }
        else
        {
            gM.canInput = true;

            yield return new WaitForSeconds(0.1f);
            players.SetActive(true);
            hInst.alpha = 255;
            gInst.alpha = 255;
        }
    }

    public void CSwitch()
    {
        if(conSwitch == false)
        {
            conSwitch = true;
            controls.text = usingCon;
            hInst.text = hCon;
            gInst.text = gCon;
        }
        else if(conSwitch == true)
        {
            conSwitch = false;
            controls.text = usingKeys;
            hInst.text = hKeys;
            gInst.text = gKeys;
        }
    }
}
