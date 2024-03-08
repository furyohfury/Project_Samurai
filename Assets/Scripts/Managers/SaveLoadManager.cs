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

namespace Samurai
{
    public class SaveLoadManager : MonoBehaviour
    {
        public LoadingType LoadingType = LoadingType.None;

        [SerializeField]
        private MMF_Player _loadingPlayer;        
        private MMF_LoadScene _loadSceneFeedback;

        private string _saveDataPath;

        public JSONObject SaveData;

        /* public Vector3 CurrentPlayerPosition { get => LoadPlayerTransform(); private set; }
        public string CurrentScene { get => SaveData.GetField("Scene").stringValue; }
        public string CurrentArena { get => SaveData.GetField("Arena").stringValue; }
        public bool CurrentArenaIsFinished { get => SaveData.GetField("ArenaIsFinished").boolValue; }

        public bool PlayerDataExist = false;
        public UnitStatsStruct PlayerStats { get => LoadStats<UnitStatsStruct>(SaveData.GetField("Player").GetField("UnitStats"), "UnitStats"); }
        public UnitBuffsStruct PlayerUnitBuffs { get => LoadStats<UnitBuffsStruct>(SaveData.GetField("Player").GetField("UnitBuffs"), "UnitBuffs"); }
        public PlayerBuffsStruct PlayerOnlyBuffs { get => LoadStats<PlayerBuffsStruct>(SaveData.GetField("Player").GetField("PlayerBuffs"), "PlayerBuffs"); }

        public string PlayerRangeWeapon { get => LoadPlayerRangeWeapon(SaveData.GetField("Player"), out PlayerPickableWeaponNumberOfBullets); }
        public int PlayerPickableWeaponNumberOfBullets; */

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
            SaveLoadManagerInitialization(true);
        }
        #endregion

        public void UpdateSaveFile()
        {
            if (File.Exists(_saveDataPath) && SaveData != null)
            {
                File.WriteAllText(_saveDataPath, SaveData.ToString());
            }
        }


        public void SaveLoadManagerInitialization(bool on)
        {
            if (on)
            {
                _loadSceneFeedback = _loadingPlayer.GetFeedbackOfType<MMF_LoadScene>();

                // SceneManager.activeSceneChanged += SceneChangedOnEvent;

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
                // SceneManager.activeSceneChanged -= SceneChangedOnEvent;
            }
        }

        public void ChangeScene(LoadingType type, string destinationScene)
        {
            LoadingType = type;
            switch (type)
            {
                case LoadingType.SwitchBetweenLevels:
                    StartCoroutine(SwitchBetweenLevels(destinationScene));
                    break;
                case LoadingType.NewGameFromMainMenu:                    
                    StartCoroutine(NewGameCoroutine(destinationScene));                    
                    break;
                default : break;
            }

            UpdateSaveFile();
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
        }
        private IEnumerator NewGameCoroutine(string destinationScene)
        {
            SaveData = new(string.Empty);
            var loadingTask = SceneManager.LoadSceneAsync(destinationScene);            
            while (!loadingTask.isDone) yield return null;

            SaveData = new(string.Empty);
            SaveData.SetField("Scene", destinationScene);
            SaveData.SetField("Arena", string.Empty);
            SaveData.SetField("ArenaIsFinished", false);

            var player = FindObjectOfType<Player>();
            SaveData.GetField("Player")
                .GetField("Transform")
                .SetField("Position", $"x: {player.transform.position.x}, y: {player.transform.position.y}, z: {player.transform.position.z}");

            PlayerUnitStats = player.GetUnitStats();
            PlayerUnitBuffs = player.GetUnitBuffs();
            PlayerBuffs = player.GetPlayerBuffs();
            PlayerRangeWeapon = player.RangeWeapon.GetType().Name;
            NumberOfBulletsForPlayer = player.RangeWeapon.NumberOfBulletsForPlayer;
        }



