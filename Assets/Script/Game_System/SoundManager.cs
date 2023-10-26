using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] protected SoundSettings m_SoundSettings;

        public Slider m_SliderMasterVolume;
        public Slider m_SliderMusicVolume;
        public Slider m_SliderMasterSFXVolume;
        public Slider m_SliderSFXVolume;
        public Slider m_SliderUIVolume;

// Start is called before the first frame update
        void Start()
        {
            InitialiseVolumes();
        }

        private void InitialiseVolumes()
        {
            SetMasterVolume(m_SoundSettings.MasterVolume);
            SetMusicVolume(m_SoundSettings.MusicVolume);
            
            if(m_SliderMasterSFXVolume != null)
                SetMasterSFXVolume(m_SoundSettings.MasterSFXVolume);
            
            SetSFXVolume(m_SoundSettings.SFXVolume);
            SetUIVolume(m_SoundSettings.UIVolume);
        }

        public void SetMasterVolume(float vol)
        {
            print("Volue is : " + vol);
            
            //Set float to the audiomixer
            m_SoundSettings.AudioMixer.SetFloat(m_SoundSettings.MasterVolumeName, vol);
            //Set float to the scriptable object to persist the value although the game is closed
                m_SoundSettings.MasterVolume = vol;
            //Set the slider bar’s value
            m_SliderMasterVolume.value = m_SoundSettings.MasterVolume;
        }

        public void SetMusicVolume(float vol)
        {
            //Set float to the audiomixer
            m_SoundSettings.AudioMixer.SetFloat(m_SoundSettings.MusicVolumeName, vol);
            //Set float to the scriptable object to persist the value although the game is closed
                m_SoundSettings.MusicVolume = vol;
            //Set the slider bar’s value
            m_SliderMusicVolume.value = m_SoundSettings.MusicVolume;
        }

        public void SetMasterSFXVolume(float vol)
        {
            //Set float to the audiomixer
            m_SoundSettings.AudioMixer.SetFloat(m_SoundSettings.MasterSFXVolumeName, vol);
            //Set float to the scriptable object to persist the value although the game is closed
                m_SoundSettings.MasterSFXVolume = vol;
            //Set the slider bar’s value
            m_SliderMasterSFXVolume.value = m_SoundSettings.MasterSFXVolume;
        }

        public void SetSFXVolume(float vol)
        {
//Set float to the audiomixer
            m_SoundSettings.AudioMixer.SetFloat(m_SoundSettings.SFXVolumeName, vol);
            //Set float to the scriptable object to persist the value although the game is closed
                m_SoundSettings.SFXVolume = vol;
            //Set the slider bar’s value
            m_SliderSFXVolume.value = m_SoundSettings.SFXVolume;
        }

        public void SetUIVolume(float vol)
        {
            //Set float to the audiomixer
            m_SoundSettings.AudioMixer.SetFloat(m_SoundSettings.UIVolumeName, vol);
            //Set float to the scriptable object to persist the value although the game is closed
                m_SoundSettings.UIVolume = vol;
            //Set the slider bar’s value
            m_SliderUIVolume.value = m_SoundSettings.UIVolume;
        }

    }
}