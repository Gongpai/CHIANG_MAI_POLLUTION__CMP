using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GDD
{
    public static class PointerOverUIElement
    {
        public static bool OnPointerOverUIElement()
        {
            int UILayer = LayerMask.NameToLayer("UI");
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            
            for (int index = 0; index < raysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = raysastResults[index];
                //print(curRaysastResult.gameObject.layer + " : " + UILayer);
                if (curRaysastResult.gameObject.layer == UILayer)
                {
                    //print("Detect UI !!!!!!!!!!!!!");
                    return true;
                }
            }
            //print("Not Detect UI !!!!!!!!!!!!!");
            return false;
        }
    }
}