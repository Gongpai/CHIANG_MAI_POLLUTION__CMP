using UnityEngine;

namespace GDD
{
    public class pm2_5_Setting : MonoBehaviour
    {
        private PM2_5_System_Script PM2_5;
        
        public void Set_PM2_5 (float value)
        {
            PM2_5 = PM2_5_System_Script.Instance;
            PM2_5.pm2_5_value = (int)(310 * value);
        }
    }
}