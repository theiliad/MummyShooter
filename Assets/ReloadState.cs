using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadState : StateMachineBehaviour {
	public float reloadTime = 1;
	bool hasReloaded = false;

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		hasReloaded = false;
	}

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (hasReloaded) return;

		if (stateInfo.normalizedTime >= reloadTime) {
			animator.GetComponent<Weapon>().reload();
			hasReloaded = true;
		}
	}

	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		hasReloaded = false;
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
