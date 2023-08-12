using UnityEngine;

namespace GDD
{
    interface  IConstruction_System
    {
        //OnGameLoad
       public void OnGameLoad();
        
        //Place
       public void OnBeginPlace();
       public void OnEndPlace();
       
       //End
       public void OnBeginRemove();
       public void OnEndRemove();
    }
}