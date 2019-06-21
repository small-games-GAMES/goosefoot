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
    public bool useCon = false; //are they using a controller
    public bool canInput = true; //gm taking inputs

    public int titleScene, gameScene; //title and game scenes by build number

    soundManager sm;
    TitleManager tM;
    TextManager tXM;

    public bool switching = false; //tells the gm that it is loading a new scene

    //title screen stuff
    bool started = false; //title screen text sequence has begun
    bool hReady = false; //human ready to go
    bool gReady = false; //goose ready to go

    bool end = false; //game has ended
    public bool canReset; //can reset game to play another round

    //NOTE: doesnt work
    string winner;
   private GameObject humanHat;
   private GameObject gooseHat;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerNum);

        canReset = false;
    }

    private void Update()
    {
        if (canInput == false) //prevents input into the gm until we want them
        {
            playerNum = 2;
        }
        else
        {
            playerNum = 0;
        }

        if(sm == null)
        {
            sm = GameObject.FindGameObjectWithTag("SoundMan").GetComponent<soundManager>();
        }

        //controls title scene stuff
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(titleScene))
        {
            //on title screen, starting
            if (started == false)
            {
                started = true;
            }

            //checking to see if players are ready to continue to the game
            if (player.GetButtonDown("KickForward") || player.GetButtonDown("KickBackward"))
            {
                hReady = true;
                //SM.READYSOUND
                tM.hRInd(); //sets active ready marker
            }
            if (player.GetButtonDown("Honk"))
            {
                gReady = true;
                //SM.READYSOUND
                tM.gRInd(); //sets active ready marker
            }

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

            //if players are ready, run coroutine to load game scene (loadGS)
            if(hReady && gReady == true)
            {
                if(switching == false)
                {
                    switching = true;
                    StartCoroutine(loadGS());
                }
            }
        }

        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(gameScene))
        {
            if (tXM == null)
            {
                if (GameObject.FindGameObjectWithTag("TextMan") != null)
                {
                    tXM = GameObject.FindGameObjectWithTag("TextMan").GetComponent<TextManager>();
                }
            }

            if(tXM.started == true)
            {
                canInput = true;
            }

            //if any button is pressed and canReset is equal to true, it resets the game
            if (player.GetAnyButton() && canReset)
            {
                canReset = false;
                canInput = false;
                end = false;
                SceneManager.LoadScene(gameScene);
                //check who won
                StartCoroutine(hat());
            }
            if (Input.anyKey && canReset)
            {
                canReset = false;
                canInput = false;
                end = false;
                SceneManager.LoadScene(gameScene);
                StartCoroutine(hat());
            }
        }

        if (player.GetButtonDown("Restart"))
        {
            end = false;
            started = false;
            SceneManager.LoadScene(titleScene);
        }
    }

    IEnumerator loadGS()
    {
        yield return new WaitForSeconds(2);

        canInput = false;
        SceneManager.LoadScene(gameScene);
    }

    //if the human wins, it plays the win sound effect, changes title card to say that the human won, and then starts the reset coroutine
    public void HWin()
    {
        if(end == false)
        {
            end = true;
            sm.playWin();
            tXM.HWin();
            //saves win
            winner = "human";
            
            StartCoroutine(waitThenReset());
        }
    }

    //if the goose wins, it plays the win sound effect, changes title card to say the goose won, and then starts the reset coroutine
    public void GWin()
    {
        if (end == false)
        {
            end = true;
            sm.playWin();
            tXM.GWin();
            //saves win
            winner = "goose";

            StartCoroutine(waitThenReset());
        }
    }

    //waits for the length of the win sound effect and then some and then enables reset
    IEnumerator waitThenReset()
    {
        yield return new WaitForSeconds(sm.winSound.length + 2.5f);

        canReset = true;
        tXM.ResPrompt();
    }

    IEnumerator hat()
    {
        yield return new WaitForSeconds(0.5f);

        gooseHat = GameObject.FindGameObjectWithTag("GHat");
        humanHat = GameObject.FindGameObjectWithTag("HHat");

        if (winner == "goose")
        {
            gooseHat.SetActive(true);
            humanHat.SetActive(false);
        }
        else if (winner == "human")
        {
            gooseHat.SetActive(false);
            humanHat.SetActive(true);
        }
    }
}
