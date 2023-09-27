using UnityEngine;

namespace GDD
{
    public class Notification
    {
        private bool _isTriggr = false;
        
        public string text { get; set; }
        public Sprite icon { get; set; }
        public Color iconColor { get; set; }
        
        public AudioClip soundSFX { get; set; }
        
        public float duration { get; set; }
        public bool isWaitTrigger { get; set; }
        public bool isTrigger
        {
            get => _isTriggr;
            set => _isTriggr = value;
        }
    }
}