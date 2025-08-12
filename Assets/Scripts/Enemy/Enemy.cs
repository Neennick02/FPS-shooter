using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.InputSystem.Processors;

public class Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] public GameObject gunObject;
    StateMachine stateMachine;
    GameObject player;
    Vector3 lastKnownPlayPos;
    public NavMeshAgent Agent { get => agent; }  
    public GameObject Player { get => player; }
    public Vector3 LastKnowsPlayerPos { get => lastKnownPlayPos; set => lastKnownPlayPos = value; }
    public EnemyPath path;
    [Header("Sight Values")]
    
    public float spottingDistance;
    public float hearingDistance;
    public float rotationSpeed;
    public float FOV = 85;
    public float eyeHeight;

    [Header("Weapon Values")]
    public int damageAmount;
    public Transform barrel;
    [Range(0.1f, 10f)] public float fireRate;
    public float bulletSpeed = 30;
    [Range(0.1f, 10f)] public float shootingAccuracy = 3;
    [SerializeField] string currentState;
    public EnemyAnimator animatorScript;
    public Transform firePoint;

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
                Vector3 origin = transform.position + Vector3.up * eyeHeight;
                Vector3 targetDir = (player.transform.position - origin).normalized;

                float angleToPlayer = Vector3.Angle(targetDir, transform.forward);
                targetDir.Normalize();

                //draw line in sceneview
                Debug.DrawLine(
                           transform.position + (Vector3.up * eyeHeight),
                           transform.position + (Vector3.up * eyeHeight) + targetDir * spottingDistance,
                           Color.red);

                //checkt if player is in FOV
                if (angleToPlayer <= FOV)
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDir);
                    RaycastHit hitInfo = new RaycastHit();
                    if(Physics.Raycast(ray, out hitInfo, spottingDistance))
                    {
                        //checkt if object is player 
                        if(hitInfo.transform.gameObject == player && hitInfo.distance < spottingDistance)
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
            if(Vector3.Distance(transform.position, player.transform.position) < hearingDistance)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(Agent.destination, new Vector3(2, 2, 2));
    }
}
