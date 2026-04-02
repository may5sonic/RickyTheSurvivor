using UnityEngine;

public class LeftHandAttach : MonoBehaviour
{
   public Animator animator;
    public Transform leftHandTarget;

    [Range(0,1)] public float ikWeight = 1f;

    void OnAnimatorIK(int layerIndex)
    {
        if (animator && leftHandTarget != null)
        {
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikWeight);

            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandTarget.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTarget.rotation);
        }
    }
}