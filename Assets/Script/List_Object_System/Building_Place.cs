using System.Collections;
using System.Collections.Generic;
using GDD;
using Unity.VisualScripting;
using UnityEngine;

public class Building_place : MonoBehaviour
{
    private Spawner_Object_Grid S_Obj_place = FindObjectOfType<Spawner_Object_Grid>();

    public void Select_Building(string assetPath)
    {
        var asset = Resources.Load(assetPath);
        S_Obj_place.ObjectToSapwn = asset.GameObject();
    }
}
