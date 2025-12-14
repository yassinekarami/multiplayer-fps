using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Event
{
    public class PlayerGotShotEvent
    {
        /// <summary>
        /// event triggred when the player is shot
        /// </summary>
        public static Action<int> onPlayerShot;
    }
}

