using UnityEngine;

public class PatrolState : BaseState
{

    public int waypointIndex;

    public override void Enter()
    {
        enemy.AnimatorScript.SetIsMoving(true);
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
        if (enemy.Agent.enabled && enemy.Path != null && enemy.Path.waypoints.Count > 0)
        {
            if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= 0.2f)
            {
                    if (waypointIndex < enemy.Path.waypoints.Count - 1)
                    {
                        waypointIndex++;
                    }
                    else
                    {
                        waypointIndex = 0;
                    }
                enemy.Agent.isStopped = false; // Unpause agent

                enemy.Agent.SetDestination(enemy.Path.waypoints[waypointIndex].position);
            }
        }
    }
}
