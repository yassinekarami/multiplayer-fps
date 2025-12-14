
namespace Core.Utils
{
    /// <summary>
    /// class of constant
    /// </summary>
    public static class Constant
    {
        public static class Tag
        {
            public const string SPAWN_POINT = "SpawnPoint";
            public const string PLAYER = "Player";
        }
        /// <summary>
        /// class for all pun event code
        /// </summary>
        public static class PunEventCode
        {
            public const byte updateTextEventCode = 1; 
            public const byte theGameIsReadyEventCode = 2;
            public const byte setUpPlayerInfoPanelEventCode = 3;
        }
       
    }

}
