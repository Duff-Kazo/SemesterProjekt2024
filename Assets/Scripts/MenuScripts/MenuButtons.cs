using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void StartButton()
    {
        PlayerController.orbsActivated = false;
        PlayerController.eyesActivated = false;
        PlayerController.plagueActivated = false;
        PlayerController.shieldActivated = false;
        ShopButtons.fullAutoBought = false;
        SceneManager.LoadScene("Dungeons");
    }

    public void StartFirstCutscene()
    {
        SceneManager.LoadScene("CutScene");
    }
    public void CreditsButton()
    {
        SceneManager.LoadScene("Credits");
    }
    public void ExitCreditsButton()
    {
        SceneManager.LoadScene("Menu");
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
