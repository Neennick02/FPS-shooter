using UnityEngine;

public class AttackState : BaseState
{
    float moveTimer;
    float losePlayerTimer;
    float shotTimer;
    public override void Enter()
    {
        enemy.Agent.stoppingDistance = 5f;
        enemy.Agent.speed = 4;
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

            UpdateAimTargetPosition();
            RotateToTarget();
            AimGunAtTarget();


            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);

            if (enemy.Agent.isActiveAndEnabled)
            {
                if (distanceToPlayer > enemy.Agent.stoppingDistance)
                {
                    enemy.Agent.isStopped = false;
                    enemy.animatorScript.SetIsMoving(false);
                    enemy.Agent.SetDestination(enemy.aimTarget.position);
                }
                else
                {
                    enemy.Agent.isStopped = true;
                    enemy.animatorScript.SetIsMoving(false);
                }


                if (shotTimer > enemy.fireRate)
                {
                    Attack();
                    shotTimer = 0;
                }
            }
          



            //store player position
            enemy.LastKnowsPlayerPos = enemy.aimTarget.position;
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
        enemy.Agent.speed = 3;
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
            Transform playerCamTransform = Camera.main.transform;

            enemy.aimTarget.position = playerCamTransform.position ;
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
