using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    public MoveCommand()
    {
        
    }

    public void Execute()
    {
        PlayerActions.Move();
    }
}
