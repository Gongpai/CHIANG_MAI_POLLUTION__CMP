using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace GDD
{
    public class Display_Setting_Script : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown m_screen_resolution_dropdown;
        [SerializeField] private TMP_Dropdown m_fullscreen_mode_dropdown;

        private Display_Data _displayData;

        private void OnEnable()
        {
            Set_Screen_Resolution_Dropdown();
            Set_Fullscreen_Mode_Dropdown();
        }

        private void Set_Screen_Resolution_Dropdown()
        {
            m_screen_resolution_dropdown.ClearOptions();
            
            List<Resolution> resolutions = Screen.resolutions.ToList();
            
            foreach (var res in resolutions)
            {
                m_screen_resolution_dropdown.options.Add(new TMP_Dropdown.OptionData(res.width + "x" + res.height + " : " + res.refreshRate));
            }

            int select_res = Get_Res_Index(Screen.currentResolution);
            if (select_res >= 0)
                m_screen_resolution_dropdown.value = select_res;
            else if (_displayData != null)
            {
                Screen.SetResolution(_displayData.w, _displayData.h, _displayData.fullScreenMode);
                m_screen_resolution_dropdown.value = _displayData.res_index;
            }
        }

        private void Set_Fullscreen_Mode_Dropdown()
        {
            m_fullscreen_mode_dropdown.ClearOptions();
            
            m_fullscreen_mode_dropdown.options.Add(new TMP_Dropdown.OptionData("FullScreen"));
            m_fullscreen_mode_dropdown.options.Add(new TMP_Dropdown.OptionData("Exclusive FullScreen"));
            m_fullscreen_mode_dropdown.options.Add(new TMP_Dropdown.OptionData("Borderless Window"));
            m_fullscreen_mode_dropdown.options.Add(new TMP_Dropdown.OptionData("Window"));

            m_fullscreen_mode_dropdown.value = Convert.ToInt32(Screen.fullScreenMode);
        }

        public void Set_Screen_Resolution(int index)
        {
            int w = Screen.resolutions[index].width;
            int h = Screen.resolutions[index].height;
            Screen.SetResolution(w, h, Screen.fullScreenMode);

            if (_displayData == null)
                _displayData = new Display_Data();

            _displayData.res_index = index;
            _displayData.w = w;
            _displayData.h = h;
            
            m_screen_resolution_dropdown.value = index;
        }

        public void Set_Fullscreen_Mode(int index)
        {
            print("Select Full Index : " + index);
            Screen.fullScreenMode = (FullScreenMode)index;
            
            if (_displayData == null)
                _displayData = new Display_Data();
            
            _displayData.fullScreenMode = Screen.fullScreenMode;
            m_fullscreen_mode_dropdown.value = index;
        }
        
        private int Get_Res_Index(Resolution res)
        {
            int i = -1;
            Parallel.ForEach(Screen.resolutions, (resolution, state, index) =>
            {
                if (resolution.width == res.width && resolution.height == res.height)
                {
                    i = (int)index;
                    state.Break();
                }
            });

            return i;
        }
    }

    public class Display_Data
    {
        public int h;
        public int w;
        public int res_index;
        public FullScreenMode fullScreenMode;

        public Display_Data(int _h = default, int _w = default, int _res_index = default, FullScreenMode _fullScreenMode = default)
        {
            h = _h;
            w = _w;
            res_index = _res_index;
            fullScreenMode = _fullScreenMode;
        }
    }
}