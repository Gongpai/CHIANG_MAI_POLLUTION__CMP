using UnityEngine;
using UnityEngine.Pool;

namespace GDD
{
    public class People_Object_Pool_Script : MonoBehaviour, IPeopleObjectPool
    {
        [SerializeField]protected int maxPoolSize = 10;
        [SerializeField]protected int stackDefaultCapacity = 10;
        protected HumanResourceManager HRM;
        private IObjectPool<People_System_Script> _people_pool;

        public IObjectPool<People_System_Script> peoplePool
        {
            get
            {
                if (_people_pool == null)
                    _people_pool = new ObjectPool<People_System_Script>(
                        CreatedPoolPeople,
                        OnTakeFromPool,
                        OnReturnedToPool,
                        OnDestroyPoolObject,
                        true,
                        stackDefaultCapacity,
                        maxPoolSize
                        );

                return _people_pool;
            }
        }

        public People_System_Script CreatedPoolPeople()
        {
            People_System_Script _peopleSystemScript = CreatePeople();
            _peopleSystemScript.Pool = _people_pool;
            return _peopleSystemScript;
        }

        public virtual People_System_Script CreatePeople()
        {
            GameObject _people = new GameObject();
            _people.name = "people";
            _people.transform.parent = gameObject.transform;
            return _people.AddComponent<People_System_Script>();
        }

        public virtual void OnReturnedToPool(People_System_Script _peopleSystemScript)
        {
            _peopleSystemScript.gameObject.SetActive(false);
        }

        public virtual void OnTakeFromPool(People_System_Script _peopleSystemScript)
        {
            _peopleSystemScript.gameObject.SetActive(true);
        }

        public virtual void OnDestroyPoolObject(People_System_Script _peopleSystemScript)
        {
            Destroy(_peopleSystemScript.gameObject);
        }

        public virtual People_System_Script Spawn(PeopleSystemSaveData _saveData = null)
        {
            People_System_Script _peopleSystemScript = peoplePool.Get();

            if (_saveData != null)
            {
                _peopleSystemScript.saveData = _saveData;
            }
            else
            {
                _peopleSystemScript.SetPeopleDatatoSavaData();
            }
                

            return _peopleSystemScript;
        }
    }
}