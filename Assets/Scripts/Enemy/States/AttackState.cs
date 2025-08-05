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
            
            //find direction to player
            Vector3 lookDir = new Vector3(enemy.Player.transform.position.x, enemy.Player.transform.position.y, enemy.Player.transform.position.z);
            Vector3 direction = lookDir - enemy.transform.position;

            //rotate to player
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.forward);
            enemy.transform.rotation = Quaternion.Lerp(enemy.transform.rotation, rotation, enemy.rotationSpeed * Time.deltaTime);


            if(shotTimer > enemy.fireRate)
            {
                Shoot();
            }

            if (moveTimer > Random.Range(3, 6))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 2));
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


    void Shoot()
    {
        Transform gunBarrel = enemy.barrel;

        //instantiate new bullet
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/RifleBullet") as GameObject, gunBarrel.position, enemy.transform.rotation);

        //calculate direction to player
        Vector3 shootDir = (enemy.Player.transform.position - gunBarrel.transform.position).normalized;

        //add force to bullet
        bullet.GetComponent<Rigidbody>().linearVelocity = Quaternion.AngleAxis(Random.Range(-enemy.shootingAccuracy, enemy.shootingAccuracy), Vector3.up) * shootDir * enemy.bulletSpeed;

        //control bulletDamage
        bullet.GetComponent<BulletScript>().SetDamage(enemy.damageAmount);
        shotTimer = 0;
    }
}
