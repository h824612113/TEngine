using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace TEngine.Editor.UI
{
    public class ScriptGenerator
    {
        public static string UIGenerateGlobalSettings = "Assets/TEngine/ResRaw/UIGenerateGlobalSettings.asset";
        private const string Gap = "/";

        [MenuItem("GameObject/ScriptGenerator/GenerateHotUI", priority = 41)]
        public static void GenerateHotUI()
        {

            GenerateHot();
        }
        [MenuItem("GameObject/ScriptGenerator/GenerateMainUI", priority = 41)]
        public static void GenerateMainUI()
        {

            GenerateMain();
        }

        [MenuItem("GameObject/ScriptGenerator/UIGlobalSettings", priority = 41)]
        public static void SelectedGlobalSettings()
        {
            UIGenerateGlobalSettings globalSettings = null;

            globalSettings = AssetDatabase.LoadAssetAtPath<UIGenerateGlobalSettings>(UIGenerateGlobalSettings);

            if (globalSettings == null)
            {
                globalSettings = ScriptableObject.CreateInstance<UIGenerateGlobalSettings>();
                AssetDatabase.CreateAsset(globalSettings, UIGenerateGlobalSettings);
            }
            Selection.activeObject = globalSettings;
        }


        private static void GenerateMain()
        {
            var root = Selection.activeTransform;
            StringBuilder bindRef = new StringBuilder();
            StringBuilder logicRef = new StringBuilder();

            //检查是否存在旧的逻辑面板 在的话读数据用追加方式改
            UIGenerateGlobalSettings globalSettings = AssetDatabase.LoadAssetAtPath<UIGenerateGlobalSettings>(UIGenerateGlobalSettings);
            var logicMainPath = Path.Combine(globalSettings.UIMainLogicSavePath, root.name, root.name + ".cs");
            if (File.Exists(logicMainPath))
            {
                logicRef.Append(File.ReadAllText(logicMainPath));
            }
            CheckUIItems(root, ref bindRef, ref logicRef);
            var UIElement = root.GetComponent<UIElement>();

            foreach (var bindData in UIElement.Elements)
            {
                UIElement.bindComponents.Add(bindData.BindCom);
            }
            UIGeneratorWindow.ShowWindow(bindRef, logicRef, root);
        }
        private static void GenerateHot()
        {
            var root = Selection.activeTransform;
            StringBuilder bindRef = new StringBuilder();
            StringBuilder logicRef = new StringBuilder();

            //检查是否存在旧的逻辑面板 在的话读数据用追加方式改
            UIGenerateGlobalSettings globalSettings = AssetDatabase.LoadAssetAtPath<UIGenerateGlobalSettings>(UIGenerateGlobalSettings);
            var logicPath = Path.Combine(globalSettings.UILogicSavePath, root.name, root.name + ".cs");
            if (File.Exists(logicPath))
            {
                logicRef.Append(File.ReadAllText(logicPath));
            }
            CheckUIItems(root, ref bindRef, ref logicRef);
            var UIElement = root.GetComponent<UIElement>();

            foreach (var bindData in UIElement.Elements)
            {
                UIElement.bindComponents.Add(bindData.BindCom);
            }
            UIGeneratorWindow.ShowWindow(bindRef, logicRef, root);
        }

        private static void CheckUIItems(Transform root, ref StringBuilder bindRef, ref StringBuilder logicRef)
        {

            ClearUnusedItems(root);
            if (root != null)
            {
                StringBuilder strVar = new StringBuilder();
                StringBuilder strBind = new StringBuilder();
                StringBuilder strOnCreate = new StringBuilder();
                StringBuilder strCallback = new StringBuilder();
                StringBuilder strLogic = new StringBuilder();
                Ergodic(root, root, ref strVar, ref strBind, ref strOnCreate, ref strCallback, logicRef.ToString());

                StringBuilder strFile = new StringBuilder();

                //BindComponents
                {

                    strFile.Append("using TMPro;\n");
                    strFile.Append("using UnityEngine;\n");
                    strFile.Append("using UnityEngine.UI;\n");
                    strFile.Append("using TEngine;\n\n");
                    strFile.Append($"namespace {SettingsUtils.GetUINameSpace()}\n");
                    strFile.Append("{\n");
                    strFile.Append("\t [Window(#UILayer#, fromResources: #FromResources#, location: \"#Location#\",fullScreen:#FullScreen#)]\n");
                    strFile.Append("\tpartial class " + root.name + " : UIWindow\n");
                    strFile.Append("\t{\n");

                    // 脚本工具生成的代码
                    strFile.Append("\t\t#region 脚本工具生成的代码\n");
                    strFile.Append(strVar);
                    strFile.Append("\t\tpublic override void ScriptGenerator()\n");
                    strFile.Append("\t\t{\n");
                    strFile.Append("\t\t\tCheckUIElement();\n");
                    strFile.Append(strBind);
                    strFile.Append(strOnCreate);
                    strFile.Append("\t\t}\n");
                    strFile.Append("\t\t#endregion");
                    strFile.Append("\n\t}\n");
                    strFile.Append("}\n");
                    bindRef.Append(strFile.ToString());
                }

                var hasLogic = logicRef.Length > 0;
                //Logic  后面再改吧 临时这么改乱的大便一样
                if (!hasLogic)
                {
                    strLogic.Append("using UnityEngine;\n");
                    strLogic.Append("using UnityEngine.UI;\n");
                    strLogic.Append("using TEngine;\n\n");
                    strLogic.Append($"namespace {SettingsUtils.GetUINameSpace()}\n");
                    strLogic.Append("{\n");
                    strLogic.Append("\tpartial class " + root.name + " \n");
                    strLogic.Append("\t{\n");

                    strLogic.Append("\n");
                    strLogic.Append("\t\t#region UIBind事件\n");
                    strLogic.Append(strCallback);
                    strLogic.Append("\n");
                    strLogic.Append("\t\t#endregion\n\n");

                    strLogic.Append("\t\t#region 生命周期事件\n");

                    strLogic.Append($"\t\tpublic override void OnCreate()\n");
                    strLogic.Append("\t\t{\n");
                    strLogic.Append("\t\t\tbase.OnCreate();\n");
                    strLogic.Append("\t\t}\n");

                    strLogic.Append($"\t\tpublic override void OnRefresh()\n");
                    strLogic.Append("\t\t{\n");
                    strLogic.Append("\t\t\tbase.OnRefresh();\n");
                    strLogic.Append("\t\t}\n");

                    strLogic.Append($"\t\tpublic override void OnDestroy()\n");
                    strLogic.Append("\t\t{\n");
                    strLogic.Append("\t\t\tbase.OnDestroy();\n");
                    strLogic.Append("\t\t}\n");

                    strLogic.Append("\t\t#endregion\n\n");

                    strLogic.Append("\n\t}\n");
                    strLogic.Append("}\n");

                    logicRef.Append(strLogic.ToString());
                }
                else
                {
                    int index = logicRef.ToString().IndexOf("UIBind事件");
                    if (index != -1)
                    {
                        logicRef.Insert(index + 8, "\n" + strCallback);
                    }
                    else
                    {
                        Debug.LogError("请检查该脚本自动生成关键字#region UIBind事件是否被去除!");
                    }
                }
            }
        }

        private static void ClearUnusedItems(Transform root)
        {
            var Element = root.gameObject.GetOrAddComponent<UIElement>();
            if (Element == null || Element.Elements == null) return;
            Element.Elements.Clear();
            Element.bindComponents.Clear();
        }

        private static void Ergodic(Transform root, Transform transform, ref StringBuilder strVar, ref StringBuilder strBind, ref StringBuilder strOnCreate,
            ref StringBuilder strCallback, string logicRef)
        {
            for (int i = 0; i < transform.childCount; ++i)
            {
                Transform child = transform.GetChild(i);
                WriteScript(root, child, ref strVar, ref strBind, ref strOnCreate, ref strCallback, logicRef);
                if (child.name.StartsWith("m_item"))
                {
                    // 子 Item 不再往下遍历
                    continue;
                }

                Ergodic(root, child, ref strVar, ref strBind, ref strOnCreate, ref strCallback, logicRef);
            }
        }


        public static string GetBtnFuncName(string varName)
        {
            return "OnClick" + varName.Replace("m_btn", string.Empty) + "Btn";
        }

        public static string GetToggleFuncName(string varName)
        {
            return "OnToggle" + varName.Replace("m_toggle", string.Empty) + "Change";
        }

        public static string GetSliderFuncName(string varName)
        {
            return "OnSlider" + varName.Replace("m_slider", string.Empty) + "Change";
        }

        private static string GetVerType(string uiName)
        {
            foreach (var pair in SettingsUtils.GetScriptGenerateRule())
            {
                if (uiName.StartsWith(pair.uiElementRegex))
                {
                    return pair.componentName;
                }
            }

            return string.Empty;
        }

        private static void WriteScript(Transform root, Transform child, ref StringBuilder strVar, ref StringBuilder strBind, ref StringBuilder strOnCreate,
            ref StringBuilder strCallback, string logicRef)
        {
            var varName = child.name;
            var varType = GetVerType(varName);
            var Element = root.gameObject.GetOrAddComponent<UIElement>();
            if (varType == string.Empty) return;
            if (Element.Elements.Find(a => a.Name == varName) != null)
            {
                Debug.LogError("有重复的key:" + varName);
                return;
            }
            var component = child.GetComponent(varType);
            if (component == null)
            {
                Debug.LogError($"{child.name}上不存在{varType}的组件");
                return;
            }
            Element.Elements.Add(new UIElement.BindData(varName, component));
            strVar.Append("\t\tprivate " + varType + " " + varName + ";\n");

            switch (varType)
            {
                case "GameObject":
                    strBind.Append($"\t\t\t{varName} = FChild(\"{varName}\").gameObject;\n");
                    break;
                case "RichItemIcon":
                    strBind.Append($"\t\t\t{varName} = CreateWidgetByType<{varType}>(FChild(\"{varName}\"));\n");
                    break;
                case "RedNoteWidget":
                    break;
                case "TextButtonItem":
                case "SwitchTabItem":
                case "UIActorWidget":
                case "UIEffectWidget":
                case "UISpineWidget":
                case "UIMainPlayerWidget":
                    strBind.Append($"\t\t\t{varName} = CreateWidget<{varType}>(FChild(\"{varName}\").gameObject);\n");
                    break;
                default:
                    strBind.Append($"\t\t\t{varName} = FChild<{varType}>({Element.Elements.Count - 1});\n");
                    break;
            }

            if (varType == "Button")
            {
                string varFuncName = GetBtnFuncName(varName);
                strOnCreate.Append($"\t\t\t{varName}.onClick.AddListener({varFuncName});\n");
                if (!logicRef.Contains(varFuncName))
                {
                    strCallback.Append($"\t\tprivate void {varFuncName}()\n");
                    strCallback.Append("\t\t{\n\t\t}\n");
                }

            }
            else if (varType == "Toggle")
            {
                string varFuncName = GetToggleFuncName(varName);
                strOnCreate.Append($"\t\t\t{varName}.onValueChanged.AddListener({varFuncName});\n");
                if (!logicRef.Contains(varFuncName))
                {
                    strCallback.Append($"\t\tprivate void {varFuncName}(bool isOn)\n");
                    strCallback.Append("\t\t{\n\t\t}\n");
                }

            }
            else if (varType == "Slider")
            {
                string varFuncName = GetSliderFuncName(varName);
                strOnCreate.Append($"\t\t\t{varName}.onValueChanged.AddListener({varFuncName});\n");
                if (!logicRef.Contains(varFuncName))
                {
                    strCallback.Append($"\t\tprivate void {varFuncName}(float value)\n");
                    strCallback.Append("\t\t{\n\t\t}\n");
                }
            }
        }

        public class GeneratorHelper : EditorWindow
        {
            [MenuItem("GameObject/ScriptGenerator/About", priority = 100)]
            public static void About()
            {
                GeneratorHelper welcomeWindow = (GeneratorHelper)EditorWindow.GetWindow(typeof(GeneratorHelper), false, "About");
            }

            public void Awake()
            {
                minSize = new Vector2(400, 600);
            }

            protected void OnGUI()
            {
                GUILayout.BeginVertical();
                foreach (var item in SettingsUtils.GetScriptGenerateRule())
                {
                    GUILayout.Label(item.uiElementRegex + "：\t" + item.componentName);
                }

                GUILayout.EndVertical();
            }

        }

        public class SwitchGroupGenerator
        {
            private const string Condition = "m_switchGroup";

            public static readonly SwitchGroupGenerator Instance = new SwitchGroupGenerator();

            public string Process(Transform root)
            {
                var sbd = new StringBuilder();
                var list = new List<Transform>();
                Collect(root, list);
                foreach (var node in list)
                {
                    sbd.AppendLine(Process(root, node)).AppendLine();
                }

                return sbd.ToString();
            }

            public void Collect(Transform node, List<Transform> nodeList)
            {
                if (node.name.StartsWith(Condition))
                {
                    nodeList.Add(node);
                    return;
                }

                var childCnt = node.childCount;
                for (var i = 0; i < childCnt; i++)
                {
                    var child = node.GetChild(i);
                    Collect(child, nodeList);
                }
            }

            private string Process(Transform root, Transform groupTf)
            {
                var parentPath = GetPath(root, groupTf);
                var name = groupTf.name;
                var sbd = new StringBuilder(@"
var _namePath = ""#parentPath"";
var _nameTf = FindChild(_namePath);
var childCnt = _nameTf.childCount;
SwitchTabItem[] _name;
_name = new SwitchTabItem[childCnt];
for (var i = 0; i < childCnt; i++)
{
    var child = _nameTf.GetChild(i);
    _name[i] = CreateWidget<SwitchTabItem>(_namePath + ""/"" + child.name);
}");
                sbd.Replace("_name", name);
                sbd.Replace("#parentPath", parentPath);
                return sbd.ToString();
            }

            public string GetPath(Transform root, Transform childTf)
            {
                if (childTf == null)
                {
                    return string.Empty;
                }

                if (childTf == root)
                {
                    return childTf.name;
                }

                var parentPath = GetPath(root, childTf.parent);
                if (parentPath == string.Empty)
                {
                    return childTf.name;
                }

                return parentPath + "/" + childTf.name;
            }
        }
    }
}