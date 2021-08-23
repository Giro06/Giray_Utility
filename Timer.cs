using System;

namespace Giroo.Utility
{
    public struct Timer
    {
        private float targetTime;
        private float currentTime;
        public bool isDone => targetTime <= currentTime;

        public float SettedTime => targetTime;
        public float PassedTime => currentTime;
        public float NormalizedTime => currentTime / targetTime;


        private bool isHaveAction;
        private Action actionToDo;

        private bool timeCircleDone;

        public Timer(float time)
        {
            this.targetTime = time;
            this.currentTime = 0;
            isHaveAction = false;
            actionToDo = null;
            timeCircleDone = false;
        }

        public Timer(float time, Action action)
        {
            this.targetTime = time;
            this.currentTime = 0;
            this.isHaveAction = true;
            this.actionToDo = action;
            timeCircleDone = false;
        }

        public void Update(float deltaTime)
        {
            if (timeCircleDone) return;
            
            currentTime += deltaTime;
            
            if (currentTime > targetTime)
            {
                currentTime = targetTime;
                timeCircleDone = true;
                if (isHaveAction)
                {
                    actionToDo?.Invoke();
                }
            }
        }

        public void Restart()
        {
            currentTime = 0;
            timeCircleDone = false;
        }

        public void ResetTime(float time)
        {
            this.targetTime = time;
            Restart();
        }
    }
}