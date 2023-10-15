using UnityEngine;
using UnityEngine.SceneManagement;

namespace GDD
{
    public class OpenScene:MonoBehaviour
    {
        private GameManager GM = default;

        private void Start()
        {
            GM = GameManager.Instance;
        }
        public void NextScene()
        {
            
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex == (4))
            {
                SceneManager.LoadScene(1);
            }
            else
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
        }
    }
}