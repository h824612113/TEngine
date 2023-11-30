namespace GameLogic
{
    public class PipeData:BaseData
    {
        /// <summary>
        /// 移动速度
        /// </summary>
        public float MoveSpeed { get; private set; }

        /// <summary>
        /// 上管道偏移
        /// </summary>
        public float OffsetUp
        {
            get;
            private set;
        }

        /// <summary>
        /// 下管道偏移
        /// </summary>
        public float OffsetDown
        {
            get;
            private set;
        }
        
 
        /// <summary>
        /// 到达此目标时隐藏自身
        /// </summary>
        public float HideTarget { get; set; }
        


        public PipeData(float moveSpeed, float offsetUp,float offsetDown, float hideTarget = -9.4f)
        {
            this.MoveSpeed = moveSpeed;
            this.OffsetUp = offsetUp;
            this.OffsetDown = offsetDown;
            this.HideTarget = hideTarget;
        }
    }
}