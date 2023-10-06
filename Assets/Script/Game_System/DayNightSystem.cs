using System;
using UnityEngine;

namespace GDD
{
    public class DayNightSystem : Sinagleton_CanDestroy<DayNightSystem>
    {
        private TimeManager TM;

        private void Start()
        {
            TM = TimeManager.Instance;
        }

        private void Update()
        {
            transform.transform.localEulerAngles = new Vector3((TM.To_TotalHour(TM.get_DateTime)  / 24.0f * -360) - 90, 45, 0);
            RenderSettings.skybox.SetFloat("Vector1_44C748BB", TM.timeScale);
        }
    }
}