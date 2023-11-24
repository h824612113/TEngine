using System;
using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    /// <summary>
    /// UI元素节点。
    /// <remarks>通过mono序列化来绑定ui节点的元素换取查找与ui的稳定性。</remarks>
    /// </summary>
    public class UIElement : MonoBehaviour
    {
#if UNITY_EDITOR
        [Serializable]
        public class BindData
        {
            public BindData()
            {
            }

            public BindData(string name, Component bindCom)
            {
                Name = name;
                BindCom = bindCom;

            }

            public string Name;
            public Component BindCom;
        }
        [HideInInspector]
        public List<BindData> Elements = new List<BindData>();

#endif

        public List<Component> bindComponents = new List<Component>();

        public T GetBindComponent<T>(int index) where T : Component
        {
            if (index >= bindComponents.Count)
            {
                Debug.LogError("索引无效");
                return null;
            }

            T bindCom = bindComponents[index] as T;

            if (bindCom == null)
            {
                Debug.LogError("类型无效");
                return null;
            }

            return bindCom;
        }

        public void OnDestroy()
        {
            bindComponents.Clear();
            bindComponents = null;
        }

    }
}