using System;
using System.Collections;
using System.Collections.Generic;
using GDD;
using Unity.VisualScripting;
using UnityEngine;

public class Building_place : MonoBehaviour
{
    private Spawner_Object_Grid S_Obj_place;

    public Spawner_Object_Grid Spawner_Obj_place
    {
        get { return S_Obj_place; }
    }
    
    public void Select_Building(string assetPath)
    {
        var asset = Resources.Load(assetPath);
        S_Obj_place.ObjectToSapwn = asset.GameObject();
    }

    public void OnActivateBuilding_Place()
    {
        S_Obj_place.enabled = true;
        S_Obj_place.SetPositionShowGirdUseMouse.enabled = true;
    }
    public void OnDisabledBuilding_Place()
    {
        if(S_Obj_place != null)
            S_Obj_place.enabled = false;
        
        if(S_Obj_place.SetPositionShowGirdUseMouse != null)
            S_Obj_place.SetPositionShowGirdUseMouse.enabled = false;
    }

    private void Awake()
    {
        S_Obj_place = FindObjectOfType<Spawner_Object_Grid>();
    }
}
