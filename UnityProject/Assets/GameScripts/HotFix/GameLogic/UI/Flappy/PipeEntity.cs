using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class PipeEntity : BaseEntity
    {

        /// <summary>
        /// 上管道
        /// </summary>
        private Transform m_UpPipe = null;

        /// <summary>
        /// 下管道
        /// </summary>
        private Transform m_DownPipe = null;
        
        private PipeData _pipeData;
        private bool isSpawn = false;
        public float MoveSpeed = 2f;

        public override void InitEntity(SpawnHandle handle)
        {
            base.InitEntity(handle);
            _pipeData = _handle.UserDatas[0] as PipeData;
            if (m_UpPipe == null || m_DownPipe == null)
            {
                m_UpPipe = transform.Find("UpPipe");
                m_DownPipe = transform.Find("DownPipe");
            }

            isSpawn = false;
            //设置Offset
            m_UpPipe.SetLocalPositionY(_pipeData.OffsetUp);
            m_DownPipe.SetLocalPositionY(_pipeData.OffsetDown);
        }

        // Update is called once per frame
        void Update()
        {
            this.gameObject.transform.Translate(Vector3.left*MoveSpeed*Time.deltaTime,Space.World);
            if (transform.position.x < _pipeData.HideTarget && !isSpawn)
            {
                isSpawn = true;
                Destroy();
                //创建新的pipe
                GameEvent.Send(FlappyEventDefine.FlappyPipeCreate);
            }
        }
    }
}
