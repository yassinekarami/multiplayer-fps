using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Model;
using System.Linq;
using Core.Event;
namespace Game.Shared.Gameplay {
    public class WeaponHandler : MonoBehaviour
{
    Dictionary<Weapon, GameObject> weaponsDictionary = new Dictionary<Weapon, GameObject>();

    Weapon firstWeapon = new Weapon(0, "Sci-Fi Gun", 30, true, 10);
    Weapon secondWeapon = new Weapon(1, "RL0N-25_low", 30, false, 20);
    Weapon thirdWeapon = new Weapon(2, "Bio Integrity Gun", 30, false, 30);


    Weapon currentWeapon;

    private void Awake()
    {
        weaponsDictionary.Add(firstWeapon, GameObject.Find("Sci-Fi Gun"));
        weaponsDictionary.Add(secondWeapon, GameObject.Find("RL0N-25_low"));
        weaponsDictionary.Add(thirdWeapon, GameObject.Find("Bio Integrity Gun"));


        foreach (KeyValuePair<Weapon, GameObject> entry in weaponsDictionary)
        {
            entry.Value.SetActive(entry.Key.isActive);
            if (entry.Key.isActive)
            {
                currentWeapon = entry.Key;
            }
        }

    }

    public void Shot()
    {
        if (currentWeapon.currentAmo <= 0)
        {
            Debug.Log("no more amo");
        }
        else
        {
            currentWeapon.decreaseAmo();
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider);
                PlayerGotShotEvent.onPlayerShot?.Invoke(currentWeapon.damage);
                 
                if (hit.collider.gameObject.GetComponentInParent<PlayerStates>() != null)
                {
                    hit.collider.gameObject.GetComponentInParent<PlayerStates>().decreaseHealth(currentWeapon.damage);
                }
            }
        }

    }
    //  CreatePlayerEvent.onColorChoosed?.Invoke(PhotonNetwork.LocalPlayer.NickName, Color.red);

    public void ChangeWeapon()
    {

        Weapon activeWeapon = weaponsDictionary.FirstOrDefault(x => x.Key.isActive == true).Key;
        int activeIndex = activeWeapon.id + 1 >= weaponsDictionary.Keys.Count ? 0 : activeWeapon.id + 1;

        foreach (KeyValuePair<Weapon, GameObject> entry in weaponsDictionary)
        {
            if (activeIndex == entry.Key.id)
            {
                entry.Key.isActive = true;
                currentWeapon = entry.Key;
            }
            else
            {
                entry.Key.isActive = false;
            }

            entry.Value.gameObject.SetActive(entry.Key.isActive);
        }
    }
}

}
