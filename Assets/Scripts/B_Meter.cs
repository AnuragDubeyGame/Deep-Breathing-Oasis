using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class B_Meter : MonoBehaviour
{
    public event Action OnProgressBarFilled;
    [SerializeField] private Image b_In, b_Hd, b_Ot, p_Bar;
    [SerializeField] private TextMeshProUGUI level_Name_Text, level_No_Text;
    [SerializeField] public float b_In_Duration, b_Hd_Duration, b_Ot_Duration, delayBetweenLvlStart;
    [SerializeField] private int noOfTimesToRepeat;
    [SerializeField] private int maxlevel;
    
    public List<Levels> LevelList = new List<Levels>();
    private int currentLevel;
    private float total_Duration, oneRound_Duration;
    private float timeElapsed = 0;
    public B_State rec_b_state;

    void Start()
    {
        currentLevel = 0;
        level_Name_Text.text = LevelList[currentLevel].LvlName;
        level_No_Text.text = currentLevel.ToString() + ". ";
        rec_b_state = B_State.Idle;
        StartCoroutine(StartBreathPattern(noOfTimesToRepeat));
    }

    public IEnumerator StartBreathPattern(int noOfTimes)
    {
        oneRound_Duration = b_In_Duration + b_Hd_Duration + b_Ot_Duration + delayBetweenLvlStart;
        total_Duration = oneRound_Duration * noOfTimes;

        StartCoroutine(StartP_Bar(total_Duration));

        for (int i = 0; i < noOfTimes; i++)
        {
            StartCoroutine(StartB_Meter());
            yield return new WaitForSeconds(oneRound_Duration);
        }
    }

    public IEnumerator StartP_Bar(float duration)
    {
        float te = 0;
        while (te < duration)
        {
            float t = te / duration;
            p_Bar.fillAmount = Mathf.Lerp(0, 1, t);
            yield return null;
            te += Time.deltaTime;
        }
        OnProgressBarFilled?.Invoke();
    }
    public void resetMeterBar()
    {
        p_Bar.fillAmount = 0f;
        b_In.fillAmount = 0f;
        b_Hd.fillAmount = 0f;
        b_Ot.fillAmount = 0f;
        timeElapsed = 0f;
        rec_b_state = B_State.Idle;
        if(currentLevel < maxlevel)
        {
            currentLevel++;
            level_Name_Text.text = LevelList[currentLevel].LvlName;
            level_No_Text.text = currentLevel.ToString() + ". ";
        }
        else
        {
            print("Game Ended!");
            // Show Game End Screen But first Disable 'NextLevel' Game Logic.
        }
        b_In_Duration = LevelList[currentLevel].b_In_Duration;
        b_Hd_Duration = LevelList[currentLevel].b_Hd_Duration;
        b_Ot_Duration = LevelList[currentLevel].b_Ot_Duration;
        delayBetweenLvlStart = LevelList[currentLevel].delayBetweenLvlStart;
        StartCoroutine(StartBreathPattern(LevelList[currentLevel].noOfTimesToRepeat));
    }
    void UpdateLevelName()
    {

    }
    public IEnumerator StartB_Meter()
    {
        yield return new WaitForSeconds(delayBetweenLvlStart);
        rec_b_state = B_State.In;
        while (timeElapsed < b_In_Duration)
        {
            float t = timeElapsed / b_In_Duration;
            b_In.fillAmount = Mathf.Lerp(0, 1, t);
            yield return null;
            timeElapsed += Time.deltaTime;
        }

        timeElapsed = 0;
        rec_b_state = B_State.Hold;
        while (timeElapsed < b_Hd_Duration)
        {
            float t = timeElapsed / b_Hd_Duration;
            b_Hd.fillAmount = Mathf.Lerp(0, 1, t);
            yield return null;
            timeElapsed += Time.deltaTime;
        }
        timeElapsed = 0;
        rec_b_state = B_State.Out;
        while (timeElapsed < b_Ot_Duration)
        {
            float t = timeElapsed / b_Ot_Duration;
            b_Ot.fillAmount = Mathf.Lerp(0, 1, t);
            yield return null;
            timeElapsed += Time.deltaTime;
        }
        ResetB_Meter();
        timeElapsed = 0;
    }

    public void ResetB_Meter()
    {
        b_In.fillAmount = 0;
        b_Hd.fillAmount = 0;
        b_Ot.fillAmount = 0;
    }


}
[System.Serializable]
public struct Levels
{
    public string LvlName;
    public float b_In_Duration, b_Hd_Duration, b_Ot_Duration, delayBetweenLvlStart;
    public int noOfTimesToRepeat;
}