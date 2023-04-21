using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public enum BreathPhase { breathIn, breathHold, breathOut }

public class B_Indicator : MonoBehaviour
{
    [SerializeField] private Color b_Idle_Colour, b_In_Colour, b_Hd_Colour, b_Ot_Colour;
    [SerializeField] private float minSize, maxSize;
    [SerializeField] private GameObject LevelCompleteMenu;

    private float CurrentScore;
    [SerializeField] private TextMeshProUGUI ScoreText, Menu_ScoreText;
     
    private float GrowthSpeed, ShrinkSpeed;
    private B_Meter b_meter;
    private SpriteRenderer sr;
    public B_State b_state;
    private float currentSize;
    public bool isLevelFinished = false;


    void Start()
    {
        LevelCompleteMenu.SetActive(false);
        sr = GetComponent<SpriteRenderer>();
        b_meter = FindObjectOfType<B_Meter>();
        b_meter.OnProgressBarFilled += B_meter_OnProgressBarFilled;
        b_state = B_State.Idle;
        CurrentScore = 0f;
        currentSize = minSize;
        transform.localScale = new Vector2(currentSize, currentSize);
        GrowthSpeed = (maxSize - minSize) / b_meter.b_In_Duration;
        ShrinkSpeed = (maxSize - minSize) / b_meter.b_Ot_Duration;
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
        if (isLevelFinished) return;
        if(b_state == b_meter.rec_b_state)
        {
            CurrentScore += 25 * Time.deltaTime;
            UpdateScore(ScoreText, CurrentScore);
        }
        else
        {
            CurrentScore -= 40 * Time.deltaTime;
            UpdateScore(ScoreText, CurrentScore);
        }
        if(Input.GetKey(KeyCode.I))
        {
            b_state = B_State.In;
            //sr.color = b_In_Colour;

            currentSize = transform.localScale.x + GrowthSpeed * Time.deltaTime;
            currentSize = Mathf.Clamp(currentSize, minSize, maxSize);
            transform.localScale = new Vector3(currentSize, currentSize);

        }
        else if (Input.GetKey(KeyCode.H))
        {
            b_state = B_State.Hold;
            //sr.color = b_Hd_Colour;
            currentSize = transform.localScale.x + 0 * Time.deltaTime;
            currentSize = Mathf.Clamp(currentSize, minSize, maxSize);
            transform.localScale = new Vector3(currentSize, currentSize);
        }
        else if (Input.GetKey(KeyCode.O))
        {
            b_state = B_State.Out;
            //sr.color = b_Ot_Colour;
            currentSize = transform.localScale.x - ShrinkSpeed * Time.deltaTime;
            currentSize = Mathf.Clamp(currentSize, minSize, maxSize);
            transform.localScale = new Vector3(currentSize, currentSize);
        }
        else
        {
            b_state = B_State.Idle;
            //sr.color = b_Idle_Colour;
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

        //Update Level Name
        //Add Array of Levels
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