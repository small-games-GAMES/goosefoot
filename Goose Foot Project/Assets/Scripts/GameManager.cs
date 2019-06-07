using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Rewired;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Player player;
    public int playerNum;

    public soundManager sm;

    public TMP_Text winTitle;
    public string gooseWin, humanWin;

    public bool canReset;
    public GameObject resetText;

    // Start is called before the first frame update
    void Start()
    {
        player = ReInput.players.GetPlayer(playerNum);

        canReset = false;

        resetText.SetActive(false);
        winTitle.text = "";
    }

    private void Update()
    {
        //if any button is pressed and canReset is equal to true, it resets the game
        if (player.GetAnyButton() && canReset)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.anyKey && canReset)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //if the human wins, it plays the win sound effect, changes title card to say that the human won, and then starts the reset coroutine
    public void HWin()
    {
        sm.playWin();

        winTitle.text = humanWin;

        StartCoroutine(waitThenReset());
    }

    //if the goose wins, it plays the win sound effect, changes title card to say the goose won, and then starts the reset coroutine
    public void GWin()
    {
        sm.playWin();

        winTitle.text = gooseWin;

        StartCoroutine(waitThenReset());
    }

    //waits for the length of the win sound effect and then some and then enables reset
    IEnumerator waitThenReset()
    {
        yield return new WaitForSeconds(sm.winSound.length + 1.5f);

        canReset = true;
        resetText.SetActive(true);
    }
}
