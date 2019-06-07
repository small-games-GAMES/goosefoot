using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    public TMP_Text controls;
    public string usingKeys, usingCon;

    bool conSwitch = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
