using UnityEngine;

public class PatrolState : BaseState
{

    public int waypointIndex;

    public override void Enter()
    {
        enemy.Agent.speed = 2f;
    }

    public override void Perform()
    {
        CheckHealth();
        PatrolCyle();


        if (enemy.CanSeePlayer() || enemy.CanHearPlayer())
        {
            stateMachine.ChangeState(new AttackState());
        }
    }

    public override void Exit()
    {

    }

    public void PatrolCyle()
    {
        if (enemy.Agent.enabled && enemy.path != null && enemy.path.waypoints.Count > 0)
        {
            if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= 0.2f)
            {
                    if (waypointIndex < enemy.path.waypoints.Count - 1)
                    {
                        waypointIndex++;
                    }
                    else
                    {
                        waypointIndex = 0;
                    }
                enemy.Agent.isStopped = false; // Unpause agent

                enemy.Agent.SetDestination(enemy.path.waypoints[waypointIndex].position);
            }
        }
    }
}
