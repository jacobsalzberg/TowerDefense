using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProType
{
    rock, arrow, fireball
};
public class Projectile : MonoBehaviour {

    //each projectile is going to know its own attack strength
    [SerializeField]
    private int attackStrength;
    //Valores serao colocados no inspector!!
    [SerializeField]
    private ProType projectileType;

    //Getters

    public int AttackStrength
    {
        get {
            return attackStrength;
        }        
    }

    public ProType ProjectileType
    {
        get
        {
            return projectileType;
        }
    }
}
