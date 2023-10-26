
using UnityEngine;
using UnityEngine.Audio;

namespace GDD
{
    [CreateAssetMenu(menuName = "GDD/SoundSettingsPreset",
    fileName = "SoundSettingsPreset")]

    public class SoundSettings : ScriptableObject
    {
        public AudioMixer AudioMixer;

        [Header("MasterVolume")]
        public string MasterVolumeName = "MasterVolume";
        [Range(-80, 20)]
        public float MasterVolume;

        [Header("MusicVolume")]
        public string MusicVolumeName = "MusicVolume";
        [Range(-80, 20)]
        public float MusicVolume;

        [Header("MasterSFXVolume")]
        public string MasterSFXVolumeName = "MasterSFXVolume";
        [Range(-80, 20)]
        public float MasterSFXVolume;

        [Header("SFXVolume")]
        public string SFXVolumeName = "SFXVolume";
        [Range(-80, 20)]
        public float SFXVolume;

        [Header("UIVolume")]
        public string UIVolumeName = "UIVolume";
        [Range(-80, 20)]
        public float UIVolume;
    }
}
