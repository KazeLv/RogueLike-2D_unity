﻿using UnityEngine;
using System.Collections;

public class Walls : MonoBehaviour {

    public int hp = 2;
    public Sprite damageSprite;
    
    void GetAttacked()
    {
        hp -= 1;
        GetComponent<SpriteRenderer>().sprite = damageSprite;
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    } 

}
