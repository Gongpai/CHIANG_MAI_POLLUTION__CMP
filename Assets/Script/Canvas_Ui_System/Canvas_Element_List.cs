using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class Canvas_Element_List : MonoBehaviour
    {
        [SerializeField] private List<Button> m_buttons;
        [SerializeField] private List<TextMeshProUGUI> m_texts;
        [SerializeField] private List<TMP_InputField> m_tmp_inputfield;
        [SerializeField] private List<Slider> m_sliders;
        [SerializeField] private List<Image> m_images;
        [SerializeField] private List<Animator> m_animators;
        [SerializeField] private List<AudioSource> m_audioSources;
        [SerializeField] private List<GameObject> m_canvas_gameObject;

        public List<Button> buttons
        {
            get => m_buttons;
        }

        public List<TextMeshProUGUI> texts
        {
            get => m_texts;
        }

        public List<TMP_InputField> inputFields
        {
            get => m_tmp_inputfield;
        }

        public List<Slider> Sliders
        {
            get => m_sliders;
        }

        public List<Image> image
        {
            get => m_images;
        }

        public List<Animator> animators
        {
            get => m_animators;
        }

        public List<AudioSource> audioSources
        {
            get => m_audioSources;
        }

        public List<GameObject> canvas_gameObjects
        {
            get => m_canvas_gameObject;
        }
    }
}