using Unity.VisualScripting;
using UnityEngine;
using static AnimatorParameters;

public class IdleAnimationState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(IsAttack);
        animator.SetInteger(AttackId, 0);
        animator.ResetTrigger(IsAttack);
        animator.ResetTrigger(IsAttackEnd);
        animator.ResetTrigger(IsRoll);
        animator.ResetTrigger(IsRollEnd);
    }
}
