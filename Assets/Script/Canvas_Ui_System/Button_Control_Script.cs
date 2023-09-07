using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace GDD
{
    public class Button_Control_Script : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private GameObject button_BG;
        private Button _button;
        private Animator _animator;
        private Button Bg_button_sc;
        private Animator Bg_animator;
        private UI_Control_Script _ui_cs;
        private GameObject Canvas_to_create;
        private GameObject CanvasSpawn = null;
        private Panel_UI_Script _panelUIScript;
        private bool isCreateCanvas;

        private bool Isp = false;

        public Panel_UI_Script panelUIScript
        {
            get { return _panelUIScript; }
            set { _panelUIScript = value; }
        }
        
        public bool IsCreateCanvas
        {
            get { return isCreateCanvas;}
            set { isCreateCanvas = value; }
        }
        public UI_Control_Script UI_CS
        {
            get { return _ui_cs; }
            set { _ui_cs = value; }
        }

        public GameObject canvas_to_create
        {
            get { return Canvas_to_create; }
            set { Canvas_to_create = value; }
        }
        
        public GameObject button_bg
        {
            get { return button_BG; }
            set { button_BG = value; }
        }

        public GameObject canvasSpawn
        {
            get { return CanvasSpawn; }
            set { CanvasSpawn = value; }
        }

        private void Start()
        {
            _button = GetComponent<Button>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (button_BG != null)
            {
                Bg_button_sc = button_BG.GetComponent<Button>();
                Bg_animator = button_BG.GetComponent<Animator>();
            }
            
            if (button_BG != null && gameObject.transform.parent.GetComponent<HorizontalLayoutGroup>() != null)
            {
                UpdateLayout();
            }
        }

        private void UpdateLayout()
        {
            bool layoutUpdate = gameObject.transform.parent.GetComponent<HorizontalLayoutGroup>().childControlWidth;
            bool BGlayoutUpdate = button_BG.transform.parent.GetComponent<HorizontalLayoutGroup>().childControlWidth;
            gameObject.transform.parent.GetComponent<HorizontalLayoutGroup>().childControlWidth = !layoutUpdate;
            Bg_button_sc.transform.parent.GetComponent<HorizontalLayoutGroup>().childControlWidth = !BGlayoutUpdate;
        }
        
        public void OnClickButton(UnityAction onClick)
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(onClick);
        }

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            Isp = true;
            //Debug.Log(this.gameObject.name + " was Selected");
            
            if(_animator != null)
                _animator.SetBool("IsStart", true);
            
            if(Bg_button_sc != null)
                Bg_animator.SetBool("IsStart", true);
        }
        
        public void OnPointerExit(PointerEventData pointerEventData)
        {
            Isp = false;
            Debug.Log(this.gameObject.name + " was Deselected");
            
            if(_animator != null)
            _animator.SetBool("IsStart", false);
            
            if(Bg_button_sc != null)
                Bg_animator.SetBool("IsStart", false);
        }

        public void OnCreateCanvas(float planeDistance = 2.0f, bool useCameraOverlay = false, int index = 0)
        {
            int OrderLayer = _panelUIScript.CheckUI_StillOpen();
            UI_CS.CheckPanel_StillOpen();
            UI_CS.PlayAnimPanel(0, false);
            
            isCreateCanvas = true;

            CanvasSpawn = Instantiate(Canvas_to_create);
            if (CanvasSpawn.GetComponent<BT_CS_Instance>() == null)
            {
                BT_CS_Instance _btCsInstance = CanvasSpawn.AddComponent<BT_CS_Instance>();
                _btCsInstance.BT_CS = GetComponent<Button_Control_Script>();
            }

            Canvas canvas = CanvasSpawn.GetComponent<Canvas>();

            if (useCameraOverlay)
            {
                canvas.worldCamera = Camera.main.GetUniversalAdditionalCameraData().cameraStack[index];
            }
            else
            {
                UI_CS.PlayAnimPanel(1, true);
                canvas.worldCamera = Camera.main;
            }

            canvas.sortingLayerName = "SortingLayerName";
            canvas.sortingOrder = OrderLayer;
            Canvas_to_create.GetComponent<Canvas>().planeDistance = planeDistance;
        }

        public void OnDestroyCanvas(bool IsSetPanelActionMode = true)
        {
            CanvasSpawn.GetComponent<Animator>().SetBool("IsStart", false);
            isCreateCanvas = false;
            
            if(IsSetPanelActionMode)
                UI_CS.PanelUIScript[0].ActionMode = PanelActionMode.Dont_Hide;
            
            UI_CS.PlayAnimPanel(0, true);
            UI_CS.PlayAnimPanel(1, false);
            
            Destroy(CanvasSpawn, CanvasSpawn.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            CanvasSpawn = null;
        }
    }

    public class BT_CS_Instance : MonoBehaviour
    {
        private Button_Control_Script bT_CS;

        public Button_Control_Script BT_CS
        {
            get => bT_CS;
            set => bT_CS = value;
        }
    }
}
