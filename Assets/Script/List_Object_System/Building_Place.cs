using System;
using System.Collections;
using System.Collections.Generic;
using GDD;
using Unity.VisualScripting;
using UnityEngine;

public class Building_place : MonoBehaviour
{
    private Spawner_Object_Grid S_Obj_place;
    private Spawner_Road_Grid S_Road_place;

    public Spawner_Object_Grid Spawner_Obj_place
    {
        get { return S_Obj_place; }
    }
    
    public void Select_Building(string assetPath)
    {
        OnActivateBuilding_Place();
        OnDisabledRoad_Place();
        var asset = Resources.Load(assetPath);
        S_Obj_place.ObjectToSapwn = asset.GameObject();
        S_Obj_place.objectData = new List<string>();

        int indexNameObject = assetPath.Split("/").Length;
        S_Obj_place.objectData.Add(assetPath.Split("/")[indexNameObject - 1]);
        S_Obj_place.objectData.Add(assetPath);
    }
    
    public void Select_Road(string assetPath)
    {
        OnDisabledBuilding_Place(false);
        OnActivateRoad_Place();
        
        if (assetPath != null)
        {
            var asset = Resources.Load(assetPath);
            S_Road_place.RoadMesh = asset.GameObject().GetComponent<MeshFilter>().sharedMesh;
            S_Road_place.roadMode = RoadMode.Place;
            int indexNameObject = assetPath.Split("/").Length;
            S_Road_place.objectData.Add(assetPath.Split("/")[indexNameObject - 1]);
            S_Road_place.objectData.Add(assetPath);
        }
        else
        {
            S_Road_place.roadMode = RoadMode.Remove;
        }
    }

    public void OnActivateBuilding_Place()
    {
        S_Obj_place.enabled = true;
        S_Obj_place.SetPositionShowGirdUseMouse.enabled = true;
        S_Obj_place.SetPositionShowGirdUseMouse.IsShowGrid = true;
    }
    public void OnDisabledBuilding_Place(bool isDisableGrid = true)
    {
        if(S_Obj_place != null)
            S_Obj_place.enabled = false;

        if (S_Obj_place.SetPositionShowGirdUseMouse != null && isDisableGrid)
        {
            S_Obj_place.SetPositionShowGirdUseMouse.enabled = false;
        }
        else
        {
            S_Obj_place.SetPositionShowGirdUseMouse.IsShowGrid = false;
        }
            
    }

    public void OnActivateRoad_Place()
    {
        S_Road_place.enabled = true;
    }

    public void OnDisabledRoad_Place()
    {
        if (S_Road_place)
            S_Road_place.enabled = false;
    }

    private void Awake()
    {
        S_Obj_place = FindObjectOfType<Spawner_Object_Grid>();
        S_Road_place = FindObjectOfType<Spawner_Road_Grid>();
    }
}
