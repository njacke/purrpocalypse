using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missile : MonoBehaviour
{
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
            missile.text = "MISSILE LAUNCHED";
        }
    }
}
