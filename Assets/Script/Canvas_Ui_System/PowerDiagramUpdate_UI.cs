using System;
using UnityEngine;
using Random = System.Random;

namespace GDD
{
    public class PowerDiagramUpdate_UI : MonoBehaviour
    {
        [SerializeField]private DD_DataDiagram m_DataDiagram;
        private ResourcesManager RM;
        private TimeManager TM;
        private float current_hour_update = 0;
        private float current_point = 1;
        GameObject line;
        
        private void Start()
        {
            RM = ResourcesManager.Instance;
            TM = TimeManager.Instance;
            Color color = Color.yellow;
            line = m_DataDiagram.AddLine("hslsdjflsjdflsj", color);
        }

        private void FixedUpdate()
        {
            UpdatePerSec();
        }

        private void UpdatePerSec()
        {
            if (current_hour_update <= TM.To_TotalSecond(TM.get_DateTime))
            {
                m_DataDiagram.InputPoint(line, new Vector2(current_point, RM.Get_Resources_Power()));
                print("current_point : " + current_point);
                current_point = 1 * (float)(TM.deltaTime * 0.01);
                current_hour_update = TM.To_TotalSecond(TM.get_DateTime) + 1;
            }
        }
    }
}