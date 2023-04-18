using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class B_Meter : MonoBehaviour
{
    [SerializeField] private Image b_In, b_Hd, b_Ot, p_Bar;
    [SerializeField] public float b_In_Duration, b_Hd_Duration, b_Ot_Duration, delayBetweenLvlStart;

    private float total_Duration, oneRound_Duration;
    private float timeElapsed = 0;
    public B_State rec_b_state;

    void Start()
    {
        rec_b_state = B_State.Idle;
        StartCoroutine(StartBreathPattern(3));
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
