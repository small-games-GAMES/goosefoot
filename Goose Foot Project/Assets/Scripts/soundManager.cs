using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{

    public AudioSource sauce;
    public AudioClip honk, kickSound, hitHeadSound, winSound;

    public void playHonk()
    {

        sauce.PlayOneShot(honk, Random.Range(0.9f, 1.2f));

    }

    public void playKick()
    {

        sauce.PlayOneShot(kickSound, Random.Range(0.9f, 1.2f));

    }

    public void playHit()
    {

        sauce.PlayOneShot(hitHeadSound);

    }

    public void playWin()
    {

        sauce.PlayOneShot(winSound);

    }

}
