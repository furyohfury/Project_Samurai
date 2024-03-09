using Defective.JSON;
using MoreMountains.Feedbacks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

namespace Samurai
{
    public class SaveLoadManager : MonoBehaviour
    {
        public LoadingType LoadingType = LoadingType.None;
        public Coroutine LoadingCoroutine;

        [SerializeField]
        private MMF_Player _loadingPlayer;
        private MMF_LoadScene _loadSceneFeedback;

        private string _saveDataPath;

        public JSONObject SaveData;

        public string CurrentScene;
        public string CurrentArena;
        public bool CurrentArenaIsFinished;

        public Vector3 CurrentPlayerPosition;
        public UnitStatsStruct PlayerUnitStats;
        public UnitBuffsStruct PlayerUnitBuffs;
        public PlayerBuffsStruct PlayerBuffs;
        public string PlayerRangeWeapon;
        public int NumberOfBulletsForPlayer;

        #region UnityMethods
        private void Awake()
        {
            _saveDataPath = Constants.SaveDataPath;
        }
        private void Start()
        {
            SaveLoadManagerInitialization();
        }
        #endregion

        public void UpdateSaveFile()
        {
            if (File.Exists(_saveDataPath) && SaveData != null)
            {
                File.WriteAllText(_saveDataPath, SaveData.ToString());
            }
        }

