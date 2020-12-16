using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoading : MonoBehaviour
{

    [SerializeField] float deathDelayInSec = 1.5f;
    [SerializeField] float loadDelayInSec = 3f;


    public string GetCurrentScene()
    {
        return SceneManager.GetActiveScene().name;
    }
        
    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadLevelOne()
    {
        SceneManager.LoadScene("Level One");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadBoss()
    {
        SceneManager.LoadScene("Boss");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadArena()
    {
        SceneManager.LoadScene("Arena");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadLevelOneEnd()
    {
        StartCoroutine(LoadLevelOneEndDelay());
    }

    IEnumerator LoadLevelOneEndDelay()
    {
        yield return new WaitForSeconds(loadDelayInSec);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadInstructions()
    {
        SceneManager.LoadScene("Instructions Menu");
    }

    public void LoadGameOverStory ()
    {
        StartCoroutine(DelayGameOverStory());
    }

    IEnumerator DelayGameOverStory()
    {
        yield return new WaitForSeconds(deathDelayInSec);
        SceneManager.LoadScene("Game Over Story");
    }

    public void LoadGameOverBoss()
    {
        StartCoroutine(DelayGameOverBoss());
    }

    IEnumerator DelayGameOverBoss()
    {
        yield return new WaitForSeconds(deathDelayInSec);
        SceneManager.LoadScene("Game Over Boss");
    }

    public void LoadGameOverArena()
    {
        StartCoroutine(DelayGameOverArena());
    }

    IEnumerator DelayGameOverArena()
    {
        yield return new WaitForSeconds(deathDelayInSec);
        SceneManager.LoadScene("Game Over Arena");
    }

    public void LoadVictory()
    {
        StartCoroutine(DelayVictory());
    }

    IEnumerator DelayVictory()
    {
        yield return new WaitForSeconds(deathDelayInSec);
        SceneManager.LoadScene("Story Menu 3");
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
