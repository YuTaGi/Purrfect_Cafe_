using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    [SerializeField] AudioSource audioSource; // ตัวเล่นเสียง
    [SerializeField] AudioClip ClickSound;
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
        audioSource.PlayOneShot(ClickSound);
    }
}
