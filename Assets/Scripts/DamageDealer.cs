using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{

    [SerializeField] int damage = 100;
    [SerializeField] int damageIncrease = 1;
    
    void Start()
    {
        damage += damageIncrease * FindObjectOfType<GameSession>().GetWaveCount();
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
