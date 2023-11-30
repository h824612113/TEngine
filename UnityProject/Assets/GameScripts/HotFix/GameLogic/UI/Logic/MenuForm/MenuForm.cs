using UnityEngine;
using UnityEngine.UI;
using TEngine;
using UnityEditor.Callbacks;
using AudioType = TEngine.AudioType;

namespace GameLogic
{
	partial class MenuForm
	{
		private ProcedureMenu m_ProcedureMenu = null;
		#region UIBind事件


		private void OnClickStartBtn()
		{
			Debug.Log("点击开始按钮");
			m_ProcedureMenu.StartGame();
		}
		private void OnClickSettingBtn()
		{
			Debug.Log("点击设置按钮");
			GameModule.UI.ShowUI<SettingForm>();
		}

		#endregion

		#region 生命周期事件
		public override void OnCreate()
		{
			base.OnCreate();
			Debug.Log("进入MenuForm的OnOnCreate");
			GameModule.Audio.Play(AudioType.Music,FlappySoundMusicConfig.MENU, true);
			m_ProcedureMenu = (ProcedureMenu)this.userDatas[0];
			FlappyModel.Instance.isBackToMenu = false;
		}
		public override void OnRefresh()
		{
			base.OnRefresh();
			Debug.Log("进入MenuForm的OnRefresh");
		}
		public override void OnDestroy()
		{
			base.OnDestroy();
			Debug.Log("进入MenuForm的OnDestroy");
			GameModule.Audio.Stop(AudioType.Music, true);
		}
		#endregion

		public override void OnUpdate()
		{
			base.OnUpdate();
			
			
		}
	}
}
