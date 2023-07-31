using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GDD
{
    public class InputUi : MonoBehaviour
    {
        [SerializeField] private TMP_InputField InputField;
        [SerializeField] private Button OKButton;
        [SerializeField] private Button CancelButton;
        [SerializeField] private TextMeshProUGUI PlaceholderInputField;
        [SerializeField] private TextMeshProUGUI OKText;
        [SerializeField] private TextMeshProUGUI CancelText;

        public TMP_InputField Input_Field
        {
            get
            {
                return InputField;
            }
            set
            {
                InputField = value;
            }
        }
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
        
        public TextMeshProUGUI Placeholder_Input_Field{
            get
            {
                return PlaceholderInputField;
            }
            set
            {
                PlaceholderInputField = value;
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
