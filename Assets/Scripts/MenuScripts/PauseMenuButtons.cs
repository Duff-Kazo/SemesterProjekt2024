using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuButtons : MonoBehaviour
{
    GameObject myEventSystem;
    private void OnEnable()
    {
        Time.timeScale = 0f;
        Interactable.gamePaused = true;
    }

    private void Start()
    {
        myEventSystem = GameObject.Find("EventSystem");
    }

    private void Update()
    {
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
    }

    public void StartButton()
    {
        transform.gameObject.SetActive(false);
        Time.timeScale = 1f;
        Interactable.gamePaused = false;
    }
    public void CreditsButton()
    {
        SceneManager.LoadScene("Credits");
        Time.timeScale = 1f;
        Interactable.gamePaused = false;
    }
    public void ExitCreditsButton()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
        Interactable.gamePaused = false;
    }
    public void ExitButton()
    {
        Application.Quit();
        Time.timeScale = 1f;
        Interactable.gamePaused = false;
    }
}
