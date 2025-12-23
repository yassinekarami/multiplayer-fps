using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Interface.WeaponUI;
using Core.Utils;
using UnityEngine.UI;
using Manager;

namespace Game.Shared.UI.WeaponUI
{
    public class WeaponUI : MonoBehaviour, IWeaponUIObserver
    {
        public List<Sprite> weaponIcons = new List<Sprite>();

        Text ammoAmount;
        Image weaponIcon;

        // Start is called before the first frame update
        void Start()
        {
          
            GameManager.weaponUIObservers.Add(this);
            ammoAmount = GetComponentInChildren<Text>();
            weaponIcon = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnNotify(Constant.WeaponNotificationType type, int currentAmmo, int index)
        {   Debug.Log($"WeaponUI OnNotify called with type: {type} and currentAmmo: {currentAmmo}");

            if (type.Equals(Constant.WeaponNotificationType.WEAPON_CHANGE))
            {
                Debug.Log($"WeaponUI received ammo change notification. Current Ammo: {currentAmmo}");
                weaponIcon.sprite = weaponIcons[index];
                ammoAmount.text = currentAmmo.ToString();
            } else  if (type.Equals(Constant.WeaponNotificationType.WEAPON_AMMO_UPDATE))
            {
                Debug.Log($"WeaponUI received ammo decrease notification. Current Ammo: {currentAmmo}");
                ammoAmount.text = currentAmmo.ToString();
               
            }
        }
    }
}