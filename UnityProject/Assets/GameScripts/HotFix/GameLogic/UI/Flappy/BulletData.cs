using UnityEngine;

namespace GameLogic
{
    public class BulletData:BaseData
    {
        /// <summary>
        /// 发射位置
        /// </summary>
        public Vector2 ShootPosition
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 飞行速度
        /// </summary>
        public float FlySpeed
        {
            get;
            private set;
        }

        /// <summary>
        /// 子弹消失距离
        /// </summary>
        public float HideDistance
        {
            get;
            private set;
        }


        public BulletData(Vector2 shootPosition, float flySpeed,float hideDistance)
        {
            this.ShootPosition = shootPosition;
            this.FlySpeed = flySpeed;
            this.HideDistance = hideDistance;
        }
    }
}