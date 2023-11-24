using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
	 [Window(UILayer.UI, fromResources: false, location: "SettingForm",fullScreen:true)]
	partial class SettingForm : UIWindow
	{
		#region 脚本工具生成的代码
		private Button m_btnClose;
		private Slider m_sliderMusic;
		private Slider m_sliderSound;
		public override void ScriptGenerator()
		{
			CheckUIElement();
			m_btnClose = FChild<Button>(0);
			m_sliderMusic = FChild<Slider>(1);
			m_sliderSound = FChild<Slider>(2);
			m_btnClose.onClick.AddListener(OnClickCloseBtn);
			m_sliderMusic.onValueChanged.AddListener(OnSliderMusicChange);
			m_sliderSound.onValueChanged.AddListener(OnSliderSoundChange);
		}
		#endregion
	}
}
