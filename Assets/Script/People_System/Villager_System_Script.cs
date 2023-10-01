using System;

namespace GDD
{
    public class Villager_System_Script : People_System_Script
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            efficiency_boot = 1;
            base.Start();
        }

        public override void SetPeopleDatatoSavaData()
        {
            GM.gameInstance.villagerSaveDatas.Add(_peopleSaveData);
        }

        protected override void OnDead()
        {
            if (_constructionSystem == null)
            {
                HRM.RemovePeople<Villager_System_Script>(this, peopleJob);
            }
            else
            {
                print("death : " + (_peopleSaveData == null));
                _constructionSystem.OnRemovePeople<Villager_System_Script>(this, peopleJob);
            }
            
            GM.gameInstance.villagerSaveDatas.Remove(_peopleSaveData);
            
            base.OnDead();
        }
    }
}