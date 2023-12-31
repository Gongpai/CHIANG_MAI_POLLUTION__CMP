﻿using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace GDD
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonControllerAI))]
    public class WaypointReachingState : MonoBehaviour
    {
        [SerializeField] protected int m_CurrentWaypointIndex = 0;

        [SerializeField] protected ThirdPersonControllerAI m_3rdPersonControllerAI;
       
        [SerializeField] protected NavMeshAgent m_NavMeshAgent;

        [SerializeField] protected List<Transform> m_Waypoints;

        [SerializeField] private bool m_is_Start = false;

        [SerializeField] private bool is_start_now = false;

        private TimeManager TM;
        
        public List<Transform> waypoints
        {
            get => m_Waypoints;
            set => m_Waypoints = value;
        }
        
        public int SetWaypointIndex
        {
            set => m_CurrentWaypointIndex = value;
        }

        public bool is_Start
        {
            set => m_is_Start = value;
        }
        
        private void Start()
        {
            TM = TimeManager.Instance;
            
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_3rdPersonControllerAI = GetComponent<ThirdPersonControllerAI>();
        }

        public void EnterState()
        {
            m_NavMeshAgent.SetDestination(m_Waypoints[m_CurrentWaypointIndex].position);
        }

        private void Update()
        {
            if (!is_start_now)
            {
                EnterState();
                is_start_now = true;
            }
            if(m_is_Start && TM.timeScale > 0)
                UpdateState();

            //print("Current " + gameObject.name + " distance : " + Mathf.Abs(Vector3.Magnitude(transform.position - m_Waypoints[m_CurrentWaypointIndex].position)));
            if (Mathf.Abs(Vector3.Magnitude(transform.position - m_Waypoints[m_CurrentWaypointIndex].position)) <= 0.51f)
            {
                DestroyAI();
            }
            if (Mathf.Abs(Vector3.Magnitude(transform.position - m_Waypoints[m_CurrentWaypointIndex].position)) <= 1.51f)
            {
                DestroyAI(10);
            }
        }

        public void UpdateState()
        {
            if (m_NavMeshAgent.remainingDistance > m_NavMeshAgent.stoppingDistance)
            {
                print("Moveeeeeeeeeee");
                m_3rdPersonControllerAI.Move(m_NavMeshAgent.desiredVelocity);
            }
            else
            {
                m_3rdPersonControllerAI.Move(Vector3.zero);
                //DestroyAI();
                //is_Start = false;
                /*
                m_3rdPersonControllerAI.Move(Vector3.zero);

                if (m_CurrentWaypointIndex + 1 < m_Waypoints.Count)
                {
                    m_CurrentWaypointIndex++;
                    CustomEvent.Trigger(gameObject,"GotoIdleState");
                }
                else
                {//reach the maximum of the available waypoints, then go back to 0
                    m_CurrentWaypointIndex = 0;
                    CustomEvent.Trigger(gameObject,"GotoIdleState");
                }
                */
            }
        }

        public void DestroyAI(float t = 1)
        {
            print("Destroy : " + gameObject.name);
            Destroy(gameObject, t);
        }

    }
}