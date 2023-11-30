using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;
using AudioType = TEngine.AudioType;

namespace GameLogic
{
    public class BirdEntity : BaseEntity
    {
        private BirdData _birdData;

        private Rigidbody2D m_Rigidbody = null;
        public override void InitEntity(SpawnHandle handle)
        {
            base.InitEntity(handle);
            _birdData = this._handle.UserDatas[0] as BirdData;
            if (m_Rigidbody == null)
            {
                m_Rigidbody = GetComponent<Rigidbody2D>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            //上升控制
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("当前音乐音量是-----------------"+GameModule.Audio.MusicVolume);
                Debug.Log("当前音效音量是-----------------"+GameModule.Audio.SoundVolume);
                // GameModule.Audio.Play(AudioType.Sound, FlappySoundEffectConfig.FLY);
                
                GameModule.Audio.Play(AudioType.Sound, FlappySoundEffectConfig.FLY,false,GameModule.Audio.SoundVolume);
                m_Rigidbody.velocity = new Vector2(0, _birdData.FlyForce);
            }
            
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.name == "ScorePipe")
            {
                //发送加分信息
                FlappyModel.Instance.TotalScore += 10;
                GameEvent.Send(FlappyEventDefine.ScoreChange);
            }
            else
            {
                // GameModule.Audio.Play(AudioType.Sound, FlappySoundEffectConfig.DEAD);
                GameModule.Audio.Play(AudioType.Sound, FlappySoundEffectConfig.DEAD,false,GameModule.Audio.SoundVolume);
                Destroy();
                GameEvent.Send(FlappyEventDefine.FlappyGameOver);
                //发送游戏结束消息       
            }
        }
    }
}
