using Core.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Interface.WeaponUI
{
    public interface IWeaponUIObserver
    {
        public void OnNotify(Constant.WeaponNotificationType type, int currentAmmo, int index);
    }

}
