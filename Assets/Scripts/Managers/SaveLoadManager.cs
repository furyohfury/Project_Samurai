using Defective.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Samurai
{
    public static class SaveLoadManager
    {

        /* private static string _sceneString = $"\"Scene\": {SceneManager.GetActiveScene().name}";


        private static string _arenaName = string.Empty;
        private static string _arenaString = $"\"Arena\": {_arenaName}";

        private static bool _isArenaFinished = false;
        private static string _arenaFinishedString = $"\"ArenaFinished\": {_isArenaFinished}";

        private static string saveString = string.Empty; */
        /* private static string _playerString = $"\"Player\": \{ \}";

        private static string UnitStatsConvert(UnitStatsStruct unitStats)
        {
            string hp = $"\"HP\": {unitStats.HP}";
            string maxhp = $"\"MaxHP\": {unitStats.MaxHP}";
            string ms = $"\"MoveSpeed\": {unitStats.MoveSpeed}";
            return string.Concat(hp, ",", maxhp, ",", ms);
        } */

        private static string _saveFilePath = Application.persistentDataPath + "/SaveFile.json";


        public static JSONObject _saveData;
        public static void UpdateSaveFile()
        {
            if (File.Exists(_saveFilePath))
            {
                File.WriteAllText(_saveFilePath, _saveData.ToString());
            }
        }

        public static void SaveLoadManagerInitialization(bool on)
        {
            if (on)
            {
                SceneManager.activeSceneChanged += SceneChanged;

                // On first launch
                if (!File.Exists(_saveFilePath))
                {
                    // string defaultSaveFileText = LoadResourceTextfile("DefaultSaveFile.json");
                    // File.WriteAllText(_saveFilePath, defaultSaveFileText);

                    // Creating empty save file
                    File.WriteAllText(_saveFilePath, string.Empty);
                }
                // Buffering save file in SaveLoadManager
                _saveData = new(File.ReadAllText(_saveFilePath));
            }
            else
            {
                SceneManager.activeSceneChanged -= SceneChanged;
            }
        }

        public static string LoadResourceTextfile(string path)
        {

            string filePath = path.Replace(".json", "");

            TextAsset targetFile = Resources.Load<TextAsset>(filePath);

            return targetFile.text;
        }

        private static void SceneChanged(Scene fromScene, Scene toScene)
        {
            if (toScene.name == "MainMenuScene") return;

            _saveData.SetField("Scene", toScene.name);
            _saveData.SetField("Arena", string.Empty);
            _saveData.SetField("ArenaIsFinished", false);
            UpdateSaveFile();
        }

        public static void ArenaSaving(string arenaName, bool isArenaFinished, Player player)
        {
#if UNITY_EDITOR
            _saveData = new(File.ReadAllText(_saveFilePath));
#endif
            _saveData.SetField("Scene", $"{SceneManager.GetActiveScene().name}");
            _saveData.SetField("Arena", arenaName);
            _saveData.SetField("ArenaIsFinished", isArenaFinished);


            JSONObject UnitStats = UnitStatsToJson(player.GetUnitStats());
            JSONObject UnitBuffs = UnitBuffsToJson(player.GetUnitBuffs());
            JSONObject PlayerBuffs = PlayerBuffsToJson(player.GetPlayerBuffs());
            JSONObject RangeWeapon = new($"{player.RangeWeapon.NumberOfBulletsForPlayer}");
            JSONObject[] ar = { UnitStats, UnitBuffs, PlayerBuffs, RangeWeapon };

            _saveData.SetField("Player", new JSONObject(ar));
            UpdateSaveFile();
        }

        /* public static string GetSaveDataFromArena(string arenaName, bool isArenaFinished, Player player)
        {
            string sceneString = $"\"Scene\": {SceneManager.GetActiveScene().name}";

            string encodedString = $"{{\r\n  \"Scene\": \"{sceneString}\",\r\n\r\n  \"Arena\": \"{arenaName}\",\r\n\r\n  \"ArenaIsFinished\": {isArenaFinished},\r\n\r\n  \"Player\": {{\r\n    \"UnitStats\": {{\r\n      \"HP\": {player.GetUnitStats().HP},\r\n      \"MaxHP\": {player.GetUnitStats().MaxHP},\r\n      \"MoveSpeed\": {player.GetUnitStats().MoveSpeed}\r\n    }},\r\n\r\n    \"UnitBuffs\": {{\r\n      \"RangeWeaponDamageBuff\": 1,\r\n      \"MeleeWeaponDamageBuff\": 1,\r\n      \"MeleeAttackCDBuff\": 1\r\n    }},\r\n\r\n    \"PlayerBuffs\" {{\r\n      \"PickableWeaponDamageBuff\": 1,\r\n      \"SlomoDurationBuff\": 1\r\n    }},\r\n\r\n    \"RangeWeapon\": {{ \"NumberOfbulletsForPlayer\": 3 }}\r\n  }}\r\n}}";
            return encodedString;
        } */
        public static JSONObject UnitStatsToJson(UnitStatsStruct unitStats)
        {
            return new($"{{\r\n      \"HP\": {unitStats.HP},\r\n      \"MaxHP\": {unitStats.MaxHP},\r\n      \"MoveSpeed\": {unitStats.MoveSpeed}\r\n    }}");
        }
        public static JSONObject UnitBuffsToJson(UnitBuffsStruct unitBuffs)
        {
            return new($"{{\r\n      \"MeleeWeaponDamageBuff\": {unitBuffs.MeleeWeaponDamageBuff},\r\n      \"MeleeAttackCDBuff\": {unitBuffs.MeleeAttackCDBuff}\r\n    }}");
        }
        public static JSONObject PlayerBuffsToJson(PlayerBuffsStruct playerBuffs)
        {
            return new($"{{\r\n      \"PickableWeaponDamageBuff\": {playerBuffs.PickableWeaponDamageBuff},\r\n      \"SlomoDurationBuff\": {playerBuffs.SlomoDurationBuff}\r\n    }}, \"DefaultPlayerWeaponDamageBuff\": {playerBuffs.DefaultPlayerWeaponDamageBuff}");
        }
        public static JSONObject RangeWeaponBuffsToJson(RangeWeapon rweapon)
        {
            return new($"{{ \"NumberOfbulletsForPlayer\": 3 }}\r\n  }}\r\n}}");
        }


        public static void Test(string encodedString)
        {
            // var encodedString = "{\"field1\": 0.5,\"field2\": \"sampletext\",\"field3\": [1,2,3]}";
            var jsonObject = new JSONObject(encodedString);
            AccessData(jsonObject);
        }

        public static void AccessData(JSONObject jsonObject)
        {
            switch (jsonObject.type)
            {
                case JSONObject.Type.Object:
                    for (var i = 0; i < jsonObject.list.Count; i++)
                    {
                        var key = jsonObject.keys[i];
                        var value = jsonObject.list[i];
                        Debug.Log(key);
                        AccessData(value);
                    }
                    break;
                case JSONObject.Type.Array:
                    foreach (JSONObject element in jsonObject.list)
                    {
                        AccessData(element);
                    }
                    break;
                case JSONObject.Type.String:
                    Debug.Log(jsonObject.stringValue);
                    break;
                case JSONObject.Type.Number:
                    Debug.Log(jsonObject.floatValue);
                    break;
                case JSONObject.Type.Bool:
                    Debug.Log(jsonObject.boolValue);
                    break;
                case JSONObject.Type.Null:
                    Debug.Log("Null");
                    break;
                case JSONObject.Type.Baked:
                    Debug.Log(jsonObject.stringValue);
                    break;
            }
        }
    }
}
