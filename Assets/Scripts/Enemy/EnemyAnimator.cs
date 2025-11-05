using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyAnimator : MonoBehaviour
{
    [SerializeField] public Animator animator;
    StateMachine stateMachine;
    Enemy enemyScript;
    [SerializeField] private Rig _weaponAimRig;

    Camera player;
    Enemy enemy;
    Rigidbody[] bodies;
    private bool _lastAimingState;


    private bool _isAiming;
    private Coroutine _moveGunCoroutine = null;
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
        /*if (stateMachine.activeState is PatrolState)
        {
            _isAiming = false;
            
            animator.SetBool("IsMoving", true);
        }
        else if(stateMachine.activeState is AttackState)
        {

            animator.SetBool("enemyFound", true);
            _isAiming = true;
            
        }

        if (stateMachine.activeState is SearchState)
        {
            _isAiming = false;
            
            animator.SetBool("enemyFound", false);
        }*/
        bool newAimingState = stateMachine.activeState is AttackState;

        if (newAimingState != _lastAimingState)
        {
            _isAiming = newAimingState;
            if (_moveGunCoroutine != null) 
                _moveGunCoroutine = null;

            _moveGunCoroutine = StartCoroutine(MoveGun());
            _lastAimingState = newAimingState;  
        }

        animator.SetBool("enemyFound", stateMachine.activeState is AttackState);
        animator.SetBool("IsMoving", stateMachine.activeState is PatrolState);



    }

    public void ActivateRagdoll()
    {
        animator.enabled = false;

        bodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody body in bodies)
        {
            body.isKinematic = false;
        }

       /* gun.AddComponent<Rigidbody>();
        gun.AddComponent<BoxCollider>();*/
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

    IEnumerator MoveGun()
    {
        if (_isAiming)
        {
            while(_weaponAimRig.weight < 1)
            {
                _weaponAimRig.weight++;
                yield return null;
            }
        }
        else
        {
            while(_weaponAimRig.weight > 0)
            {
                _weaponAimRig.weight--;
                yield return null;
            }
        }
    }

    public void SetIsMoving(bool active)
    {
        animator.SetBool("IsMoving", active);
    }
}