using UnityEngine;
using UnityEngine.UI;

public class KeyButton : MonoBehaviour
{

    LevelLoading levelLoading;

    private void Start()
    {
        levelLoading = FindObjectOfType<LevelLoading>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ReloadLevel();
        }
    }

    private void ReloadLevel()
    {
        string currentScene = levelLoading.GetCurrentScene();
        if (currentScene == "Game Over Story")
        {
            levelLoading.LoadLevelOne();
        }
        if (currentScene == "Game Over Boss")
        {
            levelLoading.LoadBoss();
        }
        if (currentScene == "Game Over Arena")
        {
            levelLoading.LoadArena();
        }
    }
}