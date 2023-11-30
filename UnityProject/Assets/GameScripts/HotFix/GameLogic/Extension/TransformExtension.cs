using UnityEngine;

namespace GameLogic
{
    public static class TransformExtension
    {
        /// <summary>
        /// 设置当前Y轴的位置
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="positionY"></param>
        public static void SetLocalPositionY(this Transform transform,float positionY)
        {
            Vector3 pos = transform.localPosition;
            pos = new Vector3(pos.x, positionY, pos.z);
            transform.SetLocalPositionAndRotation(pos,Quaternion.identity);
        }
    }
}