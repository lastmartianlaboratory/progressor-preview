using System;
using System.IO;
using UnityEngine;
using UXT;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Progressor
{
    public static class App
    {
        public static EditorEnv Env  { get; private set; } = new ();
        public static DataSets  Data { get; private set; } = null;

        public const string EnvFileLocation  = "Assets/LocalDev/EditorEnv.json";
        public const string DataSetsLocation = "Data"; // Relative to `Resources`

        private static bool Initialized = false;

        // Initialization

        public static void Initialize ()
        {
            if (Initialized)
            {
                // NOTE:
                // One of the reasons this can happen is disabled domain reloading

                Log.Dev("App is already initialized");
                return;
            }
            else
            {
                Initialized = true;
            }

            // Logging

            Log.Initialize(false);

            // Launch environment

            #if UNITY_EDITOR
            LoadEnvFile();  
            #endif

            // Resources

            Data = LoadDataSets();

            // App-level setup

            Application.targetFrameRate = Env.FrameRate;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            // ....

            Log.Dev("<i>App initialized</i>");
        }

        // Data

        public static DataSets LoadDataSets ()
        {
            //~ clock this and issue a warning if takes too long

            var data = Resources.Load<DataSets>(DataSetsLocation);
            Debug.Assert(data, $"Root data table cannot be found at <i>Resource/{DataSetsLocation}</i>");
            data.Initialize();
            return data;
        }

        // Launch Environment

        public static bool LoadEnvFile ()
        {
            /// Set environment values from the JSON file

            try
            {
                string jsonData = File.ReadAllText(EnvFileLocation);
                Env = JsonUtility.FromJson<EditorEnv>(jsonData);
                return true;
            }
            catch (Exception e)
            {
                Log.Warn($"Failed to load environment file. Default values will be used <i>[{e.Message}]</i>");
                return false;
            }
        }

        public static bool WriteDefaultEnvFile ()
        {
            try
            {
                string jsonData = JsonUtility.ToJson(new EditorEnv(), true);
                File.WriteAllText(EnvFileLocation, jsonData);
                return true;
            }
            catch (Exception e)
            {
                Log.Warn($"Failed to write environment file <i>[{e.Message}]</i>");
                return false;
            }
        }

        // Editor

        #if UNITY_EDITOR

        [InitializeOnEnterPlayMode]
        private static void OnEnterPlayMode ()
        {
            /// Reset static variables

            Initialized = false;
        }

        #endif
    }
}
