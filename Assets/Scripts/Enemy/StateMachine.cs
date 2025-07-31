using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public BaseState activeState;

    public void Initialise()
    {
        //setup default state
        Debug.Log("initialize");
        ChangeState(new PatrolState());
    }

    void Update()
    {
        //calls perform method from current state
        if(activeState != null)
        {
            activeState.Perform();
        }
    }

    public void ChangeState(BaseState newState)
    {
        if(activeState != null)
        {
            //run cleanop on activeState
            activeState.Exit();
        }
        //change to new state
        activeState = newState;

        //fail safe null check
        if(activeState != null)
        {
            //Setup new state
            activeState.stateMachine = this;
            activeState.enemy = GetComponent<Enemy>();
            activeState.Enter();
        }
    }
}
