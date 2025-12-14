using System;
using UnityEngine;

namespace Core.Event
{
    public class CreatePlayerEvent
    {
        /// <summary>
        /// event triggred when the player chose a color
        /// </summary>
        public static Action<string, Color> onColorChoosed;
    }

}
