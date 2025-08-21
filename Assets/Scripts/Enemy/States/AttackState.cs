using UnityEngine;

public class AttackState : BaseState
{
    float moveTimer;
    float losePlayerTimer;
    float shotTimer;
    public override void Enter()
    {
        enemy.Agent.stoppingDistance = 5f;
        enemy.Agent.updateRotation = false;
    }


    public override void Perform()
    {
        CheckHealth();

        if (enemy.CanSeePlayer() || enemy.CanHearPlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;

           // UpdateAimTargetPosition();
            RotateToTarget();
            AimGunAtTarget();

            enemy.Agent.SetDestination(enemy.aimTarget.transform.position);


            if (shotTimer > enemy.fireRate)
            {
                Attack();
                shotTimer = 0;
            }

            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
           // float safeDistance = 10f;

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

    void RotateToTarget()
    {
        Vector3 directionToTarget = enemy.aimTarget.transform.position - enemy.transform.position;
        directionToTarget.y = 0; // keep flat rotation

        // Rotate enemy body smoothly toward player
        if (directionToTarget.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            enemy.transform.rotation = Quaternion.Slerp(
                enemy.transform.rotation,
                targetRotation,
                Time.deltaTime * enemy.rotationSpeed
                );
        }
    }

    void UpdateAimTargetPosition()
    {
        if (enemy.Player != null && enemy.aimTarget != null)
        {
            enemy.aimTarget.position = enemy.Player.transform.position + Vector3.up * 1.5f;
        }
    }

    void AimGunAtTarget()
    {
        if (enemy.aimTarget == null || enemy.barrel == null) return;

        // 1. Keep gun at hand/animation position
        enemy.gunObject.transform.localPosition = Vector3.Lerp(
            enemy.gunObject.transform.localPosition,
            enemy.animatorScript.aimingPos.localPosition,
            Time.deltaTime * enemy.rotationSpeed
        );

        // 2. Calculate direction from barrel to aimTarget
        Vector3 dirToTarget = (enemy.aimTarget.position - enemy.barrel.position).normalized;


        // 3. Rotate barrel/gun toward aimTarget using LookRotation
        if (dirToTarget.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dirToTarget);

            Quaternion rotationOffset = Quaternion.Euler(0, 180f, 0); // flip Z-axis if needed


            // Apply rotation to gunObject's world rotation
            enemy.gunObject.transform.rotation = Quaternion.Slerp(
                enemy.gunObject.transform.rotation,
                targetRot * rotationOffset,
                Time.deltaTime * 3f
            );
        }
    }


void Attack()
    {
        Transform gunBarrel = enemy.barrel;

        Vector3 shootDir = gunBarrel.forward;


        //instantiate new bullet
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/RifleBullet") as GameObject, gunBarrel.position, Quaternion.LookRotation(shootDir));


        //add force to bullet
        bullet.GetComponent<Rigidbody>().linearVelocity = Quaternion.AngleAxis(Random.Range(-enemy.shootingAccuracy, enemy.shootingAccuracy), Vector3.up) * shootDir * enemy.bulletSpeed;

        //control bulletDamage
        bullet.GetComponent<BulletScript>().SetDamage(enemy.damageAmount);
        shotTimer = 0;

    }
}
