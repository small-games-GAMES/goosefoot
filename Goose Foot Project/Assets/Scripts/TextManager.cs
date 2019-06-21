using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextManager : MonoBehaviour
{
    public TMP_Text startText;
    public string ready, set, go;
    public bool started = false;

    public TMP_Text winTitle;
    public string gooseWin, humanWin;

    public GameObject resetText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(startRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator startRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        startText.text = ready;
        startText.enabled = true;
        yield return new WaitForSeconds(2);

        startText.text = set;
        yield return new WaitForSeconds(2);

        startText.text = go;
        yield return new WaitForSeconds(1);
        started = true;
        yield return new WaitForSeconds(1);
        startText.enabled = false;
    }

    public void HWin()
    {
        winTitle.text = humanWin;
        winTitle.enabled = true;
        print("getim");
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
}
