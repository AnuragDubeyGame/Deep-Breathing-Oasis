using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditorInternal.VersionControl.ListControl;

public class B_Indicator : MonoBehaviour
{
    [SerializeField] private Color b_Idle_Colour, b_In_Colour, b_Hd_Colour, b_Ot_Colour;
    [SerializeField] private float growSpeed, minSize, maxSize;

    private float GrowthSpeed, HoldSpeed, ShrinkSpeed;

    private B_Meter b_meter;
    private SpriteRenderer sr;
    public B_State b_state;
    private float elapsedTime = 0f;
    private float currentSize;


    public enum B_State
    {
        Idle,
        In,
        Hold,
        Out
    }
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        b_meter = FindObjectOfType<B_Meter>();
        b_state = B_State.Idle;
        currentSize = minSize;
        transform.localScale = new Vector2(currentSize, currentSize);
        GrowthSpeed = (maxSize - minSize) / b_meter.b_In_Duration;
        ShrinkSpeed = (maxSize - minSize) / b_meter.b_Ot_Duration;
        print(GrowthSpeed);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.I))
        {
            b_state = B_State.In;
            sr.color = b_In_Colour;

            currentSize = transform.localScale.x + growSpeed * Time.deltaTime;
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

    
}
