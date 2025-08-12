using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] public Animator animator;
    StateMachine stateMachine;
    Enemy enemyScript;
    [SerializeField] public Transform aimingPos;
    [SerializeField] Transform idlePos;
    [SerializeField] Transform gunPos;
    [SerializeField] GameObject gun;
    [SerializeField] public Transform target;
    Camera player;
    Enemy enemy;
    Rigidbody[] bodies;
    float aimSpeed = 5;
    bool isAttacking;
    private void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        enemyScript = GetComponent<Enemy>();
        player = Camera.main;
        enemy = GetComponent<Enemy>();
        DeactivateRagdoll();
    }

    private void Update()
    {
        if (stateMachine.activeState is PatrolState)
        {
            Debug.Log("Patrolstate is active");
            animator.SetBool("IsMoving", true);
        }
        else if(stateMachine.activeState is AttackState)
        {
            animator.SetBool("enemyFound", true);
            isAttacking = true;
        }

        if(stateMachine.activeState is SearchState)
        {
            animator.SetBool("enemyFound", false);
        }


        if (isAttacking)
        {
            var target = isAttacking ? aimingPos : idlePos;
            gun.transform.localPosition = Vector3.Lerp(
                gun.transform.localPosition,
                target.localPosition,
                Time.deltaTime * aimSpeed
            );
            gun.transform.localRotation = Quaternion.Lerp(
                gun.transform.localRotation,
                target.localRotation,
                Time.deltaTime * aimSpeed
            );
        }
    }

    public void ActivateRagdoll()
    {
        animator.enabled = false;

        bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody body in bodies)
        {
            body.isKinematic = false;
        }
    }

    public void DeactivateRagdoll()
    {
        animator.enabled = true;
        
        bodies = GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody body in bodies)
        {
            body.isKinematic = true;
        }
    }

}