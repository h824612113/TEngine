using System.Collections.Generic;
using System.IO;
using System.Text;
using Sirenix.OdinInspector.Editor;
using TEngine.Editor.UI;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace TEngine
{

    public class UIGeneratorWindow : OdinEditorWindow
    {
        private Vector2 bindScrollPosition = Vector2.zero;
        private Vector2 logicScrollPosition = Vector2.zero;
        protected Transform root;
        private StringBuilder bindRef;
        private StringBuilder logicRef;
        private UIGenerateGlobalSettings uIGenerateGlobalSettings;

        UILayer uILayer;
        bool fromResources;
        string location;
        bool fullScreen;
        bool SavePrefab;
        bool isMain;

        public static void ShowWindow(StringBuilder bindRef, StringBuilder logicRef, Transform root)
        {
            var Window = GetWindow<UIGeneratorWindow>();
            Window.position = new Rect(Screen.width / 2 - 200, Screen.height / 2 - 150, 400, 300);
            Window.bindRef = bindRef;
            Window.logicRef = logicRef;
            Window.minSize = new Vector2(900, 700);
            Window.maxSize = new Vector2(900, 700);
            Window.root = root;
            Window.location = root.name;
            Window.uILayer = UILayer.UI;

            Window.Show();

        }
        protected override void OnEnable()
        {
            base.OnEnable();
            LoadSettingCheck();
        }



        private void LoadSettingCheck()
        {
            uIGenerateGlobalSettings = AssetDatabase.LoadAssetAtPath<UIGenerateGlobalSettings>(ScriptGenerator.UIGenerateGlobalSettings);
            if (uIGenerateGlobalSettings == null)
            {
                uIGenerateGlobalSettings = ScriptableObject.CreateInstance<UIGenerateGlobalSettings>();
                AssetDatabase.CreateAsset(uIGenerateGlobalSettings, ScriptGenerator.UIGenerateGlobalSettings);
            }
        }

        protected override void OnGUI()
        {
            if (root == null || bindRef == null || logicRef == null)
            {
                GetWindow<UIGeneratorWindow>().Close();
                return;
            }
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("UIName", root.name);
            EditorGUI.EndDisabledGroup();
            uILayer = (UILayer)EditorGUILayout.EnumPopup("UILayer", uILayer);
            fromResources = EditorGUILayout.Toggle("FromResources", fromResources);
            location = EditorGUILayout.TextField("location", location);
            fullScreen = EditorGUILayout.Toggle("fullScreen", fullScreen);
            isMain = EditorGUILayout.Toggle("isMain", isMain);
            SavePrefab = EditorGUILayout.Toggle("SavePrefab", SavePrefab);
            EditorGUILayout.BeginHorizontal();

            using (new EditorGUILayout.VerticalScope("box", GUILayout.MinWidth(425), GUILayout.MinWidth(425)))
            {
                bindScrollPosition = EditorGUILayout.BeginScrollView(bindScrollPosition);
                var bindStr = GetUIBindProperty();
                GUILayout.TextField(bindStr, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndScrollView();
            }

            using (new EditorGUILayout.VerticalScope("box", GUILayout.MinWidth(425), GUILayout.MinWidth(425)))
            {
                logicScrollPosition = EditorGUILayout.BeginScrollView(logicScrollPosition);
                var logicStr = GetUILogicProperty();
                GUILayout.TextField(logicStr, GUILayout.ExpandWidth(true));
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Generate"))
            {
                GenerateFile();
            }
            base.OnGUI();

        }

        private string GetUILogicProperty()
        {
            var logicStr = logicRef.ToString();
            if (isMain)
            {
                logicStr = logicStr.Replace("namespace GameLogic", "namespace GameMain");
            }
            else
            {
                logicStr = logicStr.Replace("namespace GameMain", "namespace GameLogic");
            }

            return logicStr;
        }
        private string GetUIBindProperty()
        {
            var bindStr = bindRef.ToString();
            
            
            bindStr = bindStr.Replace("#UILayer#", "UILayer." + uILayer.ToString());
            bindStr = bindStr.Replace("#FromResources#", fromResources.ToString().ToLower());
            bindStr = bindStr.Replace("#Location#", location);
            bindStr = bindStr.Replace("#FullScreen#", fullScreen.ToString().ToLower());
            if (isMain)
            {
                bindStr = bindStr.Replace("namespace GameLogic", "namespace GameMain");
            }
            else
            {
                bindStr = bindStr.Replace("namespace GameMain", "namespace GameLogic");
            }

            return bindStr;
        }

        private void GenerateFile()
        {
            string fileName = root.name;
            string bindFloder = uIGenerateGlobalSettings.BindComponetsSavePath;
            string logicFloder = Path.Combine(uIGenerateGlobalSettings.UILogicSavePath, fileName);
            string prefabFloder = Path.Combine(uIGenerateGlobalSettings.UIPrefabSavePath, fileName);
            string bindPath = "";
            string logicPath = "";
            string prefabPath = "";
            if (isMain)
            {
                bindPath = Path.Combine(uIGenerateGlobalSettings.BindMainComponetsSavePath, fileName + ".BindComponents.cs");
                logicPath = Path.Combine(uIGenerateGlobalSettings.UIMainLogicSavePath, fileName, fileName + ".cs");
                prefabPath = Path.Combine(uIGenerateGlobalSettings.UIMainPrefabSavePath, fileName, fileName + ".prefab");
                bindFloder = uIGenerateGlobalSettings.BindMainComponetsSavePath;
                logicFloder = Path.Combine(uIGenerateGlobalSettings.UIMainLogicSavePath, fileName);
                prefabFloder = Path.Combine(uIGenerateGlobalSettings.UIMainPrefabSavePath, fileName);
            }
            else
            {
                bindPath = Path.Combine(uIGenerateGlobalSettings.BindComponetsSavePath, fileName + ".BindComponents.cs");
                logicPath = Path.Combine(uIGenerateGlobalSettings.UILogicSavePath, fileName, fileName + ".cs");
                prefabPath = Path.Combine(uIGenerateGlobalSettings.UIPrefabSavePath, fileName, fileName + ".prefab");
                bindFloder = uIGenerateGlobalSettings.BindComponetsSavePath;
                logicFloder = Path.Combine(uIGenerateGlobalSettings.UILogicSavePath, fileName);
                prefabFloder = Path.Combine(uIGenerateGlobalSettings.UIPrefabSavePath, fileName);
            }
            if (!Directory.Exists(bindFloder))
            {
                Directory.CreateDirectory(bindFloder);
            }
            if (!Directory.Exists(logicFloder))
            {
                Directory.CreateDirectory(logicFloder);
            }


            var logicStr = GetUILogicProperty();
            var bindStr = GetUIBindProperty();
            
            File.WriteAllText(bindPath, bindStr);
            File.WriteAllText(logicPath, logicStr);

            if (SavePrefab)
            {
                if (!Directory.Exists(prefabFloder))
                {
                    Directory.CreateDirectory(prefabFloder);
                }
                if (File.Exists(prefabPath))
                {
                    File.Delete(prefabPath);
                }
                if (PrefabUtility.SaveAsPrefabAsset(root.gameObject, prefabPath))
                {
                    Debug.Log("generate success");
                }
            }
            Debug.Log("generate success");
            AssetDatabase.Refresh();
        }

    }
}
#endif