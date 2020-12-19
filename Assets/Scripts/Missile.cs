using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missile : MonoBehaviour
{
    bool failCalled = false;
    [SerializeField] float failDelay = 2f;
    Text missile;

    // Start is called before the first frame update
    void Start()
    {
        missile = GetComponent<Text>();
        missile.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<Countdown>().CountdownFinished() == true)
        {
            if (failCalled == false)
            {
                StartCoroutine(CriticalFailure());
                failCalled = true;
            }
        }
    }

    IEnumerator CriticalFailure()
    {
        missile.text = "MISSILE LAUNCHED";
        yield return new WaitForSeconds(failDelay);
        missile.text = "CRITICAL FAILURE";
        missile.color = Color.red;
    }

}
