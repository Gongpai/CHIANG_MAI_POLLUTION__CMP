using System;
using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] protected float m_TimerDuration = 5;

        [SerializeField] protected UnityEvent m_TimerStartEvent = new();
        [SerializeField] protected UnityEvent m_TimerEndEvent = new();

        protected bool _IsTimerStart = false;
        protected float _StartTimeStamp;
        protected float _EndTimeStamp;

        [SerializeField] protected float _CurrentTime;

        private void Update()
        {
            if (!_IsTimerStart) return;

            _CurrentTime = (Time.time - _StartTimeStamp);

            if (Time.time >= _EndTimeStamp)
            {
                EndTimer();
            }
        }

        public virtual void StartTimer()
        {
            //Check if the timer is already running
            if (_IsTimerStart) return;

            m_TimerStartEvent.Invoke();

            _IsTimerStart = true;
            _StartTimeStamp = Time.time;
            _EndTimeStamp = Time.time + m_TimerDuration;
        }

        public virtual void EndTimer()
        {
            m_TimerEndEvent.Invoke();
            _IsTimerStart = false;
        }
    }
}