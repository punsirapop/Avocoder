using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField] Sprite[] tutorials;
    [SerializeField] Image panel;
    [SerializeField] GameObject[] changePgBtns;
    [SerializeField] GameObject startBtn;
    [SerializeField] GameObject title;

    static int tIndex = -1;

    private void Update()
    {
        if (tIndex == 0)
        {
            changePgBtns[0].SetActive(false);
            changePgBtns[1].SetActive(true);
        }
        else if (tIndex == tutorials.Length - 1)
        {
            changePgBtns[0].SetActive(true);
            changePgBtns[1].SetActive(false);
            changePgBtns[2].SetActive(true);
        }
        else if (tIndex < 0)
        {
            changePgBtns[0].SetActive(false);
            changePgBtns[1].SetActive(false);
        }
        else
        {
            changePgBtns[0].SetActive(true);
            changePgBtns[1].SetActive(true);
            changePgBtns[2].SetActive(false);
        }
    }

    public void StartTutorial()
    {
        startBtn.SetActive(false);
        title.SetActive(false);
        tIndex = 0;
        panel.color = Color.white;
        panel.sprite = tutorials[tIndex];
    }

    public void ChangePg(int i)
    {
        tIndex += i;
        panel.sprite = tutorials[tIndex];
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
