using UnityEngine;

public class AttackState : BaseState
{
    float moveTimer;
    float losePlayerTimer;
    public override void Enter()
    {

    }


    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            losePlayerTimer = 0;
            moveTimer += Time.deltaTime;

            if (moveTimer > Random.Range(3, 6))
            {
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
        }
    }

    public override void Exit()
    {

    }
}
