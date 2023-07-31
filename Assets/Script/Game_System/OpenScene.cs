using UnityEngine;
using UnityEngine.SceneManagement;

namespace GDD
{
    public class OpenScene:MonoBehaviour
    {
        private GameManager GM = default;

        private void Start()
        {
            GM = FindObjectOfType<GameManager>();
        }
        public void NextScene()
        {
            
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex == (4))
            {
                SceneManager.LoadScene(0);
                GM.GetLevel = 0;
            }
            else
            {
                SceneManager.LoadScene(nextSceneIndex);
                GM.GetLevel = SceneManager.GetActiveScene().buildIndex + 1;
            }
        }
    }
}