        #region Saving
        // Inactive rn
        private void SceneChangedOnEvent(Scene fromScene, Scene toScene)
        {
            // New game
            if (SaveData.count <= 0)
            {
                LoadingType = LoadingType.NewGameFromMainMenu;

                SaveData = new(string.Empty);
                SaveData.SetField("Scene", toScene.name);
                SaveData.SetField("Arena", string.Empty);
                SaveData.SetField("ArenaIsFinished", false);

                var player = FindObjectOfType<Player>();
                SaveData.GetField("Player")
                    .GetField("Transform")
                    .SetField("Position", $"x: {player.transform.position.x}, y: {player.transform.position.y}, z: {player.transform.position.z}");

                PlayerUnitStats = player.GetUnitStats();
                PlayerUnitBuffs = player.GetUnitBuffs();
                PlayerBuffs = player.GetPlayerBuffs();
                PlayerRangeWeapon = player.RangeWeapon.GetType().Name;
                NumberOfBulletsForPlayer = player.RangeWeapon.NumberOfBulletsForPlayer;
            }
            // Switch Between Levels
            else if (fromScene.name != toScene.name &&
                !(fromScene.name.Contains("MainMenu", StringComparison.OrdinalIgnoreCase) || toScene.name.Contains("MainMenu", StringComparison.OrdinalIgnoreCase)))
            {
                LoadingType = LoadingType.SwitchBetweenLevels;

                SaveData.SetField("Scene", toScene.name);
                SaveData.SetField("Arena", string.Empty);
                SaveData.SetField("ArenaIsFinished", false);

                var playerPosition = FindObjectOfType<Player>().transform.position;
                SaveData.GetField("Player")
                    .GetField("Transform")
                    .SetField("Position", $"x: {playerPosition.x}, y: {playerPosition.y}, z: {playerPosition.z}");
            }
            // Continue
            else if (fromScene.name.Contains("MainMenu", StringComparison.OrdinalIgnoreCase))
            {
                LoadingType = LoadingType.ContinueFromMainMenu;
            }
            // Checkpoint Reload
            else if (fromScene.name != toScene.name)
            {
                LoadingType = LoadingType.CheckpointReload;
            }

            UpdateSaveFile();
        }

        public void ArenaSaving(string arenaName, bool isArenaFinished, Player player)
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
        private JSONObject PlayerJSON(Player player)
        {
            JSONObject UnitStats = UnitStatsToJson(player.GetUnitStats());
            JSONObject UnitBuffs = UnitBuffsToJson(player.GetUnitBuffs());
            JSONObject PlayerBuffs = PlayerBuffsToJson(player.GetPlayerBuffs());
            JSONObject Transform = TransformToJson(player.transform);

            JSONObject RangeWeapon = RangeWeaponToJson(player.RangeWeapon);
            Dictionary<string, JSONObject> jsonDic = new(){
                { nameof(UnitStats), UnitStats },
                { nameof(UnitBuffs), UnitBuffs },
                { nameof(PlayerBuffs), PlayerBuffs },
                { nameof(RangeWeapon), RangeWeapon },
                { nameof(Transform), Transform}
            };

            // _saveData.SetField("Player", $"{{\"UnitStats\" : {UnitStats}, \"UnitBuffs\": {UnitBuffs}, \"PlayerBuffs\": {PlayerBuffs}, \"RangeWeapon\": {RangeWeapon}");
            JSONObject playerJSON = new("");
            foreach (var key in jsonDic.Keys)
            {
                playerJSON.AddField(key, jsonDic[key]);
            }
            return playerJSON;
        }
        private JSONObject UnitStatsToJson(UnitStatsStruct unitStats)
        {
            return new($"{{\r\n      \"HP\": {unitStats.HP},\r\n      \"MaxHP\": {unitStats.MaxHP},\r\n      \"MoveSpeed\": {unitStats.MoveSpeed}\r\n    }}");
        }
        private JSONObject UnitBuffsToJson(UnitBuffsStruct unitBuffs)
        {
            return new($"{{\r\n      \"MeleeWeaponDamageBuff\": {unitBuffs.MeleeWeaponDamageBuff},\r\n      \"MeleeAttackCDBuff\": {unitBuffs.MeleeAttackCDBuff}\r\n    }}");
        }
        private JSONObject PlayerBuffsToJson(PlayerBuffsStruct playerBuffs)
        {
            return new($"{{\r\n      \"PickableWeaponDamageBuff\": {playerBuffs.PickableWeaponDamageBuff},\r\n      \"SlomoDurationBuff\": {playerBuffs.SlomoDurationBuff}\r\n    }}, \"DefaultPlayerWeaponDamageBuff\": {playerBuffs.DefaultPlayerWeaponDamageBuff}}}");
        }
        private JSONObject RangeWeaponToJson(RangeWeapon rweapon)
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
                PlayerUnitStats = LoadStats<UnitStatsStruct>(playerJSON, "UnitStats");
                PlayerUnitBuffs = LoadStats<UnitBuffsStruct>(playerJSON, "UnitBuffs");
                PlayerBuffs = LoadStats<PlayerBuffsStruct>(playerJSON, "PlayerBuffs");
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
        private T LoadStats<T>(JSONObject unitJsonObj, string fieldName)
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


        public string LoadResourceTextfile(string path)
        {

            string filePath = path.Replace(".json", "");

            TextAsset targetFile = Resources.Load<TextAsset>(filePath);

            return targetFile.text;
        }

        public void NewGameStart()
        {
            SaveData.Clear();
            File.WriteAllText(_saveDataPath, string.Empty);
        }
    }
}


