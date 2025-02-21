using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] AudioSource audioSource; // ตัวเล่นเสียง
    [SerializeField] AudioClip ClickSound;
    public string SceneName;
    public void GotoScene()
    {
        SceneManager.LoadScene(SceneName);
        audioSource.PlayOneShot(ClickSound);
    }
}
