using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UXT;

namespace Progressor
{
    public static class Menus
    {
        [MenuItem("▲ Progressor/Main", false, 10)]
        public static void OpenScene_Main ()
        {
            OpenScene("Assets/Scenes/Main.unity");
        }

        [MenuItem("▲ Progressor/Update Version", false, 20)]
        public static void UpdateVersion ()
        {
            // Manually update the version

            var settings = Settings.Load();

            if (settings)
            {
                var version = $"{settings.BaseVersion} ({UXT.Git.GetActiveBranch()}:{UXT.Git.GetCommitId()[..8]})"; //~ version generation code should not be in Menus
                PlayerSettings.bundleVersion = version;
                AssetDatabase.SaveAssets();

                Log.Msg(PlayerSettings.bundleVersion);
            }
            else
            {
                Log.Warn("Failed to load Settings");
            }
        }

        public static void OpenScene (string scenePath) //~ Switch to UXT 
        {
            //? do not open currently active scene

            try
            {
                if ((!EditorSceneManager.GetActiveScene().isDirty) ||
                    EditorUtility.DisplayDialog("Confirm scene switching",
                        "Current scene has been modified.\nSwitching to a different scene will cause changes to be lost.\n\nContinue switching without saving?",
                        "Yes", "No"))
                {
                    EditorSceneManager.OpenScene(scenePath);
                    Log.Dev($"Scene <b>{scenePath}</b> opened");
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"Cannot open scene <b>{scenePath}</b> (<i>{e.Message}</i>)");
            }
        }
    }
}
