using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    /*[Header("Movement")]
    [SerializeField] float rotationSpeed = 3;
    [SerializeField] float movementSpeed = 3;

    [Header("Attack config")]
    [SerializeField] protected float attackInterval = 1f;
    [SerializeField] protected float SpottingDistance = 6f;
    [SerializeField] protected float InRangeDistance = 2f;
    [SerializeField] protected int damageAmount = 10;

    [Header("Others")]
    [SerializeField] States startState;
    

    [Tooltip("Zorg dat dit object geen child is. Vul een leeg GameObject met meerdere Transforms, dit worden de wayPoints")]
    [SerializeField] GameObject wayPointsObject;

    protected float attackTimer = 0;
    protected Transform[] wayPoints;
    protected float startSpeed;
    protected float attackRate = 0;
    protected int wayPointIndex = 0;
    protected NavMeshAgent agent;
    protected Health health;

    [Header("Debug info")]
    [SerializeField] protected States currentState;
    protected Transform playerPos;





    //s 







/*
    protected enum States
    {
        patrolling,
        followPlayer,
        runAway,
        standAndAttack,
        findAlly,
        heal,
        dead
    }

    private void Start()
    {
        currentState = startState;
        agent = GetComponent<NavMeshAgent>();
        playerPos = GameObject.FindWithTag("Player").transform;
        agent.speed = movementSpeed;
        startSpeed = movementSpeed;
        health = GetComponent<Health>();

        AssignWaypoints();
    }

    void Update()
    {
        StateMachine();
        ChangeState();
    }

    public void StateMachine()
    {
        switch (currentState)
        {
            case States.patrolling:
                Patrolling();
                break;

            case States.followPlayer:
                FollowPlayer();
                break;

            case States.runAway:
                RunAway();
                break;

            case States.standAndAttack:
                StandAndAttack();
                break;
            case States.findAlly:
                FindAlly();
                break;
            case States.heal:
                Healt();
                break;
            case States.dead:
                Dead();
                break;
        }
    }

    public virtual void ChangeState()
    {
        //depends on enemy type
    }

    void Patrolling()
    {
        movementSpeed = startSpeed;

        if (!agent.hasPath || agent.remainingDistance < 0.5f)
        {
            MoveToNextPoint();
        }
    }

    public void MoveToNextPoint()
    {
        if (wayPoints.Length == 0)
            return;

        if (agent.remainingDistance < .5f && !agent.pathPending)
        {
            wayPointIndex = (wayPointIndex + 1) % wayPoints.Length;
            agent.destination = wayPoints[wayPointIndex].position;
        }
    }
    void AssignWaypoints()
    {
        wayPoints = wayPointsObject.GetComponentsInChildren<Transform>();
        if (wayPoints == null || wayPoints.Length == 0)
        {
            Debug.LogError("No waypoint found");
        }
    }

    void FollowPlayer()
    {
        movementSpeed = startSpeed * 1.2f;
        agent.destination = playerPos.position;
        RotateToPlayer();
        Attack();
    }

    void RunAway()
    {
        Vector3 direction = (transform.position - playerPos.position).normalized;
        Vector3 runToPos = transform.position + direction * 10f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(runToPos, out hit, 2f, NavMesh.AllAreas))
        {
            agent.destination = hit.position;
        }

        movementSpeed = startSpeed * 1.5f;
    }

    public virtual void FindAlly()
    {
        //
    }

    public virtual void Healt()
    {
        //
    }

    public virtual void Attack()
    {
        //
    }

    public virtual void Dead()
    {
        //
    }

    public virtual void StandAndAttack()
    {
        agent.destination = transform.position;
        RotateToPlayer();
        Attack();
    }

    public void RotateToPlayer()
    {
        Vector3 lookDirection = new Vector3(playerPos.position.x, 2, playerPos.position.z);
        Vector3 direction = lookDirection - transform.position;

        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }

    

    public bool PlayerIsVisable()
    {
        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 direction = (playerPos.position - origin).normalized;
        float distance = Vector3.Distance(origin, playerPos.position);

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            if (hit.collider.CompareTag("Terrain"))
                return false;
            else if (hit.collider.CompareTag("Player"))
                return true;
        }

        return false;
    } */



    NavMeshAgent agent;
    StateMachine stateMachine;
    public NavMeshAgent Agent { get => agent; }
    //debug
    [SerializeField] string currentState;
    public EnemyPath path;
    GameObject player;
    public float spottingDistance;
    public float FOV = 85;
    public float eyeHeight;
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
            if(Vector3.Distance(transform.position, player.transform.position) < spottingDistance)
            {
                Vector3 targetDir = player.transform.position - transform.position - (Vector3.up * eyeHeight);
                float angleToPlayer = Vector3.Angle(targetDir, transform.forward);
                targetDir.Normalize();

                Debug.DrawLine(
                           transform.position + (Vector3.up * eyeHeight),
                           transform.position + (Vector3.up * eyeHeight) + targetDir * spottingDistance,
                           Color.red);

                //checkt if player is in FOV
                if (angleToPlayer >= -FOV && angleToPlayer <= FOV)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDir);
                    RaycastHit hitInfo = new RaycastHit();
                    if(Physics.Raycast(ray, out hitInfo, spottingDistance))
                    {
                        //checkt if object is player 
                        if(hitInfo.transform.gameObject == player)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}
