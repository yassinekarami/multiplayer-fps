using Core.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Core.Utils.Constant;

namespace Core.Interface.WeaponUI
{
    /// <summary>
    /// Defines a contract for a subject that manages weapon UI observers and notifies them of weapon-related events.
    /// </summary>
    /// <remarks>Implementations of this interface allow observers to subscribe to and receive updates when weapon
    /// information, such as ammo count, changes. This interface follows the observer design pattern, enabling
    /// decoupled notification of interested parties. Observers must implement the IWeaponUIObserver interface to
    /// receive updates.</remarks>
    /// 
    public interface IWeaponUISubject
    {
        public void RegisterObserver(IWeaponUIObserver observer);
        public void RemoveObserver(IWeaponUIObserver observer);
        public void NotifyObservers(WeaponNotificationType type, int currentAmmo, int index);
    }

}
