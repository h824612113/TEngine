using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class BgEntity : BaseEntity
    {
        
        private BgData _bgData;
        private bool m_IsSpawn = false;
        public float MoveSpeed = 3f;
        private bool isSpawn = false;
        private bool isHide = false;

        public override void InitEntity(SpawnHandle handle)
        {
            base.InitEntity(handle);
            _bgData = this._handle.UserDatas[0] as BgData;
            isSpawn = false;
            isHide = false;
            Debug.Log("运动速度是-------------"+_bgData.MoveSpeed);
        }


        public void Update()
        {
            this.gameObject.transform.Translate(Vector3.left*MoveSpeed*Time.deltaTime,Space.World);
            Debug.Log(gameObject.transform+"===的位置是0--------------"+transform.position);
            if (transform.position.x < _bgData.SpawnTarget && !isSpawn)
            {
                isSpawn = true;
                //生成新的bg
                //
                // transform.position = new Vector3(_bgData.SpawnTarget, 0.5f, transform.position.z);
                GameEvent.Send(FlappyEventDefine.FlappyBgCreate,transform.position,transform.rotation);
                Debug.Log("生成新的背景------------------------");
            }

            if (transform.position.x < _bgData.HideTarget && !isHide)
            {
                isHide = true;
                Destroy();
                FlappyModel.Instance.TotalBgCount -= 1;
                Debug.Log("旧的BG消失-----------------------11111111111111111111111");
            }
        }
    }
}
