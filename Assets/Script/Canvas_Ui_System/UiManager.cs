using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using TMPro;

namespace GDD
{
    public class UiManager:MonoBehaviour
    {
        [SerializeField]private TMP_InputField inputName;
        [SerializeField] private TextMeshProUGUI NameText;
        [SerializeField] private TextMeshProUGUI LevelText;
        [SerializeField] private TextMeshProUGUI EXPText;
        [SerializeField] private TextMeshProUGUI ScoreText;

        [SerializeField] private GameObject SaveLoadGameUi;

        private GameManager GM = default;
        private SaveManager SM = default;

        private void Start()
        {
            GM = GameManager.Instance;
            SM = SaveManager.Instance;
        }

        private void Update()
        {
            
        }
        public void SaveGame()
        {
            GameObject SLGUi = Instantiate(SaveLoadGameUi);
            SLGUi.GetComponent<SaveLoadUi>().IsOpenSaveUi = true;
        }

        public void LoadGame()
        {
            GameObject SLGUi = Instantiate(SaveLoadGameUi);
            SLGUi.GetComponent<SaveLoadUi>().IsOpenSaveUi = false;
        }
    }
}