using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    public EnemyObject _enemyObject;
    [SerializeField] NavMeshAgent agent;
    StateMachine stateMachine;
    GameObject player;
    Vector3 lastKnownPlayPos;
    public NavMeshAgent Agent { get => agent; }  
    public GameObject Player { get => player; }
    public Vector3 LastKnowsPlayerPos { get => lastKnownPlayPos; set => lastKnownPlayPos = value; }
    public EnemyPath path;

    [SerializeField] string currentState;
    public EnemyAnimator animatorScript;
    [SerializeField] public GameObject gunObject;
    [SerializeField] public Transform idlePos;
    public Transform barrel;

    public Transform aimTarget;

    private void Start()
    {
        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();
        stateMachine.Initialise();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        CanSeePlayer();
        currentState = stateMachine.activeState.ToString();
    }

    public bool CanSeePlayer()
    {
        if(player != null)
        {
            //is player close enough
            if(Vector3.Distance(transform.position, player.transform.position) < _enemyObject.SpottingDistance)
            {
                Vector3 origin = transform.position + Vector3.up * _enemyObject.EyeHeight;
                Vector3 targetDir = (player.transform.position - origin).normalized;

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
                        if(hitInfo.transform.gameObject == player && hitInfo.distance < _enemyObject.SpottingDistance)
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
        if(player != null)
        {
            if(Vector3.Distance(transform.position, player.transform.position) < _enemyObject.HearingDistance)
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
