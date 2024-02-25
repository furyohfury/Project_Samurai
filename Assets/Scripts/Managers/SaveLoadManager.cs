using Defective.JSON;
using System;
using System.Collections;
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
        private static readonly string _saveDataPath = Application.persistentDataPath + "/SaveFile.json";

        public static JSONObject _saveData;

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
                File.WriteAllText(_saveDataPath, _saveData.ToString());
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
                    // string defaultSaveFileText = LoadResourceTextfile("DefaultSaveFile.json");
                    // File.WriteAllText(_saveFilePath, defaultSaveFileText);

                    // Creating empty save file
                    File.WriteAllText(_saveDataPath, string.Empty);
                }
                // Buffering save file in SaveLoadManager
                _saveData = new(File.ReadAllText(_saveDataPath));
            }
            else
            {
                // todo Useless?
                SceneManager.activeSceneChanged -= SceneChanged;
            }
        }

        public static string LoadResourceTextfile(string path)
        {

            string filePath = path.Replace(".json", "");

            TextAsset targetFile = Resources.Load<TextAsset>(filePath);

            return targetFile.text;
        }

        #region Saving
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
            _saveData = new(File.ReadAllText(_saveDataPath));
#endif
            _saveData.SetField("Scene", $"{SceneManager.GetActiveScene().name}");
            _saveData.SetField("Arena", arenaName);
            _saveData.SetField("ArenaIsFinished", isArenaFinished);

            _saveData.SetField("Player", PlayerJSON(player));

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

        #region Loading
        public static void LoadLastSave()
        {
            _saveData = new(File.ReadAllText(_saveDataPath));

            // Player
            var playerJSON = _saveData.GetField("Player");
            if (playerJSON != null)
            {
                CurrentPlayerPosition = LoadPlayerTransform();
                PlayerStats = LoadStats<UnitStatsStruct>(playerJSON, "UnitStats");
                PlayerUnitBuffs = LoadStats<UnitBuffsStruct>(playerJSON, "UnitBuffs");
                PlayerOnlyBuffs = LoadStats<PlayerBuffsStruct>(playerJSON, "PlayerBuffs");
                LoadPlayerRangeWeapon(playerJSON);
            }
            else
            {
                CurrentPlayerPosition = Vector3.zero;
            }


            // Arena
            if (_saveData.GetField(out string arenaName, "Arena", string.Empty))
            {
                CurrentArena = arenaName;
            }
            if (_saveData.GetField(out bool arenaIsFinished, "ArenaIsFinished", false))
            {
                CurrentArenaIsFinished = arenaIsFinished;
            }

            if (_saveData.GetField(out string sceneName, "Scene", "MainMenu"))
            {
                SceneManager.LoadSceneAsync(sceneName);

                // todo enable finish arena soomehow
            }
            else
            {
                Debug.LogError("No scene found in save file");
            }

        }

        // Mb do for rotation and scale
        private static Vector3 LoadPlayerTransform()
        {
            JSONObject player = _saveData.GetField("Player");
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

            // Reflections SetField doesnt work with structs
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
