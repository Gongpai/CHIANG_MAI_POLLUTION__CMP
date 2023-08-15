using UnityEngine;
using UnityEngine.Events;

namespace GDD
{
    public struct Menu_Data
    {
        public string text;
        public Sprite sprite;
        public UnityAction unityAction;

        public Menu_Data(string _text, Sprite _sprite, UnityAction _unityAction)
        {
            text = _text;
            sprite = _sprite;
            unityAction = _unityAction;
        }
    }
}