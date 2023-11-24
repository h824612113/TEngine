using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    [CreateAssetMenu(fileName = "UIGenerateGlobalSettings", menuName = "TEngine/UIGenerateGlobalSettings")]
    public class UIGenerateGlobalSettings : ScriptableObject
    {
        [Sirenix.OdinInspector.FolderPath]
        public string BindComponetsSavePath;

        [Sirenix.OdinInspector.FolderPath]
        public string UILogicSavePath;

        [Sirenix.OdinInspector.FolderPath]
        public string UIPrefabSavePath;
        
        [Sirenix.OdinInspector.FolderPath]
        public string BindMainComponetsSavePath;

        [Sirenix.OdinInspector.FolderPath]
        public string UIMainLogicSavePath;

        [Sirenix.OdinInspector.FolderPath]
        public string UIMainPrefabSavePath;
    }
}