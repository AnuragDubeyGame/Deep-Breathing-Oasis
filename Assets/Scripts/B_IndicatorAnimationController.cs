using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_IndicatorAnimationController : MonoBehaviour
{
    private Animator animator;
    private B_Indicator indicatorPhase;
    [SerializeField] private string breathInPhase,breathHoldPhase,breathOutPhase;
    private string idlePhase;

    private void Start()
    {
        animator = GetComponent<Animator>();
        indicatorPhase = GetComponent<B_Indicator>();
    }

    private void Update()
    {
        switch (indicatorPhase.b_state) 
        {
            case B_State.Idle:
                animator.SetBool(idlePhase, true);
                animator.SetBool(breathInPhase, false);
                animator.SetBool(breathHoldPhase, false);
                animator.SetBool(breathOutPhase, false);
                break;
            case B_State.In:
                animator.SetBool(idlePhase, false);
                animator.SetBool(breathInPhase, true);
                animator.SetBool(breathHoldPhase, false);
                animator.SetBool(breathOutPhase, false);
                break;
            case B_State.Hold:
                animator.SetBool(idlePhase, false);
                animator.SetBool(breathInPhase, false);
                animator.SetBool(breathHoldPhase, true);
                animator.SetBool(breathOutPhase, false);
                break;
            case B_State.Out:
                animator.SetBool(idlePhase, false);
                animator.SetBool(breathInPhase, false);
                animator.SetBool(breathHoldPhase, false);
                animator.SetBool(breathOutPhase, true);
                break;
           
        }
    }
}
