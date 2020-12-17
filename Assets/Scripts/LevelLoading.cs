using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoading : MonoBehaviour
{

    [SerializeField] float deathDelayInSec = 1.5f;
    [SerializeField] float loadDelayInSec = 3f;
    [SerializeField] float barkDelayInSec = 2f;
    [SerializeField] AudioClip barkSound;
    [SerializeField] [Range (0, 1)] float barkSoundVolume;


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

    public void LoadNextSceneBark()
    {
        StartCoroutine(LoadNextSceneBarkDelay());
    }

    IEnumerator LoadNextSceneBarkDelay()
    {
        AudioSource.PlayClipAtPoint(barkSound, Camera.main.transform.position, barkSoundVolume);
        yield return new WaitForSeconds(barkDelayInSec);
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
        SceneManager.LoadScene("Story Menu 1");
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
