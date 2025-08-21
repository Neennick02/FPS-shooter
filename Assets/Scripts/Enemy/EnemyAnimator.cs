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
            animator.SetBool("IsMoving", true);
            MoveGunToIdlePos();
        }
        else if(stateMachine.activeState is AttackState)
        {

            animator.SetBool("enemyFound", true);
            isAttacking = true;
        }

        if(stateMachine.activeState is SearchState)
        {
            MoveGunToIdlePos();
            animator.SetBool("enemyFound", false);
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

        gun.AddComponent<Rigidbody>();
        gun.AddComponent<BoxCollider>();
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

    void MoveGunToIdlePos()
    {
        enemy.gunObject.transform.localPosition = Vector3.Lerp(
               enemy.gunObject.transform.localPosition,
               enemy.idlePos.localPosition,
               Time.deltaTime * enemy.rotationSpeed);
    }

    public void SetIsMoving(bool active)
    {
        animator.SetBool("IsMoving", active);
    }

}