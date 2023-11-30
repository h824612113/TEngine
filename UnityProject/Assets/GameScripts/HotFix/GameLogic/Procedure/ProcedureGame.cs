

using Cysharp.Threading.Tasks;
using TEngine;
using UnityEngine.SceneManagement;

namespace GameLogic
{
    public class ProcedureGame:ProcedureBase
    {
        
        
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            GameModule.Scene.LoadScene(FlappySceneConfig.GameSceneName,LoadSceneMode.Single,false,100, (handle) =>
            {
                
                GameModule.UI.ShowUI<ScoreForm>();
                Utility.Unity.StartCoroutine(GameSystem.Instance.LoadRoom());
            });
        }

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (FlappyModel.Instance.isBackToMenu)
            {
                ChangeState<ProcedureMenu>(procedureOwner);
            }
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }
        
        
    }
}