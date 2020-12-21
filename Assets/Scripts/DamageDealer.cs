using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{

    [SerializeField] int damage = 100;
    [SerializeField] int damageIncrease = 1;
    [SerializeField] int easyDamage = 100;


    void Start()
    {
        damage += damageIncrease * FindObjectOfType<GameSession>().GetWaveCount();
        if (FindObjectOfType<Difficulty>().EasyDifficulty() == true)
        {
            damage = easyDamage;
        }
    }

    public int GetDamage()
    {
        return damage;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }
}
