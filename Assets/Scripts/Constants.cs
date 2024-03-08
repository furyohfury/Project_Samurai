using UnityEngine;

namespace Samurai
{
    public static class Constants
    {
        public static int FloorLayer = 6;
        public static int ObstacleLayer = 7;
        public static string SaveDataPath = Application.persistentDataPath + "/SaveFile.json";
        public static string StartGameSceneName = "StartGameScene";
    }
}