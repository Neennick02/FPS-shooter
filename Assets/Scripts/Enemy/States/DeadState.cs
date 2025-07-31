using UnityEngine;

public class DeadState : BaseState
{
    bool isDead = false;
    float despawnTime = 20f;
    float timer = 0;
    public override void Enter()
    {
        //disable navmeshAgent
        enemy.Agent.enabled = false;

        //add rigidbody
            isDead = true;
            Rigidbody enemyRB = enemy.gameObject.AddComponent<Rigidbody>();
            enemyRB.isKinematic = false;
            enemyRB.mass = 40f;
    }

    public override void Perform()
    {
        if (isDead)
        {

                UnityEngine.Object.Destroy(enemy.gameObject, despawnTime);
        }
    }

    public override void Exit()
    {
        
    }
}
