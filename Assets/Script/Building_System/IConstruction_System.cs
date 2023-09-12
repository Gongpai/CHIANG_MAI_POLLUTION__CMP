using UnityEngine;

namespace GDD
{
    interface  IConstruction_System
    {
        //OnGameLoad
       public void OnGameLoad();
       
       //OnStart
       public void BeginStart();
       public void EndStart();
        
       //Resource usage
       public void Resource_usage();
       
        //Place
       public void OnBeginPlace();
       public void OnEndPlace();
       
       //End
       public void OnBeginRemove();
       public void OnEndRemove();
       
       //Destroy
       public void OnDestroyBuilding();
       
       //onEnable and onDisable
       public void OnEnableBuilding();
       public void OnDisableBuilding();
    }
}