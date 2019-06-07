using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextManager : MonoBehaviour
{
    public TMP_Text winTitle;
    public string gooseWin, humanWin;

    public GameObject resetText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void HWin()
    {
        winTitle.text = humanWin;
        winTitle.enabled = true;
    }

    public void GWin()
    {
        winTitle.text = gooseWin;
        winTitle.enabled = true;
    }

    public void ResPrompt()
    {
        resetText.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
