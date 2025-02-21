using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource; // ตัวเล่นเสียง
    [SerializeField] AudioClip ClickSound;
    public void StartGame()
    {
        if (gameObject.CompareTag("Play"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

        public void QuitGame()
    {
        if (gameObject.CompareTag("Exit"))
        {
            Application.Quit();
        }
    }
        public void BackToMenu()
    {
        if (gameObject.CompareTag("BackToMenu"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    public GameObject objectToHide; // วัตถุที่ต้องการซ่อน
    public GameObject objectToShow; // วัตถุที่ต้องการแสดง

    public void ToggleObjects()
    {
        if (objectToHide != null)
        {
            objectToHide.SetActive(false); // ซ่อนวัตถุแรก
            audioSource.PlayOneShot(ClickSound);


        }

        if (objectToShow != null)
        {
            objectToShow.SetActive(true); // แสดงวัตถุที่สอง
            audioSource.PlayOneShot(ClickSound);
        }
    }
}

