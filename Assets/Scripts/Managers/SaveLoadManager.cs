using Defective.JSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Samurai
{
    public static class SaveLoadManager
    {
        public static LoadingType LoadingType = LoadingType.None;

        private static readonly string _saveDataPath = Constants.SaveDataPath;

        public static JSONObject SaveData;

        /* public static Vector3 CurrentPlayerPosition { get => LoadPlayerTransform(); private set; }
        public static string CurrentScene { get => SaveData.GetField("Scene").stringValue; }
        public static string CurrentArena { get => SaveData.GetField("Arena").stringValue; }
        public static bool CurrentArenaIsFinished { get => SaveData.GetField("ArenaIsFinished").boolValue; }

        public static bool PlayerDataExist = false;
        public static UnitStatsStruct PlayerStats { get => LoadStats<UnitStatsStruct>(SaveData.GetField("Player").GetField("UnitStats"), "UnitStats"); }
        public static UnitBuffsStruct PlayerUnitBuffs { get => LoadStats<UnitBuffsStruct>(SaveData.GetField("Player").GetField("UnitBuffs"), "UnitBuffs"); }
        public static PlayerBuffsStruct PlayerOnlyBuffs { get => LoadStats<PlayerBuffsStruct>(SaveData.GetField("Player").GetField("PlayerBuffs"), "PlayerBuffs"); }

        public static string PlayerRangeWeapon { get => LoadPlayerRangeWeapon(SaveData.GetField("Player"), out PlayerPickableWeaponNumberOfBullets); }
        public static int PlayerPickableWeaponNumberOfBullets; */

        public static string CurrentScene;
        public static string CurrentArena;
        public static bool CurrentArenaIsFinished;

        public static Vector3 CurrentPlayerPosition;
        public static UnitStatsStruct PlayerStats;
        public static UnitBuffsStruct PlayerUnitBuffs;
        public static PlayerBuffsStruct PlayerBuffs;
        public static string PlayerRangeWeapon;
        public static int NumberOfBulletsForPlayer;
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
                    File.WriteAllText(_saveDataPath, string.Empty);
                }
                else
                {
                    SaveData = new(_saveDataPath);
                    LoadSaveFile();
                }
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
            if (fromScene.name != toScene.name &&
                !(fromScene.name.Contains("MainMenu", StringComparison.OrdinalIgnoreCase) || toScene.name.Contains("MainMenu", StringComparison.OrdinalIgnoreCase)))
            {
                SaveData.SetField("Scene", toScene.name);
                SaveData.SetField("Arena", string.Empty);
                SaveData.SetField("ArenaIsFinished", false);
            }
            else if ()


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
        public static void LoadSaveFile()
        {
            SaveData = new(File.ReadAllText(_saveDataPath));

            // Scene
            if (SaveData.GetField(out string sceneName, "Scene", string.Empty))
            {
                CurrentScene = sceneName;
            }

            // Arena
            if (SaveData.GetField(out string arenaName, "Arena", string.Empty))
            {
                CurrentArena = arenaName;
            }
            if (SaveData.GetField(out bool arenaIsFinished, "ArenaIsFinished", false))
            {
                CurrentArenaIsFinished = arenaIsFinished;
            }

            // Player
            var playerJSON = SaveData.GetField("Player");
            if (playerJSON != null)
            {
                CurrentPlayerPosition = LoadPlayerTransform();
                PlayerStats = LoadStats<UnitStatsStruct>(playerJSON, "UnitStats");
                PlayerUnitBuffs = LoadStats<UnitBuffsStruct>(playerJSON, "UnitBuffs");
                PlayerBuffs = LoadStats<PlayerBuffsStruct>(playerJSON, "PlayerBuffs");
                PlayerRangeWeapon = LoadPlayerRangeWeapon(playerJSON, out NumberOfBulletsForPlayer);
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
        private static string LoadPlayerRangeWeapon(JSONObject unitJsonObj, out int numberOfBullets)
        {
            var rWeaponJson = unitJsonObj.GetField("RangeWeapon");
            rWeaponJson.GetField(out string type, "Type", "");
            rWeaponJson.GetField(out int bullets, "NumberOfBulletsForPlayer", 0);
            numberOfBullets = bullets;
            return type;
        }
        #endregion


        public static string LoadResourceTextfile(string path)
        {

            string filePath = path.Replace(".json", "");

            TextAsset targetFile = Resources.Load<TextAsset>(filePath);

            return targetFile.text;
        }

        public static void NewGameStart()
        {
            SaveData.Clear();
            File.WriteAllText(_saveDataPath, string.Empty);
        }
    }
}


