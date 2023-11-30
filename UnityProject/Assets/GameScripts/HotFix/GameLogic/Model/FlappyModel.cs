using GameBase;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class FlappyModel:Singleton<FlappyModel>
    {
        private int totalBgCount = 0;//当前的bg预制总数

        public int TotalBgCount
        {
            set
            {
                totalBgCount = value;
                Debug.Log("当前的bg总数是------------------"+totalBgCount);
            }
            get
            {
                return totalBgCount;
            }
        }

        //Bg的移动速度
        public float MoveSpeed = 10f;
        
        //通过管道得分
        private float totalscore;
        public float TotalScore
        {
            get
            {
                return totalscore;
            }
            set
            {
                totalscore = value;
                Debug.Log("当前totalscore----------"+totalscore);
                GameEvent.Send(FlappyEventDefine.ScoreChange);
            }
        }

        public bool isBackToMenu = false;
    }
}