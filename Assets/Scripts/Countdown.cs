using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Countdown : MonoBehaviour
{
    [SerializeField] float timeRemaining = 30;
    Text countdownText;
    bool countdownFinished = false;


    // Start is called before the first frame update
    void Start()
    {
        countdownText = GetComponent<Text>();
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }

        if (timeRemaining < 0)
        {
            timeRemaining = 0;
            countdownFinished = true;
        }

        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);

        countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        
        if (countdownFinished == true)
        {
            FindObjectOfType<LevelLoading>().LoadLevelOneEnd();
        }
    }

    public bool CountdownFinished()
    {
        return countdownFinished;
    }
}
