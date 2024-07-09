using UnityEngine;

public class PlayerRunBehaviour : StateMachineBehaviour
{

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<GameVFXManager>()?.Update_FootStep(true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<GameVFXManager>()?.Update_FootStep(false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void onstatemove(animator animator, animatorstateinfo stateinfo, int layerindex)
    //{
    //    implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
