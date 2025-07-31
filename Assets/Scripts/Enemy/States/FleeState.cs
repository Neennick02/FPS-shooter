using UnityEngine;
using UnityEngine.AI;

public class FleeState : BaseState
{
    float startSpeed;
    public override void Enter()
    {
        startSpeed = enemy.Agent.speed;
    }

    public override void Perform()
    {
        CheckHealth();
        Vector3 direction = (enemy.transform.position - enemy.Player.transform.position).normalized;
        Vector3 runToPos = enemy.transform.position + direction * 10f;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(runToPos, out hit, 2f, NavMesh.AllAreas) && enemy.Agent.enabled)
        {
            enemy.Agent.destination = hit.position;
        }

        enemy.Agent.speed = startSpeed * 1.2f;
    }

    public override void Exit()
    {
        
    }
}
