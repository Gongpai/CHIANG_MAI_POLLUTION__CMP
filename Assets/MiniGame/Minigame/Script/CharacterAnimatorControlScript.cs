using UnityEngine;

namespace Script.Unit7
{
    public class CharacterAnimatorControlScript : MonoBehaviour
    {
        protected Animator m_Animator;
        private Rigidbody _rigidbody;

        private static readonly int Punch = Animator.StringToHash("Punch");
        private static readonly int Dancing = Animator.StringToHash("Dancing");
        private static readonly int State = Animator.StringToHash("State");
        private static readonly int Turn = Animator.StringToHash("Turn");

        private void Start()
        {
            m_Animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Animator.SetTrigger("Jump");
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                m_Animator.SetTrigger(Punch);
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                m_Animator.SetBool(Dancing, true);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                m_Animator.SetInteger(State, 2);
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                m_Animator.SetFloat("Forward", 1f);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                m_Animator.SetFloat("Forward", 0.5f);
            }
            
            if (Input.GetKeyDown(KeyCode.W))
            {
                m_Animator.SetFloat("Forward", 0.5f);
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                m_Animator.SetFloat("Forward", 0);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                m_Animator.SetFloat(Turn, 0.5f);
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                m_Animator.SetFloat(Turn, 0);
            }
            
            if (Input.GetKeyDown(KeyCode.A))
            {
                m_Animator.SetFloat(Turn, -0.5f);
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                m_Animator.SetFloat(Turn, 0);
            }
            
            /*if (Input.GetKeyDown(KeyCode.Q))
            {
                m_Animator.SetFloat(Turn, -1f);
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                m_Animator.SetFloat(Turn, 0);
            }
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                m_Animator.SetFloat(Turn, 1f);
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                m_Animator.SetFloat(Turn, 0);
            }*/
        }
    }
}