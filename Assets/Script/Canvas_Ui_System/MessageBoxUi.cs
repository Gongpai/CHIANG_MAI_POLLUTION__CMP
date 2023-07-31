using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class MessageBoxUi : MonoBehaviour
    {
        [SerializeField] private Button OKButton;
        [SerializeField] private Button CancelButton;
        [SerializeField] private TextMeshProUGUI MessageText;
        [SerializeField] private TextMeshProUGUI OKText;
        [SerializeField] private TextMeshProUGUI CancelText;

        public Button OK_Button{
            get
            {
                return OKButton;
            }
            set
            {
                OKButton = value;
            }
        }
        public Button Cancel_Button{
            get
            {
                return CancelButton;
            }
            set
            {
                CancelButton = value;
            }
        }
        
        public TextMeshProUGUI Message_Text{
            get
            {
                return MessageText;
            }
            set
            {
                MessageText = value;
            }
        }
        public TextMeshProUGUI OK_Text{
            get
            {
                return OKText;
            }
            set
            {
                OKText = value;
            }
        }
        public TextMeshProUGUI Cancel_Text{
            get
            {
                return CancelText;
            }
            set
            {
                CancelText = value;
            }
        }
    }
}
