using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuButtons : MonoBehaviour
{
    GameObject myEventSystem;
    AudioReverbFilter reverb;
    [SerializeField] AudioSource ambience;
    private void OnEnable()
    {
        Time.timeScale = 0f;
        Interactable.gamePaused = true;
    }

    private void Start()
    {
        myEventSystem = GameObject.Find("EventSystem");
        reverb = FindObjectOfType<AudioReverbFilter>();
        StartButton();
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

    public void PauseGame()
    {
        ambience.enabled = false;
        reverb.enabled = false;
        Time.timeScale = 0f;
        Interactable.gamePaused = true;
    }
}
