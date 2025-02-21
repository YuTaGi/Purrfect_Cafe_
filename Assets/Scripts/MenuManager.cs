using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource; // ���������§
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

    public GameObject objectToHide; // �ѵ�ط���ͧ��ë�͹
    public GameObject objectToShow; // �ѵ�ط���ͧ����ʴ�

    public void ToggleObjects()
    {
        if (objectToHide != null)
        {
            objectToHide.SetActive(false); // ��͹�ѵ���á
            audioSource.PlayOneShot(ClickSound);


        }

        if (objectToShow != null)
        {
            objectToShow.SetActive(true); // �ʴ��ѵ�ط���ͧ
            audioSource.PlayOneShot(ClickSound);
        }
    }
}

