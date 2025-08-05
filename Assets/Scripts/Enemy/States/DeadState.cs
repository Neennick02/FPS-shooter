using UnityEngine;
using UnityEngine.UI;
public class DeadState : BaseState
{
    bool isDead = false;
    float despawnTime = 20f;
    public override void Enter()
    {
        //disable navmeshAgent
        enemy.Agent.enabled = false;

        isDead = true;

        //disable healthbar
        enemy.transform.GetComponent<Health>().DisableHealthBar();
        
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
