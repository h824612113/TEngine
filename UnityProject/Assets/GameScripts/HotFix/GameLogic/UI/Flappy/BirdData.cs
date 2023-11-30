using UnityEngine;

namespace GameLogic
{
    public class BirdData:BaseData
    {
        /// <summary>
        /// 飞行力度
        /// </summary>
        public float FlyForce
        {
            get;
            private set;
        }


        public BirdData(float flyForce)
        {
            this.FlyForce = flyForce;
        }
    }
}