using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
	 [Window(UILayer.UI, fromResources: false, location: "GameOverForm",fullScreen:true)]
	partial class GameOverForm : UIWindow
	{
		#region 脚本工具生成的代码
		private TextMeshProUGUI m_tmpUScore;
		private Button m_btnRestart;
		private Button m_btnMain;
		public override void ScriptGenerator()
		{
			CheckUIElement();
			m_tmpUScore = FChild<TextMeshProUGUI>(0);
			m_btnRestart = FChild<Button>(1);
			m_btnMain = FChild<Button>(2);
			m_btnRestart.onClick.AddListener(OnClickRestartBtn);
			m_btnMain.onClick.AddListener(OnClickMainBtn);
		}
		#endregion
	}
}
