using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(fileName = "PM2_5_Preset",
        menuName = "GDD/PM2_5 Preset", order = 1)]
    public class PM2_5_Preset : ScriptableObject
    {
        [Header("Day 0-30")]
        public int before_pm2_5_30 = 0;
        public int after_pm2_5_30 = 200;
        
        [Header("Day 31-60")]
        public int before_pm2_5_60 = 150;
        public int after_pm2_5_60 = 400;
        
        [Header("Day 61-90")]
        public int before_pm2_5_90 = 300;
        public int after_pm2_5_90 = 600;
        
        [Header("Day 91+")]
        public int before_pm2_5_100 = 400;
        public int after_pm2_5_100 = 800;
    }
}