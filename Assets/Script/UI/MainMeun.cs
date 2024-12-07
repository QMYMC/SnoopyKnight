 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMeun : MonoBehaviour
{
    Button newGameBtn;
    Button continueBtn;
    Button quitBtn;

    void Awake()
    {
        newGameBtn =transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        quitBtn = transform.GetChild(3).GetComponent<Button>();

        newGameBtn.onClick.AddListener(SwtichScene);
        quitBtn.onClick.AddListener(QuitGame);
    }

    private void NewGame()
    {
        
    }

    void SwtichScene()
    {
        SceneManager.LoadScene("SimpleNaturePack_Demo");
    }

    void QuitGame()
    {
        Application.Quit();
        Debug.Log("ÍË³öÓÎÏ·");
    }
}
