namespace GDD
{
    public class Worker_System_Script : People_System_Script
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            efficiency_boot = 1.5f;
            base.Start();
        }
        
        public override void SetPeopleDatatoSavaData()
        {
            base.SetPeopleDatatoSavaData();
            
            GM.gameInstance.workerSaveDatas.Add(_peopleSaveData);
        }

        protected override void OnDead()
        {
            if (_constructionSystem == null)
            {
                HRM.RemovePeople<Worker_System_Script>(this, peopleJob);
            }
            else
            {
                print("death");
                _constructionSystem.OnRemovePeople<Worker_System_Script>(this, peopleJob);
            }
            
            GM.gameInstance.workerSaveDatas.Remove(_peopleSaveData);
            
            base.OnDead();
        }
    }
}