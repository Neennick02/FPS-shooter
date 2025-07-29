using UnityEngine;

public class SearchState : BaseState
{
    float searchTimer;
    float moveTimer;
    public override void Enter()
    {
        enemy.Agent.SetDestination(enemy.LastKnowsPlayerPos);
    }

    public override void Perform()
    {
        //checks if player can be seen
        if (enemy.CanSeePlayer())
        {
            //checks if player can be attacked
            stateMachine.ChangeState(new AttackState());
        }

        //checking if we arrived at last know position
        if(enemy.Agent.remainingDistance < enemy.Agent.stoppingDistance)
        {

            searchTimer += Time.deltaTime;
            moveTimer += Time.deltaTime;

            if (moveTimer > Random.Range(1, 3))
            {
                //walk around to find player
                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 10));
                moveTimer = 0f;
            }
            //if player is not found in time return to patrolling state
            if (searchTimer > Random.Range(6, 10))
            {
                stateMachine.ChangeState(new PatrolState());
            }
        }
    }

    public override void Exit()
    {
    }

    
}
