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
        [SerializeField] private List<Image> m_images;
        [SerializeField] private List<Animator> m_animators;

        public List<Button> buttons
        {
            get => m_buttons;
        }

        public List<TextMeshProUGUI> tests
        {
            get => m_texts;
        }

        public List<Image> image
        {
            get => m_images;
        }

        public List<Animator> animators
        {
            get => m_animators;
        }
    }
}