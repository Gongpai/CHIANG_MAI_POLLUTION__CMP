using UnityEngine;
using UnityEngine.SceneManagement;

public class Camp : MonoBehaviour
{
    public string sceneToLoad; // ชื่อซีนที่คุณต้องการโหลด

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.UnloadSceneAsync(sceneToLoad);
        }
    }
}
