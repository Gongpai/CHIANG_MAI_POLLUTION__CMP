using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneSystem : MonoBehaviour
{
    [SerializeField] private GameObject LoadingScreen;
    [SerializeField] private UnityEvent _loading_event;
    [SerializeField] private Image LoadingBarfill;

    private float progressValue;
    string Scene_ID;
    private Animator animator;
    AsyncOperation operation;
    private UnityAction _action_loading;
    private UnityAction _action_loaded;

    public UnityAction action_loading_scane
    {
        get => _action_loading;
        set => _action_loading = value;
    }
    
    public UnityAction action_loaded_scane
    {
        get => _action_loaded;
        set => _action_loaded = value;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void LoadScene(string SceneID)
    {
        LoadingScreen.SetActive(true);
        Scene_ID = SceneID;
        animator.SetBool("IsIn", true);
        animator.SetBool("IsOut", false);
    }

    public void EndLoadScene()
    {
        operation.allowSceneActivation = true;
    }

    private void OpenScene()
    {
        animator.SetBool("IsIn", false);
        animator.SetBool("IsOut", true);
        
        if(_action_loaded != null)
            _action_loaded.Invoke();
    }

    public void StartLoading()
    {
        if(_action_loading != null)
            _action_loading.Invoke();
        
        if(_loading_event != null)
            _loading_event.Invoke();
        
        StartCoroutine(LoadSceneAsync(Scene_ID));
    }

    IEnumerator LoadSceneAsync(string SceneID)
    {
        operation = SceneManager.LoadSceneAsync(SceneID);
        operation.allowSceneActivation = false;

        print("PRRR : " + operation);

        while (!operation.isDone)
        {
            progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBarfill.GetComponent<Image>().fillAmount = progressValue;
            print("Loading : " + operation.progress + " ----------------------------------------------");
            yield return null;
            if((operation.progress / 0.9f) >= 1)
                OpenScene();
            //operation.allowSceneActivation = true;
        }
    }
}
