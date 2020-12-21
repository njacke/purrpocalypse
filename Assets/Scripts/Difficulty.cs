using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{

    [SerializeField] bool easy = false; //for debug

    private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        int numberDifficulties = FindObjectsOfType<Difficulty>().Length;
        if (numberDifficulties > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetHardDifficulty()
    {
        easy = false;
    }

    public void SetEasyDifficulty()
    {
        easy = true;
    }

    public bool EasyDifficulty()
    {
        return easy;
    }
}
