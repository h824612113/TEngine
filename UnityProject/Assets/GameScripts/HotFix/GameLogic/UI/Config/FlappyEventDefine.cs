using TEngine;

namespace GameLogic
{
    public static class FlappyEventDefine
    {
        public static readonly int ScoreChange = RuntimeId.ToRuntimeId("FlappyEventDefine.ScoreChange");
        public static readonly int FlappyGameOver = RuntimeId.ToRuntimeId("FlappyEventDefine.GameOver");
        public static readonly int PlayerFireBullet = RuntimeId.ToRuntimeId("FlappyEventDefine.PlayerFireBullet");
        
        public static readonly int FlappyBgCreate = RuntimeId.ToRuntimeId("FlappyEventDefine.FlappyBgCreate");
        
        public static readonly int FlappyPipeCreate = RuntimeId.ToRuntimeId("FlappyEventDefine.FlappyPipeCreate");
    }
}