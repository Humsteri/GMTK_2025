using System;
using System.Threading.Tasks;
using UnityEngine;

public class TutorialPrompt : MonoBehaviour
{
    InputManager inputManager => InputManager.Instance;
    [SerializeField] GameObject tutorialPrompt;
    async Task Start()
    {
        await WaitForSecondsAsync(0.5f);
    }
    void Update()
    {
        if (tutorialPrompt.activeInHierarchy && inputManager.TutorialInteraction)
        {
            tutorialPrompt.SetActive(false);
            inputManager.DisableTutorialPrompt();
        }
        if (inputManager.Esc)
        {
            tutorialPrompt.SetActive(!tutorialPrompt.activeInHierarchy);
            if (tutorialPrompt.activeInHierarchy)
                inputManager.EnableTutorialPrompt();
            else
                inputManager.DisableTutorialPrompt();
        }
        
    }

    async Task WaitForSecondsAsync(float delay)
    {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        tutorialPrompt.SetActive(true);
        inputManager.EnableTutorialPrompt();
    }
}
