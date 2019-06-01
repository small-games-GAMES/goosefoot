using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{

    public AudioSource sauce;
    public AudioClip honk, kickSound, hitHeadSound;

    public void playHonk()
    {

        sauce.PlayOneShot(honk, Random.Range(0.9f, 1.2f));

    }

}
