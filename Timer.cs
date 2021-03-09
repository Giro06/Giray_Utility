using UnityEngine;

namespace Giroo.Utility
{
      
    public struct Timer
    {
        float duration;
        float timer;
        bool firtCompleteFlag;

        public float NormalizedTime => Mathf.Min(1, timer / duration);

        public float NormalizedTimePingPong
        {
            get
            {
                float t = Mathf.Min(1, timer / duration);
                if (t < 0.5f)
                {
                    return t * 2;
                }
                return (1 - t) * 2;
            }
        }
        public bool Done => timer >= duration;
        public float Duration => duration;
        public float TimePassed => timer;
      
        public Timer(float duration)
        {
            this.duration = duration;
            timer = 0;
            firtCompleteFlag = false;
        }

        public void Restart()
        {
            timer = 0;
            firtCompleteFlag = false;
        }

        public void Restart(float newDuration)
        {
            duration = newDuration;
            Restart();
        }

        public bool Update(float deltaTime)
        {
            timer += deltaTime;
            if (timer >= duration)
            {
                timer = duration;
                return true;
            }
            return false;
        }

        public bool Update(float deltaTime, out bool isFirstComplete)
        {
            bool r = Update(deltaTime);
            isFirstComplete = false;
            if (r && !firtCompleteFlag)
            {
                isFirstComplete = true;
                firtCompleteFlag = true;
            }
            return r;
        }

        public void ForceComplete()
        {
            timer = duration;
        }

        public override string ToString()
        {
            return timer + "/" + duration;
        }
    }
}

  
