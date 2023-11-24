using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
	 [Window(UILayer.UI, fromResources: false, location: "ScoreForm",fullScreen:true)]
	partial class ScoreForm : UIWindow
	{
		#region 脚本工具生成的代码
		private Text m_textTotalScore;
		public override void ScriptGenerator()
		{
			CheckUIElement();
			m_textTotalScore = FChild<Text>(0);
		}
		#endregion
	}
}
