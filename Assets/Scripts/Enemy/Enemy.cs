using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    public EnemyObject _enemyObject;

    public NavMeshAgent Agent;
    private StateMachine _stateMachine;
    private Transform _player;
    Vector3 lastKnownPlayPos;
   // public NavMeshAgent Agent { get => _agent; }  
    public Transform Player { get => _player; }
    //public Vector3 LastKnowsPlayerPos { get => lastKnownPlayPos; set => lastKnownPlayPos = value; }
    [SerializeField] private EnemyPath path;

    [SerializeField] string currentState;

    public EnemyAnimator AnimatorScript;

    [SerializeField] private GameObject gunObject;
    public Transform barrel;
    private void Start()
    {
        _stateMachine = GetComponent<StateMachine>();
        _stateMachine.Initialise();
        _player = InputManager.Instance.transform;
    }

    private void Update()
    {
        CanSeePlayer();
        currentState = _stateMachine.activeState.ToString();
    }

    public bool CanSeePlayer()
    {
        if(_player != null)
        {
            //is player close enough
            if(Vector3.Distance(transform.position, _player.position) < _enemyObject.SpottingDistance)
            {
                Vector3 origin = transform.position + Vector3.up * _enemyObject.EyeHeight;
                Vector3 targetDir = (_player.position - origin).normalized;

                float angleToPlayer = Vector3.Angle(targetDir, transform.forward);
                targetDir.Normalize();

                //draw line in sceneview
                Debug.DrawLine(
                           transform.position + (Vector3.up * _enemyObject.EyeHeight),
                           transform.position + (Vector3.up * _enemyObject.EyeHeight) + targetDir * _enemyObject.SpottingDistance,
                           Color.red);

                //checkt if player is in FOV
                if (angleToPlayer <= _enemyObject.Fov)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * _enemyObject.EyeHeight), targetDir);
                    RaycastHit hitInfo = new RaycastHit();
                    if(Physics.Raycast(ray, out hitInfo, _enemyObject.SpottingDistance))
                    {
                        //checkt if object is player 
                        if(hitInfo.transform.gameObject == _player && hitInfo.distance < _enemyObject.SpottingDistance)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public bool CanHearPlayer()
    {
        if(_player != null)
        {
            if(Vector3.Distance(transform.position, _player.position) < _enemyObject.HearingDistance)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {/*
        Gizmos.color = Color.red;
        Gizmos.DrawCube(Agent.destination, new Vector3(2, 2, 2));*/
    }
}
