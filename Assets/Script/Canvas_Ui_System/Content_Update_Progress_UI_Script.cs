using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class Content_Update_Progress_UI_Script : MonoBehaviour
    {
        [SerializeField] private Image content_progress_bar;
        private TimeManager TM;
        private GameManager GM;
        private int old_time = -1;

        private void Start()
        {
            TM = TimeManager.Instance;
            GM = GameManager.Instance;
        }

        private void Update()
        {
            if (old_time != TM.getGameTimeHour)
            {
                float all_content = 0;

                Parallel.ForEach(GM.gameInstance.villagerSaveDatas, data =>
                {
                    all_content += data.content;
                });

                Parallel.ForEach(GM.gameInstance.workerSaveDatas, data =>
                {
                    all_content += data.content;
                });

                if (TM.getGameTimeHour == 0)
                    content_progress_bar.fillAmount = 1;
                else
                    content_progress_bar.fillAmount = all_content / (GM.gameInstance.villagerSaveDatas.Count + GM.gameInstance.workerSaveDatas.Count);
                old_time = TM.getGameTimeHour;
                print("Content : " + (all_content / (GM.gameInstance.villagerSaveDatas.Count + GM.gameInstance.workerSaveDatas.Count)));
            }
        }
    }
}