using System;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace GDD
{
    public class Ui_Utilities : MonoBehaviour
    {
        private GameObject Ui_Element;

        public GameObject UI_Elemant
        {
            get { return Ui_Element; }
            set { Ui_Element = value; }
        }

        /// <summary>
        /// Create button ui element
        /// </summary>
        /// <param name="interactable">interactable button</param>
        /// <param name="images">Sprite image for button</param>
        /// <param name="color">Color image</param>
        public void Create_New_Button_UI_Element(string text, bool interactable = default, Sprite images = default,
            Color color = default)
        {
            GameObject button = new GameObject();
            button.AddComponent<Button>().AddComponent<Image>();
            button.GetComponent<Button>().interactable = interactable;
            button.GetComponent<Image>().sprite = images;
            button.GetComponent<Image>().color = color;

            GameObject Text_element = new GameObject();
            Text_element.AddComponent<TextMeshProUGUI>().text = text;
            Text_element.transform.parent = button.transform;

            Ui_Element = Text_element;
        }

        public GameObject Add_Button_Element_To_List_View(GameObject List_View, UnityAction event_call, string Button_text, int index_child = 0)
        {
            TextMeshProUGUI but_text = Ui_Element.transform.GetChild(index_child).GetComponent<TextMeshProUGUI>();
            if(but_text != null)
                but_text.text = Button_text;
            
            GameObject button = Instantiate(Ui_Element, List_View.transform);
            button.GetComponent<Button>().onClick.AddListener(event_call);
            return button;
        }

        public void CreateInputUI(GameObject InputUiGameObject, UnityAction<string> event_on_value_Changed, UnityAction event_Button_OK, string PlaceholderInputField, string OKText, string CancelText)
        {
            GameObject Input_Ui = Instantiate(InputUiGameObject);

            InputUi input_ui = Input_Ui.GetComponent<InputUi>();
            input_ui.Input_Field.onValueChanged.AddListener(event_on_value_Changed);
            input_ui.OK_Button.onClick.AddListener(event_Button_OK + (() => { Destroy(Input_Ui); }) );
            input_ui.Cancel_Button.onClick.AddListener(() => { Destroy(Input_Ui); });
            input_ui.Placeholder_Input_Field.text = PlaceholderInputField;
            input_ui.OK_Text.text = OKText;
            input_ui.Cancel_Text.text = CancelText;
        }

        public void CreateMessageUI(GameObject MessageUiGameObject, UnityAction event_Button_OK, string MessageText, string OKText, string CancelText)
        {
            GameObject Message_Ui = Instantiate(MessageUiGameObject);

            MessageBoxUi messagebox_ui = Message_Ui.GetComponent<MessageBoxUi>();
            messagebox_ui.OK_Button.onClick.AddListener(event_Button_OK + (() => { Destroy(Message_Ui); }) );
            messagebox_ui.Cancel_Button.onClick.AddListener(() => { Destroy(Message_Ui); });
            messagebox_ui.Message_Text.text = MessageText;
            messagebox_ui.OK_Text.text = OKText;
            messagebox_ui.Cancel_Text.text = CancelText;
        }
    }
}