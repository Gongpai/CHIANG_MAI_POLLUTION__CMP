using System;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace GDD
{
    public class Ui_Utilities : MonoBehaviour
    {
        private GameObject Ui_Element;
        private Camera _camera;
        private GameObject _canvasUI;
        private float _planeDistance;
        private int _order_in_layer;
        private bool _useCameraOverlay = false;
        private int _cameraOverlay_Index = 0;

        public Camera camera
        {
            get => _camera;
            set => _camera = value;
        }

        public GameObject canvasUI
        {
            get => _canvasUI;
            set => _canvasUI = value;
        }

        public float planeDistance
        {
            get => _planeDistance;
            set => _planeDistance = value;
        }

        public int order_in_layer
        {
            get => _order_in_layer;
            set => _order_in_layer = value;
        }
        public bool useCameraOverlay 
        { 
            get => _useCameraOverlay;
            set => _useCameraOverlay = value;
        }

        public int cameraOverlay_Index
        {
            get => _cameraOverlay_Index;
            set => _cameraOverlay_Index = value;
        }
        
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

        public GameObject Add_Button_Element_To_List_View(GameObject List_View, UnityAction event_call,
            string Button_text)
        {
            if (Ui_Element.GetComponent<Canvas_Element_List>() != null)
            {
                //print("UIUIUIUIUI : " + Ui_Element.name);
                Ui_Element.GetComponent<Canvas_Element_List>().texts[0].text = Button_text;
                GameObject button = Instantiate(Ui_Element, List_View.transform);
                RectTransform but_rectTransform = button.GetComponent<RectTransform>();
                but_rectTransform.sizeDelta = new Vector2(1, 1);
                but_rectTransform.anchoredPosition = Vector2.zero;
                button.GetComponent<Button>().onClick.AddListener(event_call);
                return button;
            }
            else
            {
                return null;
            }
        }

        public GameObject CreateInputUI(UnityAction<string> event_on_value_Changed, UnityAction event_Button_OK, UnityAction event_Button_Cancel, string PlaceholderInputField, string OKText, string CancelText, bool isUseUiBack = true)
        {
            GameObject Input_Ui = Instantiate(_canvasUI);

            Canvas_Element_List input_ui_element = Input_Ui.GetComponent<Canvas_Element_List>();
            input_ui_element.inputFields[0].onValueChanged.AddListener(event_on_value_Changed);
            input_ui_element.GetComponent<Back_UI_Button_Script>().enabled = isUseUiBack;
            input_ui_element.buttons[0].onClick.AddListener(event_Button_OK + (() =>
            {
                if (!isUseUiBack)
                {
                    _canvasUI = Input_Ui;
                    RemoveUI();
                }
                else
                {
                    input_ui_element.GetComponent<Back_UI_Button_Script>().OnDestroyUI(false); 
                }
            }) );
            input_ui_element.buttons[1].onClick.AddListener(event_Button_Cancel + (() =>
            {
                if (!isUseUiBack)
                {
                    _canvasUI = Input_Ui;
                    print("Remove");
                    RemoveUI();
                }
                else
                {
                    input_ui_element.GetComponent<Back_UI_Button_Script>().OnDestroyUI(false); 
                }
            }));
            input_ui_element.inputFields[0].placeholder.GetComponent<TextMeshProUGUI>().text = PlaceholderInputField;
            input_ui_element.texts[0].text = OKText;
            input_ui_element.texts[1].text = CancelText;
            input_ui_element.texts[2].text = "Save Game";
            
            Canvas _canvas = Input_Ui.GetComponent<Canvas>();
            
            if (_useCameraOverlay)
            {
                _canvas.worldCamera = Camera.main.GetUniversalAdditionalCameraData().cameraStack[_cameraOverlay_Index];
            }
            else
            {
                _canvas.worldCamera = _camera;
            }
            
            _canvas.planeDistance = _planeDistance;
            _canvas.sortingOrder = _order_in_layer;

            return Input_Ui;
        }

        public GameObject CreateInputSliderUI(UnityAction<float> event_on_value_Changed, UnityAction event_Button_OK, UnityAction event_Button_Cancel, float min, float max, string title, string input_title, string OKText, string CancelText, bool isUseUiBack = false)
        {
            GameObject Input_Ui = Instantiate(_canvasUI);
            Canvas_Element_List input_ui_element = Input_Ui.GetComponent<Canvas_Element_List>();
            Slider _slider = input_ui_element.Sliders[0];
           _slider.onValueChanged.AddListener(event_on_value_Changed);
           _slider.minValue = min;
           _slider.maxValue = max;

           input_ui_element.GetComponent<Back_UI_Button_Script>().enabled = isUseUiBack;
           if (isUseUiBack)
           {
               input_ui_element.buttons[0].onClick.AddListener(() =>
               {
                   input_ui_element.GetComponent<Back_UI_Button_Script>().OnDestroyUI(false); 
               });
               input_ui_element.buttons[1].onClick.AddListener(() =>
               {
                   input_ui_element.GetComponent<Back_UI_Button_Script>().OnDestroyUI(false); 
               });
           }
           
           input_ui_element.canvas_gameObjects[0].SetActive(false);
           input_ui_element.canvas_gameObjects[1].SetActive(true);
           input_ui_element.buttons[0].onClick.AddListener(event_Button_OK + (() =>
           {
               if (!isUseUiBack)
               {
                   _canvasUI = Input_Ui;
                   RemoveUI();
               }
           }) );
           input_ui_element.buttons[1].onClick.AddListener(event_Button_Cancel + (() =>
           {
               if (!isUseUiBack)
               {
                   _canvasUI = Input_Ui;
                   RemoveUI();
               }
           }));
           
           input_ui_element.texts[0].text = OKText;
           input_ui_element.texts[1].text = CancelText;
           input_ui_element.texts[2].text = title;
           input_ui_element.canvas_gameObjects[1].GetComponent<SliderInput>().text = input_title;
           
           Canvas _canvas = Input_Ui.GetComponent<Canvas>();
            
           if (_useCameraOverlay)
           {
               _canvas.worldCamera = Camera.main.GetUniversalAdditionalCameraData().cameraStack[_cameraOverlay_Index];
           }
           else
           {
               _canvas.worldCamera = _camera;
           }
            
           _canvas.planeDistance = _planeDistance;
           _canvas.sortingOrder = _order_in_layer;
            
            return Input_Ui;
        }

        public GameObject CreateMessageUI(UnityAction event_Button_OK, UnityAction event_Button_Cancel, string title, string messageText, string OKText, string CancelText, bool isUseUiBack = true)
        {
            GameObject Message_Ui = Instantiate(_canvasUI);

            
            print("IS ONNN : " + isUseUiBack);
            Canvas_Element_List messagebox_element = Message_Ui.GetComponent<Canvas_Element_List>();
            messagebox_element.GetComponent<Back_UI_Button_Script>().enabled = isUseUiBack;
            messagebox_element.buttons[0].onClick.AddListener(event_Button_OK + (() =>
            {
                print("IS ONNN : " + isUseUiBack);
                
                if (!isUseUiBack)
                {
                    _canvasUI = Message_Ui;
                    RemoveUI();
                }
                else
                {
                    messagebox_element.GetComponent<Back_UI_Button_Script>().OnDestroyUI(false); 
                }
            }) );
            messagebox_element.buttons[1].onClick.AddListener(event_Button_Cancel + (() =>
            {
                print("IS ONNN : " + isUseUiBack);
                
                if (!isUseUiBack)
                {
                    _canvasUI = Message_Ui;
                    print("Remove");
                    RemoveUI();
                }
                else
                {
                    messagebox_element.GetComponent<Back_UI_Button_Script>().OnDestroyUI(false); 
                }
            }) );
            messagebox_element.texts[2].text = title;
            messagebox_element.texts[3].text = messageText;
            messagebox_element.texts[0].text = OKText;
            messagebox_element.texts[1].text = CancelText;
            
            Canvas _canvas = Message_Ui.GetComponent<Canvas>();
            
            if (_useCameraOverlay)
            {
                _canvas.worldCamera = Camera.main.GetUniversalAdditionalCameraData().cameraStack[_cameraOverlay_Index];
            }
            else
            {
                _canvas.worldCamera = _camera;
            }
            
            _canvas.planeDistance = _planeDistance;
            _canvas.sortingOrder = _order_in_layer;

            return Message_Ui;
        }

        public GameObject CreateUI()
        {
            GameObject ui = Instantiate(_canvasUI);
            Canvas _canvas = ui.GetComponent<Canvas>();
            
            if (_useCameraOverlay)
            {
                _canvas.worldCamera = Camera.main.GetUniversalAdditionalCameraData().cameraStack[_cameraOverlay_Index];
            }
            else
            {
                _canvas.worldCamera = _camera;
            }
            
            _canvas.planeDistance = _planeDistance;
            _canvas.sortingOrder = _order_in_layer;

            return ui;
        }
        
        public void RemoveUI(float offset_time = 0)
        {
            _canvasUI.GetComponent<Animator>().SetBool("IsStart", false);
            
            Destroy(_canvasUI, _canvasUI.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + offset_time);
            _canvasUI = null;
        }
    }
}