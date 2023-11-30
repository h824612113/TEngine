using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
	partial class ScoreForm 
	{

		#region UIBind事件


		#endregion

		#region 生命周期事件
		public override void OnCreate()
		{
			base.OnCreate();
			m_textTotalScore.text = FlappyModel.Instance.TotalScore.ToString();

			GameEvent.AddEventListener(FlappyEventDefine.ScoreChange, OnAddScore);
		}

		private void OnAddScore()
		{
			m_textTotalScore.text = FlappyModel.Instance.TotalScore.ToString();
		}
		public override void OnRefresh()
		{
			base.OnRefresh();
		}
		public override void OnDestroy()
		{
			base.OnDestroy();
			GameEvent.RemoveEventListener(FlappyEventDefine.ScoreChange, OnAddScore);
		}
		#endregion


	}
}
