using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TEngine;
using UnityEngine;
using AudioType = TEngine.AudioType;

namespace GameLogic
{
    public class BulletEntity : BaseEntity
    {
        
        private BulletData _bulletData;
        public override void InitEntity(SpawnHandle handle)
        {
            base.InitEntity(handle);
            _bulletData = handle.UserDatas[0] as BulletData;

        }

        // Update is called once per frame
        void Update()
        {
            this.transform.Translate(Vector2.right*_bulletData.FlySpeed*Time.deltaTime,Space.World);
            if (this.transform.position.x >= _bulletData.HideDistance)
            {
                Destroy();
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            GameModule.Audio.Play(AudioType.Sound, FlappySoundEffectConfig.DEAD);
            other.gameObject.SetActive(false);
            Destroy();
        }
    }
}
