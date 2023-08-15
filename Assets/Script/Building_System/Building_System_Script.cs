using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GDD
{
    public abstract class Building_System_Script : MonoBehaviour, IConstruction_System, IMenuInteractable
    {
        [SerializeField] protected BuildingType enum_buildingType;
        private BuildingSaveData _buildingSaveData = new BuildingSaveData();
        protected GameManager GM;
        
        public BuildingType _buildingType
        {
            get { return enum_buildingType; }
            set { enum_buildingType = value; }
        }

        public BuildingSaveData buildingSaveData
        {
            get => _buildingSaveData;
            set => _buildingSaveData = value;
        }

        public string name
        {
            get
            {
                if(_buildingSaveData == null)
                    _buildingSaveData = new BuildingSaveData();
                
                return _buildingSaveData.nameObject;
            }
            set
            {
                if(_buildingSaveData == null)
                    _buildingSaveData = new BuildingSaveData();
                
                _buildingSaveData.nameObject = value;
            }
        }

        public string path
        {
            get
            {
                if(_buildingSaveData == null)
                    _buildingSaveData = new BuildingSaveData();
                
                return _buildingSaveData.pathObject;
            }
            set
            {
                if(_buildingSaveData == null)
                    _buildingSaveData = new BuildingSaveData();
                
                _buildingSaveData.pathObject = value;
            }
        }

        public virtual List<Menu_Data> GetInteractAction()
        {
            List<Menu_Data> menuDatas = new List<Menu_Data>();
            menuDatas.Add(new Menu_Data("Interact", Resources.Load<Sprite>("Icon/build_"), Interact));
            menuDatas.Add(new Menu_Data("Change Enable Building", Resources.Load<Sprite>("Icon/construction"), ChangeEnableBuilding));
            menuDatas.Add(new Menu_Data("Remove", Resources.Load<Sprite>("Icon/account_tree"), RemoveBuilding));

            return menuDatas;
        }

        public virtual void Interact()
        {
            print("Innnnteerrraaacccttt : " + name);
        }

        public virtual void ChangeEnableBuilding()
        {
            print("OnChangeEnableBuilding : " + name);
        }

        public virtual void RemoveBuilding()
        {
            print("Remove : " + name);
        }

        public void OnGameLoad()
        {
            GM = FindObjectOfType<GameManager>();
            OnBeginPlace();
            transform.position = new Vector3(_buildingSaveData.Position.X, _buildingSaveData.Position.Y, _buildingSaveData.Position.Z);
            transform.eulerAngles = new Vector3(_buildingSaveData.Rotation.X, _buildingSaveData.Rotation.Y,_buildingSaveData.Rotation.Z);
            OnBeginPlace();
        }

        public virtual void OnPlaceBuilding()
        {
            GM = FindObjectOfType<GameManager>();
            OnBeginPlace();
            _buildingSaveData.Position = new Vector3D(transform.position.x, transform.position.y, transform.position.z);
            _buildingSaveData.Rotation = new Vector3D(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            _buildingSaveData.b_buildingtype = (byte)enum_buildingType;
            GM.gameInstance.buildingSystemScript.Add(_buildingSaveData);
            OnEndPlace();
        }

        public abstract void OnBeginPlace();
        public abstract void OnEndPlace();

        public void OnRemoveBuilding()
        {
            OnBeginRemove();
            
            //System script
            //-----------------------------------
            
            //-----------------------------------
            
            OnEndRemove();
        }

        public abstract void OnBeginRemove();
        public abstract void OnEndRemove();

        private void OnDestroy()
        {
            if(GM != null && _buildingSaveData != null)
                GM.gameInstance.buildingSystemScript.Remove(_buildingSaveData);
        }
    }
}
