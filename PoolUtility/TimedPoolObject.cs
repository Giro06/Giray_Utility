using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Giroo.Utility
{
    public class TimedPoolObject : MonoBehaviour
    {
        public Timer timer;

        public void SetTimer(float returnTime)
        {
            timer = new Timer(returnTime);
        }
        // Update is called once per frame
        void Update()
        {
            timer.Update(Time.deltaTime);
            if (timer.Done)
            {
                GiroPool.instance.ReturnObjectToPool(this.gameObject);
                Destroy(this);
            }
        }
    }
}

