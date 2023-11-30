

namespace GameLogic
{
    public class BgData:BaseData
    {
        /// <summary>
        /// 移动速度
        /// </summary>
        public float MoveSpeed { get; private set; }
 
        /// <summary>
        /// 到达此目标时产生新的背景实体
        /// </summary>
        public float SpawnTarget { get; set; }
 
        /// <summary>
        /// 到达此目标时隐藏自身
        /// </summary>
        public float HideTarget { get; set; }
        


        public BgData(float moveSpeed, float spawnTarget, float hideTarget)
        {
            this.MoveSpeed = moveSpeed;
            this.SpawnTarget = spawnTarget;
            this.HideTarget = hideTarget;
        }
    }
}