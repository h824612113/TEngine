using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
	 [Window(UILayer.UI, fromResources: false, location: "MenuForm",fullScreen:true)]
	partial class MenuForm : UIWindow
	{
		#region 脚本工具生成的代码
		private Button m_btnStart;
		private Button m_btnSetting;
		public override void ScriptGenerator()
		{
			CheckUIElement();
			m_btnStart = FChild<Button>(0);
			m_btnSetting = FChild<Button>(1);
			m_btnStart.onClick.AddListener(OnClickStartBtn);
			m_btnSetting.onClick.AddListener(OnClickSettingBtn);
		}
		#endregion
	}
}
