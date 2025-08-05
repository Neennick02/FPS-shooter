using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;
    StateMachine stateMachine;
    private void Start()
    {
        stateMachine = GetComponent<StateMachine>();
    }

    private void Update()
    {
        if (stateMachine.activeState is PatrolState)
        {
            Debug.Log("Patrolstate is active");

        }
    }

    public void ActivateRagdoll()
    {
        animator.enabled = false;
    }

}