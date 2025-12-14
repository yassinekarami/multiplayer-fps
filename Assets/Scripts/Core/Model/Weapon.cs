using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Model
{
    public class Weapon
    {
        public int id;
        public string name;
        public int maxAmoAmt;
        public int currentAmo;
        public int damage;
        public bool isActive;

        public Weapon() { }
        public Weapon(int id, string name, int maxAmoAmt, bool isActive, int damage)
        {
            this.id = id;
            this.name = name;
            this.maxAmoAmt = maxAmoAmt;
            this.currentAmo = maxAmoAmt;
            this.isActive = isActive;
            this.damage = damage;
        }

        public void decreaseAmo()
        {
            currentAmo--;
        }

        public void increaseAmo(int amount)
        {
            if (currentAmo + amount > maxAmoAmt)
            {
                currentAmo = maxAmoAmt;
            }
            else
            {
                currentAmo = currentAmo + amount;
            }
        }
        public void increateAmo()
        {
            currentAmo = maxAmoAmt;
        }
    }

}
