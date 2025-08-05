using UnityEngine;

public class AttackState : BaseState
{
    float moveTimer;
    float losePlayerTimer;
    float shotTimer;
    public override void Enter()
    {

    }


    public override void Perform()
    {
        CheckHealth();

        if (enemy.CanSeePlayer() || enemy.CanHearPlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;

            // RotateToPlayer();
            enemy.transform.LookAt(enemy.transform.position);
            if (shotTimer > enemy.fireRate)
            {
                Attack();
                shotTimer = 0;
            }

            float distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.Player.transform.position);
            float safeDistance = 10f;

            if (moveTimer > Random.Range(3, 6))
            {
                Vector3 destination;

                if (distanceToPlayer < safeDistance)
                {
                    // Move away from player to maintain distance

                    Vector3 awayFromPlayer = enemy.transform.position - enemy.Player.transform.position;
                    awayFromPlayer.y = 0;
                    awayFromPlayer.Normalize();

                    // Add some random strafe component so enemy doesn't just back away straight
                    Vector3 strafe = Vector3.Cross(Vector3.up, awayFromPlayer) * Random.Range(-1f, 1f);

                    Vector3 moveDirection = (awayFromPlayer + strafe).normalized;

                    destination = enemy.transform.position + moveDirection * safeDistance/ 2;  // Move 2 units away/strafe
                }
                else
                {
                    // random offset within 2 units radius
                    Vector3 randomOffset = Random.insideUnitSphere * 2f;
                    randomOffset.y = 0;  // keep on same ground level if your game is 3D on flat ground

                     destination = enemy.Player.transform.position + randomOffset;
                }
                enemy.Agent.SetDestination(destination);
                moveTimer = 0;
            }
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

    }

    void RotateToPlayer()
    {
        Vector3 direction = enemy.Player.transform.position - enemy.transform.position;
        direction.y = 0; // keep rotation horizontal

        float distance = direction.magnitude;


        // Rotate towards targetRotation at a fixed speed (degrees per second)
        if (direction.sqrMagnitude > 0.01f) // avoid zero-length vectors
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, targetRotation, enemy.rotationSpeed * Time.deltaTime);
        }
    }


    void Attack()
    {
        RotateToPlayer();
        Transform gunBarrel = enemy.barrel;

        Vector3 shootDir = -gunBarrel.forward;

        //instantiate new bullet
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/RifleBullet") as GameObject, gunBarrel.position, Quaternion.LookRotation(shootDir));


        //add force to bullet
        bullet.GetComponent<Rigidbody>().linearVelocity = Quaternion.AngleAxis(Random.Range(-enemy.shootingAccuracy, enemy.shootingAccuracy), Vector3.up) * shootDir * enemy.bulletSpeed;

        //control bulletDamage
        bullet.GetComponent<BulletScript>().SetDamage(enemy.damageAmount);
        shotTimer = 0;

        Debug.DrawRay(gunBarrel.position, shootDir * 5f, Color.red, 2f);
    }
}
