using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum BreathPhase { breathIn, breathHold, breathOut }

public class B_Indicator : MonoBehaviour
{
    [SerializeField] private Color b_Idle_Colour, b_In_Colour, b_Hd_Colour, b_Ot_Colour;
    [SerializeField] private float minSize, maxSize;
    [SerializeField] private GameObject LevelCompleteMenu, Timer_text;

    [SerializeField] private List<GameObject> UIs = new List<GameObject>();

    private float CurrentScore;
    [SerializeField] private TextMeshProUGUI ScoreText, Menu_ScoreText;
     
    private float GrowthSpeed, ShrinkSpeed;
    private B_Meter b_meter;
    private SpriteRenderer sr;
    public B_State b_state;
    private float currentSize;
    public bool isLevelFinished = false, isMenu = true;
    [SerializeField] private float WaitTimeBeforeLevelSart;

    void Start()
    {
        LevelCompleteMenu.SetActive(false);
        sr = GetComponent<SpriteRenderer>();
        b_meter = FindObjectOfType<B_Meter>();
        b_meter.OnProgressBarFilled += B_meter_OnProgressBarFilled;
        currentSize = minSize;
        transform.localScale = new Vector2(currentSize, currentSize);
        GrowthSpeed = (maxSize - minSize) / b_meter.LevelList[b_meter.currentLevel].b_In_Duration;
        ShrinkSpeed = (maxSize - minSize) / b_meter.LevelList[b_meter.currentLevel].b_Ot_Duration;
    }
    public void startLevels()
    {
        isMenu = false;
        CurrentScore = 0f;
        b_state = B_State.Idle;
    }
    public void StartGame()
    {
        StartCoroutine(StartGame_CR());
    }
    public IEnumerator StartGame_CR()
    {
        Timer_text.SetActive(true);
        float et = WaitTimeBeforeLevelSart;
        while (et >= 0)
        {
            et -= Time.deltaTime;
            Timer_text.GetComponent<TextMeshProUGUI>().text = et.ToString("F0");
            yield return null;
        }
        Timer_text.SetActive(false);
        foreach (var item in UIs)
        {
            item.SetActive(true);
        }
        startLevels();
        b_meter.StartLevels();
    }
    private void B_meter_OnProgressBarFilled()
    {
        isLevelFinished = true;
        LevelCompleteMenu.SetActive(true);
        Menu_ScoreText.text = CurrentScore.ToString("F1");
        resetGameLevelState();
    }

    private void Update()
    {
        if (isMenu) return;
        if (isLevelFinished) return;
        if(b_state == b_meter.rec_b_state)
        {
            CurrentScore += 25 * Time.deltaTime;
            ScoreText.color = Color.green;
            UpdateScore(ScoreText, CurrentScore);
        }
        else
        {
            CurrentScore -= 40 * Time.deltaTime;
            ScoreText.color = Color.red;
            UpdateScore(ScoreText, CurrentScore);
        }
        if(Input.GetKey(KeyCode.I))
        {
            b_state = B_State.In;
            sr.color = b_In_Colour;

            currentSize = transform.localScale.x + GrowthSpeed * Time.deltaTime;
            currentSize = Mathf.Clamp(currentSize, minSize, maxSize);
            transform.localScale = new Vector3(currentSize, currentSize);

        }
        else if (Input.GetKey(KeyCode.H))
        {
            b_state = B_State.Hold;
            sr.color = b_Hd_Colour;
            currentSize = transform.localScale.x + 0 * Time.deltaTime;
            currentSize = Mathf.Clamp(currentSize, minSize, maxSize);
            transform.localScale = new Vector3(currentSize, currentSize);
        }
        else if (Input.GetKey(KeyCode.O))
        {
            b_state = B_State.Out;
            sr.color = b_Ot_Colour;
            currentSize = transform.localScale.x - ShrinkSpeed * Time.deltaTime;
            currentSize = Mathf.Clamp(currentSize, minSize, maxSize);
            transform.localScale = new Vector3(currentSize, currentSize);
        }
        else
        {
            b_state = B_State.Idle;
            sr.color = b_Idle_Colour;
        }

        Debug.Log("Current State: " + b_state);
    }

    private void UpdateScore(TextMeshProUGUI text,float score)
    {
        text.text = score.ToString("F1");
    }
    private void resetGameLevelState()
    {
        transform.localScale = new Vector3(minSize, minSize, minSize);
        b_state = B_State.Idle;
        sr.color = b_Idle_Colour;
    }
    public void PlayNextLevel()
    {
        currentSize = minSize;
        CurrentScore = 0;
        UpdateScore(ScoreText,0);
        b_meter.resetMeterBar();
        LevelCompleteMenu.SetActive(false);
        GrowthSpeed = (maxSize - minSize) / b_meter.b_In_Duration;
        ShrinkSpeed = (maxSize - minSize) / b_meter.b_Ot_Duration;
        isLevelFinished = false;
    }
    public void Quit()
    {
        Application.Quit();
    }
}

public enum B_State
{
    Idle,
    In,
    Hold,
    Out
}