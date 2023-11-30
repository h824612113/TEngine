using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class BaseEntity : MonoBehaviour
    {

        public SpawnHandle _handle;


        public virtual void InitEntity(SpawnHandle handle)
        {
            this._handle = handle;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Destroy()
        {
            _handle.Restore();
            _handle = null;
        }
    }
}
