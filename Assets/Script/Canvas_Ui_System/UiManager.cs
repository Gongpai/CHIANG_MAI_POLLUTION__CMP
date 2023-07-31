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
            GM = FindObjectOfType<GameManager>();
            SM = FindObjectOfType<SaveManager>();
        }

        private void Update()
        {
            NameText.text = "Name : " + GM.GetName;
            LevelText.text = "Level : " + GM.GetLevel.ToString();
            EXPText.text = "EXP : " + GM.GetEXP.ToString();
            ScoreText.text = "Score : " + GM.GetScore.ToString();
        }

        public void SetName(string name)
        {
            
            GM.GetName = name;
        }

        public void AddEXP()
        {
            GM.GetEXP += 100;
        }

        public void AddScore()
        {
            GM.GetScore += 999;
        }

        public void SaveGame()
        {
            GameObject SLGUi = Instantiate(SaveLoadGameUi);
            SLGUi.GetComponent<SaveLoadUi>().OnOpenUi(true);
        }

        public void LoadGame()
        {
            GameObject SLGUi = Instantiate(SaveLoadGameUi);
            SLGUi.GetComponent<SaveLoadUi>().OnOpenUi(false);
        }
    }
}