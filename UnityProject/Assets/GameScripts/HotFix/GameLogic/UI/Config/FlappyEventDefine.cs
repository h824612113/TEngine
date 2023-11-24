using TEngine;

namespace GameLogic
{
    public static class FlappyEventDefine
    {
        public static readonly int ScoreChange = RuntimeId.ToRuntimeId("FlappyEventDefine.ScoreChange");
        public static readonly int GameOver = RuntimeId.ToRuntimeId("FlappyEventDefine.GameOver");
        public static readonly int PlayerFireBullet = RuntimeId.ToRuntimeId("FlappyEventDefine.PlayerFireBullet");
    }
}