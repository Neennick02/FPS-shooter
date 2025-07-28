using UnityEngine;

public class MeleeEnemy : Enemy
{
    public override void ChangeState()
    {
        float distance = Vector3.Distance(transform.position, playerPos.position);

        // Run away if health is low
        if (health.GetHealth() < health.maxHealth / 4)
        {
            currentState = States.runAway;
            return;
        }

        // Follow player if within spotting distance and visible, and currently patrolling
        if (distance <= SpottingDistance && PlayerIsVisable() && currentState == States.patrolling)
        {
            currentState = States.followPlayer;
            return;
        }

        if(distance <= InRangeDistance && PlayerIsVisable())
        {
            currentState = States.standAndAttack;
        }

        if(distance <= SpottingDistance && PlayerIsVisable() && distance > InRangeDistance)
        {
            currentState = States.followPlayer;
        }

        // Go back to patrolling if too far or no longer visible
        if ((distance > SpottingDistance || !PlayerIsVisable()) &&
        (currentState == States.followPlayer || currentState == States.standAndAttack))
        {
            currentState = States.patrolling;
            MoveToNextPoint();
            return;
        }
    }

    public override void Attack()
    {
        float distance = Vector3.Distance(transform.position, playerPos.position);

        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval && distance < InRangeDistance)
        {
           Health playerHealth = playerPos.GetComponent<Health>();

            if(health != null)
            {
                playerHealth.Sethealth(damageAmount);
            }
            attackTimer = 0;
        }
    }
}
