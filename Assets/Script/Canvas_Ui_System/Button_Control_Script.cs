using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace GDD
{
    public class Button_Control_Script : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private GameObject button_BG;
        private Button _button;
        private Animator _animator;
        private Button Bg_button_sc;
        private Animator Bg_animator;
        private GameUI_Anim_Controller_Script _GameUI_ACS;
        private GameObject Canvas_to_create;
        private GameObject CanvasSpawn;
        private bool isCreateCanvas;

        private bool Isp = false;

        public bool IsCreateCanvas
        {
            get { return isCreateCanvas;}
            set { isCreateCanvas = value; }
        }
        public GameUI_Anim_Controller_Script GameUI_ACS
        {
            get { return _GameUI_ACS; }
            set { _GameUI_ACS = value; }
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
            Debug.Log(this.gameObject.name + " was Selected");
            
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

        public void OnCreateCanvas()
        {
            isCreateCanvas = true;
            GameUI_ACS.PlayAnimToolBar(false);
            GameUI_ACS.PlayAnimHomeBar();
            CanvasSpawn = Instantiate(Canvas_to_create);
            Canvas canvas = Canvas_to_create.GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = "SortingLayerName";
            canvas.sortingOrder = 0;
            Canvas_to_create.GetComponent<Canvas>().planeDistance = 2.0f;
        }
        
        public void OnDestroyCanvas()
        {
            isCreateCanvas = false;
            GameUI_ACS.PanelUIScriptToolPanel.ActionMode = PanelActionMode.Dont_Hide;
            GameUI_ACS.PlayAnimToolBar();
            GameUI_ACS.PlayAnimHomeBar(false);
            CanvasSpawn.GetComponent<Animator>().SetBool("IsStart", false);
            Destroy(CanvasSpawn, CanvasSpawn.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        }
    }
}
