using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GDD
{
    public struct Button_Action_Data
    {
        public string text;
        public Sprite sprite;
        public ColorBlock colorBlock;
        public UnityAction unityAction;

        public Button_Action_Data(string _text, Sprite _sprite, UnityAction _unityAction,ColorBlock _colorBlock = default)
        {
            text = _text;
            sprite = _sprite;
            colorBlock = _colorBlock;
            unityAction = _unityAction;
        }
    }
}