

using TEngine;

namespace GameLogic
{
    public class ProcedureMenu:ProcedureBase
    {

        private bool m_StartGame = false;
        
        public void StartGame()
        {
            m_StartGame = true;
        }
        
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            m_StartGame = false;
            GameModule.UI.ShowUI(typeof(MenuForm),this);
        }
        

        protected override void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (m_StartGame)
            {
                ChangeState<ProcedureGame>(procedureOwner);
            }
        }

        protected override void OnLeave(IFsm<IProcedureManager> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

            GameModule.UI.CloseWindow<MenuForm>();
        }
    }
}