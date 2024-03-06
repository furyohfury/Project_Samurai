using Defective.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Samurai
{
    public static class SaveLoadManager
    {
        private static readonly string _saveDataPath = Application.persistentDataPath + "/SaveFile.json";

        public static JSONObject SaveData;

        public static Vector3 CurrentPlayerPosition = new();
        public static string CurrentScene;
        public static string CurrentArena;
        public static bool CurrentArenaIsFinished;
        public static UnitStatsStruct PlayerStats;
        public static UnitBuffsStruct PlayerUnitBuffs;
        public static PlayerBuffsStruct PlayerOnlyBuffs;

        public static string PlayerRangeWeapon;
        public static int PlayerPickableWeaponNumberOfBullets;

        public static void UpdateSaveFile()
        {
            if (File.Exists(_saveDataPath))
            {
                File.WriteAllText(_saveDataPath, SaveData.ToString());
            }
        }


        public static void SaveLoadManagerInitialization(bool on)
        {
            if (on)
            {
                SceneManager.activeSceneChanged += SceneChanged;

                // On first launch
                if (!File.Exists(_saveDataPath))
                {

                    // Creating empty save file
                    File.WriteAllText(_saveDataPath, string.Empty);
                }
                // Buffering save file and all data in SaveLoadManager
                LoadLastSave();
            }
            else
            {
                // todo Useless?
                SceneManager.activeSceneChanged -= SceneChanged;
            }
        }

        

        #region Saving
        private static void SceneChanged(Scene fromScene, Scene toScene)
        {
            /* if (toScene.name == "MainMenuScene" || toScene.name == "AntiSpill_MainMenuScene" || 
                (SaveData.GetField(out string endSceneName, "Scene", "") && toScene.name == endSceneName))
            {
                return;
            } */
            if (toScene.name.Contains("MainMenu", StringComparison.OrdinalIgnoreCase) 
                || toScene.name.Contains("LoadingScreen",StringComparison.OrdinalIgnoreCase)) return;

            SaveData.SetField("Scene", toScene.name);
            SaveData.SetField("Arena", string.Empty);
            SaveData.SetField("ArenaIsFinished", false);
            UpdateSaveFile();
        }

        public static void ArenaSaving(string arenaName, bool isArenaFinished, Player player)
        {
#if UNITY_EDITOR
            SaveData = new(File.ReadAllText(_saveDataPath));
#endif
            SaveData.SetField("Scene", $"{SceneManager.GetActiveScene().name}");
            SaveData.SetField("Arena", arenaName);
            SaveData.SetField("ArenaIsFinished", isArenaFinished);

            SaveData.SetField("Player", PlayerJSON(player));

            UpdateSaveFile();
        }
        private static JSONObject PlayerJSON(Player player)
        {
            JSONObject UnitStats = UnitStatsToJson(player.GetUnitStats());
            JSONObject UnitBuffs = UnitBuffsToJson(player.GetUnitBuffs());
            JSONObject PlayerBuffs = PlayerBuffsToJson(player.GetPlayerBuffs());
            JSONObject PlayerTransform = TransformToJson(player.transform);

            JSONObject RangeWeapon = RangeWeaponToJson(player.RangeWeapon);
            Dictionary<string, JSONObject> jsonDic = new(){
                { nameof(UnitStats), UnitStats },
                { nameof(UnitBuffs), UnitBuffs },
                { nameof(PlayerBuffs), PlayerBuffs },
                { nameof(RangeWeapon), RangeWeapon },
                { nameof(PlayerTransform), PlayerTransform}
            };

            // _saveData.SetField("Player", $"{{\"UnitStats\" : {UnitStats}, \"UnitBuffs\": {UnitBuffs}, \"PlayerBuffs\": {PlayerBuffs}, \"RangeWeapon\": {RangeWeapon}");
            JSONObject playerJSON = new("");
            foreach (var key in jsonDic.Keys)
            {
                playerJSON.AddField(key, jsonDic[key]);
            }
            return playerJSON;
        }
        private static JSONObject UnitStatsToJson(UnitStatsStruct unitStats)
        {
            return new($"{{\r\n      \"HP\": {unitStats.HP},\r\n      \"MaxHP\": {unitStats.MaxHP},\r\n      \"MoveSpeed\": {unitStats.MoveSpeed}\r\n    }}");
        }
        private static JSONObject UnitBuffsToJson(UnitBuffsStruct unitBuffs)
        {
            return new($"{{\r\n      \"MeleeWeaponDamageBuff\": {unitBuffs.MeleeWeaponDamageBuff},\r\n      \"MeleeAttackCDBuff\": {unitBuffs.MeleeAttackCDBuff}\r\n    }}");
        }
        private static JSONObject PlayerBuffsToJson(PlayerBuffsStruct playerBuffs)
        {
            return new($"{{\r\n      \"PickableWeaponDamageBuff\": {playerBuffs.PickableWeaponDamageBuff},\r\n      \"SlomoDurationBuff\": {playerBuffs.SlomoDurationBuff}\r\n    }}, \"DefaultPlayerWeaponDamageBuff\": {playerBuffs.DefaultPlayerWeaponDamageBuff}}}");
        }
        private static JSONObject RangeWeaponToJson(RangeWeapon rweapon)
        {
            JSONObject rw = new();
            if (rweapon is DefaultPlayerWeapon)
            {
                // return new("{ \"Type\": DefaultPlayerWeapon }");
                rw.AddField("Type", "DefaultPlayerWeapon");
            }
            else
            {
                // return new($"{{ \"Type\": {rweapon.GetType().Name}, \"NumberOfbulletsForPlayer\": {rweapon.NumberOfBulletsForPlayer}}}\r\n}}");
                rw.AddField("Type", rweapon.GetType().Name);
                rw.AddField("NumberOfBulletsForPlayer", rweapon.NumberOfBulletsForPlayer);
            }
            return rw;
        }
        private static JSONObject TransformToJson(Transform transform)
        {
            JSONObject tr = new();
            JSONObject trPos = new();
            trPos.AddField("x", transform.position.x);
            trPos.AddField("y", transform.position.y);
            trPos.AddField("z", transform.position.z);

            tr.AddField("Position", trPos);
            return tr;
        }
        #endregion

        // Loads everything from save file to this class fields
        #region Loading        
        public static void LoadLastSave()
        {
            SaveData = new(File.ReadAllText(_saveDataPath));

            // Arena
            if (SaveData.GetField(out string arenaName, "Arena", string.Empty))
            {
                CurrentArena = arenaName;
            }
            if (SaveData.GetField(out bool arenaIsFinished, "ArenaIsFinished", false))
            {
                CurrentArenaIsFinished = arenaIsFinished;
            }

            // Scene
            if (SaveData.GetField(out string sceneName, "Scene", string.Empty))
            {
                CurrentScene = sceneName;
            }

            // Player
            var playerJSON = SaveData.GetField("Player");
            if (playerJSON != null)
            {
                CurrentPlayerPosition = LoadPlayerTransform();
                PlayerStats = LoadStats<UnitStatsStruct>(playerJSON, "UnitStats");
                PlayerUnitBuffs = LoadStats<UnitBuffsStruct>(playerJSON, "UnitBuffs");
                PlayerOnlyBuffs = LoadStats<PlayerBuffsStruct>(playerJSON, "PlayerBuffs");
                LoadPlayerRangeWeapon(playerJSON);
            }
        }

        // Mb do for rotation and scale
        private static Vector3 LoadPlayerTransform()
        {
            JSONObject player = SaveData.GetField("Player");
            JSONObject transform = player.GetField("PlayerTransform");
            JSONObject position = transform.GetField("Position");
            Vector3 playerPos = new();
            playerPos.x = position.GetField(out float x, "x", 0) ? x : 0;
            playerPos.y = position.GetField(out float y, "y", 0) ? y : 0;
            playerPos.z = position.GetField(out float z, "z", 0) ? z : 0;
            return playerPos;
        }
        private static T LoadStats<T>(JSONObject unitJsonObj, string fieldName)
        {
            var unitStatsJsonObj = unitJsonObj.GetField(fieldName);

            // Reflections SetField doesnt work with structs. Have to use a box
            T stats = (T)Activator.CreateInstance(typeof(T));
            object box = stats;
            var ussFields = stats.GetType().GetFields();
            for (var i = 0; i < unitStatsJsonObj.count; i++)
            {
                var field = ussFields.Single((ussfield) => ussfield.Name == unitStatsJsonObj.keys[i]);
                if (unitStatsJsonObj.list[i].isInteger) field.SetValue(box, (int)unitStatsJsonObj.list[i].intValue);
                else if (unitStatsJsonObj.list[i].isNumber) field.SetValue(box, (float)unitStatsJsonObj.list[i].floatValue);
            }
            return (T)box;
        }
        private static void LoadPlayerRangeWeapon(JSONObject unitJsonObj)
        {
            var rWeaponJson = unitJsonObj.GetField("RangeWeapon");
            if (rWeaponJson.GetField(out string type, "Type", ""))
            {
                PlayerRangeWeapon = type;
                if (rWeaponJson.GetField(out int numberOfBullets, "NumberOfBulletsForPlayer", 0) && numberOfBullets != 0)
                {
                    PlayerPickableWeaponNumberOfBullets = numberOfBullets;
                }
            }
        }
        #endregion


        public static string LoadResourceTextfile(string path)
        {

            string filePath = path.Replace(".json", "");

            TextAsset targetFile = Resources.Load<TextAsset>(filePath);

            return targetFile.text;
        }
    }
}

