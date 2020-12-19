using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    // Start is called before the first frame update

    int score = 0;
    int waveCount = 0;
    
    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }    

    public int GetScore()
    {
        return score;
    }    

    public void AddToScore(int scoreValue)
    {
        score += scoreValue;
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }

    public void AddWaveCount()
    {
        waveCount += 1;
        Debug.Log(waveCount);
    }

    public int GetWaveCount()
    {
        return waveCount;
    }

}
