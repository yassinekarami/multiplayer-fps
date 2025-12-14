
using System.Globalization;
using UnityEngine;

namespace Core.Utils
{
    public static class ColorUtils
    {
        /// <summary>
        /// return the color according to string
        /// </summary>
        /// <param name="colorString"></param>
        /// <returns>color</returns>
        public static Color ResolveColorFromString(string colorString)
        {
            if (colorString.ToLower().Contains("red")) return Color.red;
            else if (colorString.ToLower().Contains("green")) return Color.green;
            else if (colorString.ToLower().Contains("yellow")) return Color.yellow;
            else if (colorString.ToLower().Contains("blue")) return Color.blue;
            else if (colorString.ToLower().Contains("purple")) return Color.magenta;
            else if (colorString.ToLower().Contains("black")) return Color.black;
            else return Color.white;

        }
        /// <summary>
        /// convert string to RGBA
        /// </summary>
        /// <param name="rgbaString"></param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public static Color ParseRGBA(string rgbaString)
        {
            // Remove "RGBA(" and ")"
            string cleaned = rgbaString.Replace("RGBA(", "").Replace(")", "");

            // Split the values
            string[] parts = cleaned.Split(',');
            if (parts.Length != 4)
                throw new System.Exception("Invalid RGBA format: " + rgbaString);

            // Convert using invariant culture (for the dot ".")
            float r = float.Parse(parts[0], CultureInfo.InvariantCulture);
            float g = float.Parse(parts[1], CultureInfo.InvariantCulture);
            float b = float.Parse(parts[2], CultureInfo.InvariantCulture);
            float a = float.Parse(parts[3], CultureInfo.InvariantCulture);

            return new Color(r, g, b, a);
        }
    }

}
