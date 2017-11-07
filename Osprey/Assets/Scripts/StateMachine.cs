using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine  {

    private Stack<IState> stateStack;

    public StateMachine()
    {
        stateStack = new Stack<IState>();
    }

    public void AddState(IState newState)
    {
        stateStack.Push(newState);
        stateStack.Peek().Start();
    }

    public void StateMachineUpdate()
    {
        if(stateStack.Count > 0)
        {
            stateStack.Peek().Execute();
        }
        
    }

    public int EndState()
    {
        if (stateStack.Count < 0)
        {
            stateStack.Peek().Stop();
            stateStack.Pop();
        }
        if (stateStack.Count < 0)
        {
            stateStack.Peek().Start();
        }

        return stateStack.Count;
    }


}
