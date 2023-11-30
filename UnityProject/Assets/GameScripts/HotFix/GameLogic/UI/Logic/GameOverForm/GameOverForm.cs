using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TEngine;
using UnityEngine.SceneManagement;

namespace GameLogic
{
	partial class GameOverForm 
	{

		#region UIBind事件

		private void OnClickRestartBtn()
		{
            GameModule.Scene.LoadScene(FlappySceneConfig.GameSceneName,LoadSceneMode.Single,false,100, (handle) =>
            {
	            Debug.Log("重玩");
	            GameSystem.Instance.DestroyRoom();
	            Utility.Unity.StartCoroutine(GameSystem.Instance.LoadRoom()); 
            });
		}
		private void OnClickMainBtn()
		{
			GameSystem.Instance.DestroyRoom();
			FlappyModel.Instance.isBackToMenu = true;
			Debug.Log("回到菜单界面");
		}

		#endregion

		#region 生命周期事件
		public override void OnCreate()
		{
			base.OnCreate();
			m_tmpUScore.text = FlappyModel.Instance.TotalScore.ToString();
		}
		public override void OnRefresh()
		{
			base.OnRefresh();
		}
		public override void OnDestroy()
		{
			base.OnDestroy();
		}
		#endregion


	}
}