        public void SaveLoadManagerInitialization()
        {
            _loadSceneFeedback = _loadingPlayer.GetFeedbackOfType<MMF_LoadScene>();

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

        public void ChangeScene(LoadingType type, string destinationScene = "MainMenu")
        {
            LoadingType = type;
            switch (type)
            {
                case LoadingType.SwitchBetweenLevels:
                    LoadingCoroutine = StartCoroutine(SwitchBetweenLevels(destinationScene));
                    break;
                case LoadingType.NewGameFromMainMenu:
                    LoadingCoroutine = StartCoroutine(NewGameCoroutine());
                    break;
                default:
                    LoadingCoroutine = StartCoroutine(DefaultLoadCoroutine(CurrentScene));
                    break;
            }
        }
        private void FinishingLoad()
        {
            LoadingCoroutine = null;
            UpdateSaveFile();
            FindObjectOfType<SaveLoadSceneAssistant>().InitializeScene(LoadingType);
        }

        private IEnumerator SwitchBetweenLevels(string destinationScene)
        {
            var loadingTask = SceneManager.LoadSceneAsync(destinationScene);

            while (!loadingTask.isDone) yield return null;

            SaveData.SetField("Scene", destinationScene);
            SaveData.SetField("Arena", string.Empty);
            SaveData.SetField("ArenaIsFinished", false);

            var playerPosition = FindObjectOfType<Player>().transform.position;
            SaveData.GetField("Player")
                .GetField("Transform")
                .SetField("Position", $"x: {playerPosition.x}, y: {playerPosition.y}, z: {playerPosition.z}");

            FinishingLoad();
        }
        private IEnumerator NewGameCoroutine()
        {
            SaveData = new(string.Empty);
            var loadingTask = SceneManager.LoadSceneAsync(Constants.StartGameSceneName);
            while (!loadingTask.isDone) yield return null;

            SaveData = new(string.Empty);
            SaveData.SetField("Scene", Constants.StartGameSceneName);
            SaveData.SetField("Arena", string.Empty);
            SaveData.SetField("ArenaIsFinished", false);

            var player = FindObjectOfType<Player>();
            SaveData.SetField("Player", PlayerJSON(player));

            FinishingLoad();
        }
        private IEnumerator DefaultLoadCoroutine(string _)
        {
            var loadingTask = SceneManager.LoadSceneAsync(CurrentScene);
            while (!loadingTask.isDone) yield return null;
            FinishingLoad();
        }



        #region Saving

        public void ArenaSaving(string arenaName, bool isArenaFinished, Player player)
        {
#if UNITY_EDITOR
            // SaveData = new(File.ReadAllText(_saveDataPath));
#endif
            // SaveData.SetField("Scene", $"{SceneManager.GetActiveScene().name}");
            SaveData.SetField("Arena", arenaName);
            SaveData.SetField("ArenaIsFinished", isArenaFinished);

            SaveData.SetField("Player", PlayerJSON(player));

            UpdateSaveFile();
        }
        private JSONObject PlayerJSON(Player player)
        {
            JSONObject Transform = TransformToJson(player.transform);
            JSONObject UnitStats = StructToJson(player.GetUnitStats());
            JSONObject UnitBuffs = StructToJson(player.GetUnitBuffs());
            JSONObject PlayerBuffs = StructToJson(player.GetPlayerBuffs());

            JSONObject RangeWeapon = RangeWeaponToJson(player.RangeWeapon);
            Dictionary<string, JSONObject> jsonDic = new()
            {
                { nameof(UnitStats), UnitStats },
                { nameof(UnitBuffs), UnitBuffs },
                { nameof(PlayerBuffs), PlayerBuffs },
                { nameof(RangeWeapon), RangeWeapon },
                { nameof(Transform), Transform }
            };

            JSONObject playerJSON = new("");
            foreach (var key in jsonDic.Keys)
            {
                playerJSON.AddField(key, jsonDic[key]);
            }
            return playerJSON;
        }
        private JSONObject StructToJson<T>(T @struct)
        {
            var jsonObj = new JSONObject();
            var fields = @struct.GetType().GetFields();
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(int))
                {
                    jsonObj.AddField(field.Name, (int)field.GetValue(@struct));
                }
                if (field.FieldType == typeof(float))
                {
                    jsonObj.AddField(field.Name, (float)field.GetValue(@struct));
                }
            }
            return jsonObj;
        }
        private JSONObject RangeWeaponToJson(RangeWeapon rweapon)
        {
            JSONObject rw = new();
            if (rweapon is DefaultPlayerWeapon)
            {
                rw.AddField("Type", "DefaultPlayerWeapon");
            }
            else
            {
                rw.AddField("Type", rweapon.GetType().Name);
                rw.AddField("NumberOfBulletsForPlayer", rweapon.NumberOfBulletsForPlayer);
            }
            return rw;
        }
        private JSONObject TransformToJson(Transform transform)
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
        public void LoadSaveFile()
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
                PlayerUnitStats = LoadJSONtoStruct<UnitStatsStruct>(playerJSON, "UnitStats");
                PlayerUnitBuffs = LoadJSONtoStruct<UnitBuffsStruct>(playerJSON, "UnitBuffs");
                PlayerBuffs = LoadJSONtoStruct<PlayerBuffsStruct>(playerJSON, "PlayerBuffs");
                PlayerRangeWeapon = LoadPlayerRangeWeapon(playerJSON, out NumberOfBulletsForPlayer);
            }
        }

        // Mb do for rotation and scale
        private Vector3 LoadPlayerTransform()
        {
            JSONObject player = SaveData.GetField("Player");
            JSONObject transform = player.GetField("Transform");
            JSONObject position = transform.GetField("Position");
            Vector3 playerPos = new();
            playerPos.x = position.GetField(out float x, "x", 0) ? x : 0;
            playerPos.y = position.GetField(out float y, "y", 0) ? y : 0;
            playerPos.z = position.GetField(out float z, "z", 0) ? z : 0;
            return playerPos;
        }
        private T LoadJSONtoStruct<T>(JSONObject unitJsonObj, string fieldName)
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
        private string LoadPlayerRangeWeapon(JSONObject unitJsonObj, out int numberOfBullets)
        {
            var rWeaponJson = unitJsonObj.GetField("RangeWeapon");
            rWeaponJson.GetField(out string type, "Type", "");
            rWeaponJson.GetField(out int bullets, "NumberOfBulletsForPlayer", 0);
            numberOfBullets = bullets;
            return type;
        }
        #endregion
    }
}


