using UnityEngine;

public class EnemySniper : RangedEnemy
{
   /* public override void ChangeState()
    {
        float distance = Vector3.Distance(transform.position, playerPos.position);

        // Ren weg als health laag is
        if (health.GetHealth() < health.maxHealth / 4)
        {
            currentState = States.runAway;
            return;
        }
        else
        {
            // Blijft staan en valt aan als player in beeld is

            currentState = States.standAndAttack;
        }
    }

    public override void StandAndAttack()
    {
        //zorgt dat sniper alleen aanvalt als speler in range is
        agent.destination = transform.position;
        if (PlayerIsVisable())
        {
            RotateToPlayer();
            Attack();
        }
    }*/
}
