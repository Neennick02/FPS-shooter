using UnityEngine;

public abstract class BaseState 
{
    public Enemy enemy;
    public StateMachine stateMachine;
    public abstract void Enter();

    public abstract void Perform();

    public abstract void Exit();

    protected void CheckHealth()
    {
        Health currentHealth = enemy.GetComponent<Health>();

         //if health is low flee 
        if (currentHealth.GetHealth() <= currentHealth.maxHealth / 3
             && currentHealth.GetHealth() > 0 && stateMachine.activeState is not FleeState)
        {
            stateMachine.ChangeState(new FleeState());
        }

        //is health is 0 die
        if (currentHealth.GetHealth() <= 0)
        {
            stateMachine.ChangeState(new DeadState());
            enemy.animatorScript.ActivateRagdoll();
        }

    }
}
