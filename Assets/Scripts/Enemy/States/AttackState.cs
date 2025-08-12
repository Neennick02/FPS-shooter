using UnityEngine;

public class AttackState : BaseState
{
    float moveTimer;
    float losePlayerTimer;
    float shotTimer;
    public override void Enter()
    {
        enemy.Agent.stoppingDistance = 5f;
    }


    public override void Perform()
    {
        CheckHealth();
        RotateToPlayer();
        UpdateAimTargetPosition();

        if (enemy.CanSeePlayer() || enemy.CanHearPlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;

          


            if (shotTimer > enemy.fireRate)
            {
                Attack();
                shotTimer = 0;
            }

            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
            float safeDistance = 10f;

            /*if (moveTimer > 3)
            {
                Vector3 destination;
                float bufferDistance = 3f;

                if (distanceToPlayer < safeDistance)
                {
                    // Move away from player to maintain distance

                    Vector3 awayFromPlayer = (enemy.transform.position - enemy.Player.transform.position).normalized;

                    // Add some random strafe component so enemy doesn't just back away straight
                    Vector3 strafe = Vector3.Cross(Vector3.up, awayFromPlayer) * Random.Range(-1f, 1f);
                    Vector3 moveDir = (awayFromPlayer + strafe).normalized;
                    
     

                    destination = enemy.transform.position + moveDir * (safeDistance - distanceToPlayer + bufferDistance);
                    enemy.Agent.SetDestination(destination);
                }
                else if(distanceToPlayer > safeDistance + bufferDistance)
                {
                    // random offset within 2 units radius
                    Vector3 randomOffset = Random.insideUnitSphere * 2f;
                    randomOffset.y = 0;  // keep on same ground level if your game is 3D on flat ground

                   destination = enemy.Player.transform.position + randomOffset;
                    enemy.Agent.SetDestination(destination);
                }
                else
                {
                    enemy.Agent.ResetPath();
                }
                moveTimer = 0;
            }*/


            enemy.Agent.SetDestination(enemy.Player.transform.position);


            //store player position
            enemy.LastKnowsPlayerPos = enemy.Player.transform.position;
        }
        else
        {
            losePlayerTimer += Time.deltaTime;
            if(losePlayerTimer > 3)
            {
                //change to search state
                
                stateMachine.ChangeState(new SearchState());
            }
        }



    }

    public override void Exit()
    {
        enemy.Agent.updateRotation = true;
    }

    void RotateToPlayer()
    {
        Vector3 directionToPlayer = enemy.Player.transform.position - enemy.transform.position;
        directionToPlayer.y = 0; // keep flat rotation

        // Rotate enemy body smoothly toward player
        if (directionToPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * enemy.rotationSpeed);
        }

        // Move enemy forward towards player (or use NavMeshAgent)
        if (enemy.Agent != null)
        {
            enemy.Agent.SetDestination(enemy.Player.transform.position);
        }
        else
        {
            // Simple movement toward player if no NavMeshAgent
            float moveSpeed = 3f; // example speed
            enemy.transform.position += enemy.transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    void UpdateAimTargetPosition()
    {
        // Update aim target position to player's head (or adjusted height)
        Vector3 targetPos = enemy.Player.transform.position + Vector3.up * 1;
        enemy.animatorScript.target.position = targetPos;
    }


void Attack()
    {
        RotateToPlayer();
        Transform gunBarrel = enemy.barrel;

        Vector3 shootDir = (enemy.Player.transform.position - enemy.barrel.position).normalized;


        //instantiate new bullet
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/RifleBullet") as GameObject, gunBarrel.position, Quaternion.LookRotation(gunBarrel.forward));


        //add force to bullet
        bullet.GetComponent<Rigidbody>().linearVelocity = Quaternion.AngleAxis(Random.Range(-enemy.shootingAccuracy, enemy.shootingAccuracy), Vector3.up) * shootDir * enemy.bulletSpeed;

        //control bulletDamage
        bullet.GetComponent<BulletScript>().SetDamage(enemy.damageAmount);
        shotTimer = 0;

    }
}
