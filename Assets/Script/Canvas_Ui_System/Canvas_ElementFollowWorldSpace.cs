using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_ElementFollowWorldSpace : MonoBehaviour
{
    [SerializeField] public Transform lookATransform;

    [SerializeField] public Vector3 offSet;

    [SerializeField] public float m_Set_Offset = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 adjustedPosition = Camera.main.WorldToScreenPoint(lookATransform.position + new Vector3(offSet.x, offSet.y + m_Set_Offset, offSet.z));
        
        RectTransform _rect = GetComponent<RectTransform>();
        _rect.anchoredPosition = new Vector2(adjustedPosition.x, adjustedPosition.y - Screen.height);
    }
}
