using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class TimeCounter : MonoBehaviour
{
    [Header("Componant")] public TextMeshProUGUI TimerText;

    [Header("TimerSetting")] 
    public float CurrentTime;
    public bool CountDown;

    [Header("LimitSetting")] 
    public bool HasLimit;
    public float TimeLimit;

    [Header("TimeForm")] 
    public bool HasForm;
    public TimerForm Form;
    private Dictionary<TimerForm, string> timeform = new Dictionary<TimerForm, string>();

    public int t=1;
    public GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        CurrentTime = gameData.playerTime;
        timeform.Add(TimerForm.one,"0");
        timeform.Add(TimerForm.two,"0.0");
        timeform.Add(TimerForm.three,"0.00");
    }

    // Update is called once per frame
    void Update()
    {
        
        
        CurrentTime = CountDown ? CurrentTime -= (Time.deltaTime*t) : CurrentTime += (Time.deltaTime*t);
        if ((CurrentTime <= 0.01f ) && (CurrentTime >0 ))
        {
            timeend();
            CurrentTime = 0;
            SceneManager.LoadScene("Gameover", LoadSceneMode.Additive);
            TimerText.color = Color.black;
            t = 0;
        }
        
        if (HasLimit &&((CountDown && CurrentTime <= TimeLimit) || (!CountDown && CurrentTime >= TimeLimit)))
        {
            CurrentTime = TimeLimit;
            SetTimerText();
            TimerText.color = Color.black;
            enabled = false;
        }
        
        SetTimerText();
        
        
    }

    private void SetTimerText()
    {
        TimerText.text = HasForm ? CurrentTime.ToString(timeform[Form]) : CurrentTime.ToString("0.00");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SetTimerText();
            TimerText.color = Color.yellow;
            Debug.Log("enter dust");
            t = 2;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SetTimerText();
            TimerText.color = Color.white;
            Debug.Log("exit dust");
            t = 1;
        }
    }

    public void pausetime()
    {
        t = 0;
    }
    public void playtime()
    {
        t = 1;
    }
    [SerializeField] protected UnityEvent<GameObject> m_OnTimeEnd = new();
    public void timeend()
    {
        m_OnTimeEnd.Invoke(gameObject);
    }
}

public enum TimerForm
{
    one,
    two,
    three
}