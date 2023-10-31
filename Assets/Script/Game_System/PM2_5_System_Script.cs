using UnityEngine;

namespace GDD
{
    public class PM2_5_System_Script : Sinagleton_CanDestroy<PM2_5_System_Script>
    {
        private GameManager GM;
        private TimeManager TM;
        
        private float pm2_5_delta = 1;
        private bool is_add_value = false;
        private float current_time = 0;
        private bool is_change_pm = false;
        private int set_pm2_5_value;
        private int Old_pm2_5_value = 0;
        public int pm2_5_value;
        

        private void Start()
        {
            GM = GameManager.Instance;
            TM = TimeManager.Instance;
        }

        private void Update()
        {
            //UpdatePm2_5();

            if (is_change_pm)
                ChangePM2_5_Current();
            
            UpdateFogDistance();

            //pm2_5_value = 300; //(int)pm2_5_delta;
        }

        public void OnChangePM2_5_Value(int pm_value)
        {
            set_pm2_5_value = pm_value;
            current_time = 0;
            Old_pm2_5_value = pm2_5_value;
            is_change_pm = true;
            
            print("PM Value : " + pm_value);
        }

        private void ChangePM2_5_Current()
        {
            if (current_time + (TM.deltaTime * 0.01f) >= 1)
            {
                current_time = 1;
            }
            else
            {
                current_time += (TM.deltaTime * 0.01f);
            }
            
            pm2_5_value = (int)Mathf.Lerp(Old_pm2_5_value, set_pm2_5_value, current_time);
            if (current_time >= 1)
                is_change_pm = false;
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
            float pm25_max = 800;
            MinMax distance_minmax = new MinMax();
            distance_minmax.min = 120;
            distance_minmax.max = 400;
            float pm25_scale = pm2_5_value / pm25_max;
            float pm25_distance = (distance_minmax.max - distance_minmax.min) * pm25_scale + distance_minmax.min;
            RenderSettings.fogEndDistance = (distance_minmax.max + distance_minmax.min) - pm25_distance;
        }
    }
}