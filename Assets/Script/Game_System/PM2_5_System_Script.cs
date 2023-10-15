using System;
using UnityEngine;

namespace GDD
{
    public class PM2_5_System_Script : Sinagleton_CanDestroy<PM2_5_System_Script>
    {
        private GameManager GM;
        
        private float pm2_5_delta = 1;
        private bool is_add_value = false;
        public int pm2_5_value;

        private void Start()
        {
            GM = GameManager.Instance;
        }

        private void Update()
        {
            UpdatePm2_5();
            UpdateFogDistance();

            //pm2_5_value = 300; //(int)pm2_5_delta;
        }

        private void UpdatePm2_5()
        {
            if (pm2_5_value > 310)
            {
                is_add_value = false;
            }else if (pm2_5_value <= 1 )
            {
                is_add_value = true;
            }

            if (is_add_value)
            {
                pm2_5_delta += Time.deltaTime * 25.0f;
            }
            else
            {
                pm2_5_delta -= Time.deltaTime * 25.0f;
            }

            GM.PM_25 = pm2_5_value;
        }
        
        private void UpdateFogDistance()
        {
            float pm25_max = 310;
            MinMax distance_minmax = new MinMax();
            distance_minmax.min = 15;
            distance_minmax.max = 300;
            float pm25_scale = pm2_5_value / pm25_max;
            float pm25_distance = (distance_minmax.max - distance_minmax.min) * pm25_scale + distance_minmax.min;
            RenderSettings.fogEndDistance = (distance_minmax.max + distance_minmax.min) - pm25_distance;
        }
    }
}