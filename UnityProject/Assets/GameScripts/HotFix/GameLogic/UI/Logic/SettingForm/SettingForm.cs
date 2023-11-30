using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
	partial class SettingForm 
	{

		#region UIBind事件

		private void OnClickCloseBtn()
		{
			GameModule.UI.CloseWindow<SettingForm>();
			Debug.Log("关闭设置界面");
		}
		private void OnSliderMusicChange(float value)
		{
			GameModule.Audio.MusicVolume = value;
			Debug.Log("当前的音乐大小是===11111111111111111==="+value);
		}
		private void OnSliderSoundChange(float value)
		{
			GameModule.Audio.SoundVolume = value;
			Debug.Log("当前的音效大小是===22222222222222222==="+value);
		}

		#endregion

		#region 生命周期事件
		public override void OnCreate()
		{
			base.OnCreate();
			Debug.Log("enter-----SettingForm-------OnCreate");
			m_sliderMusic.value = GameModule.Audio.MusicVolume;
			m_sliderSound.value = GameModule.Audio.SoundVolume;
		}
		public override void OnRefresh()
		{
			base.OnRefresh();
			Debug.Log("enter-----SettingForm------OnRefresh");
		}
		public override void OnDestroy()
		{
			Debug.Log("enter-----SettingForm------OnDestroy");
			base.OnDestroy();
		}
		#endregion


	}
}
