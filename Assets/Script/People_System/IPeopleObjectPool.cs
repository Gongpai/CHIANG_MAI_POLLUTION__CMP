using UnityEngine;

namespace GDD
{
    public interface IPeopleObjectPool
    {
        People_System_Script CreatedPoolPeople();
        People_System_Script CreatePeople();
        void OnReturnedToPool(People_System_Script _peopleSystemScript);
        void OnTakeFromPool(People_System_Script _peopleSystemScript);
        void OnDestroyPoolObject(People_System_Script _peopleSystemScript);
        People_System_Script Spawn(PeopleSystemSaveData _saveData);
    }
}