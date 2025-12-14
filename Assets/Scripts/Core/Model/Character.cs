using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Model
{
    [Serializable]
    public class Character
    {
        public string nickname;
        public string color;
        public int score;
        public int currentHealth;
        public int maxHealth;
        public bool isDead;

        public Character() { }

        public Character(string nickname, string color, int maxHealth)
        {
            this.nickname = nickname;
            this.color = color;
            this.currentHealth = maxHealth;
            this.isDead = false;
        }

        public void decreaseHealth(int amount)
        {
            currentHealth = currentHealth - amount;
            if (currentHealth < 0)
            {
                isDead = true;
            }
        }
    }

}
