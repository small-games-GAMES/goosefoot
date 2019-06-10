using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Rewired;

public class GameManager : MonoBehaviour
{
    public static GameManager gM;

    private void Awake()
    {
        if(gM == null)
        {
            gM = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(gM != this)
            {
                Destroy(gameObject);
            }
        }
    }

    Player player;
    public int playerNum;
    bool useCon = false; //are they using a controller

    TitleManager tM;
    TextManager tXM;
    soundManager sm;

    public bool canReset;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerNum);

        canReset = false;
    }

    private void Update()
    {
        if(sm == null)
        {
            sm = GameObject.FindGameObjectWithTag("SoundMan").GetComponent<soundManager>();
        }

        //controls title scene stuff
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(0))
        {
            if (tM == null)
            {
                if (GameObject.FindGameObjectWithTag("TitleMan") != null)
                {
                    tM = GameObject.FindGameObjectWithTag("TitleMan").GetComponent<TitleManager>();
                }
            }

            //switches from controller to keyboard preferred input and back
            if (player.GetButtonDown("ConSwitch"))
            {
                if (useCon == false)
                {
                    useCon = true;
                    tM.CSwitch();
                }
                else if(useCon == true)
                {
                    useCon = false;
                    tM.CSwitch();
                }
            }
        }

        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(1))
        {
            if (tXM == null)
            {
                if (GameObject.FindGameObjectWithTag("TextMan") != null)
                {
                    tXM = GameObject.FindGameObjectWithTag("TextMan").GetComponent<TextManager>();
                }
            }
        }

        //if any button is pressed and canReset is equal to true, it resets the game
        if (player.GetAnyButton() && canReset)
        {
            canReset = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.anyKey && canReset)
        {
            canReset = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (player.GetButtonDown("Restart"))
        {
            SceneManager.LoadScene(0);
        }
    }

    //if the human wins, it plays the win sound effect, changes title card to say that the human won, and then starts the reset coroutine
    public void HWin()
    {
        sm.playWin();
        tXM.HWin();

        StartCoroutine(waitThenReset());
    }

    //if the goose wins, it plays the win sound effect, changes title card to say the goose won, and then starts the reset coroutine
    public void GWin()
    {
        sm.playWin();
        tXM.GWin();

        StartCoroutine(waitThenReset());
    }

    //waits for the length of the win sound effect and then some and then enables reset
    IEnumerator waitThenReset()
    {
        yield return new WaitForSeconds(sm.winSound.length + 2.5f);

        canReset = true;
        tXM.ResPrompt();
    }
}
