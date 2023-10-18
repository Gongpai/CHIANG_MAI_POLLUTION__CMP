using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

namespace GDD
{
    public class IdleState : MonoBehaviour
    {
        protected float m_IdleTime;
        
        
        public void EnterState()
        {
            RandomIdleTime();
        }

        private void RandomIdleTime()
        {
            m_IdleTime = UnityEngine.Random.Range(2, 5);
        }

        
        public void UpdateState()
        {
            ReduceIdleTimer();
        }


        private void ReduceIdleTimer()
        {
            m_IdleTime -= Time.deltaTime;
            if (m_IdleTime <= 0)
            {
                CustomEvent.Trigger(gameObject,"GotoWaypointReachingState");
            }
        }

        public void ExitState()
        {
            
        }
    }
}