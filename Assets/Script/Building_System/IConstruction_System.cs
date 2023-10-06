using UnityEngine;

namespace GDD
{
    public interface  IConstruction_System
    {
        //OnGameLoad
       public void OnGameLoad();
       
       //OnStart
       public void BeginStart();
       public void EndStart();
        
       //Resource usage
       public void Resource_usage();
       
       //Resource product
       public void Resource_product();
       
        //Place
       public void OnBeginPlace();
       public void OnEndPlace();
       
       //Check surrounding roads
       public void Check_Surround_Road();
       
       //Get Construction Active
       public bool Get_Construction_Active();
       
       //End
       public void OnBeginRemove();
       public void OnEndRemove();
       
       //Destroy
       public void OnDestroyBuilding();
       
       //Remove People
       public void OnRemovePeople<T>(People_System_Script _peopleSystemScript, PeopleJob job);
       
       //onEnable and onDisable
       public void OnEnableBuilding();
       public void OnDisableBuilding();
    }
}