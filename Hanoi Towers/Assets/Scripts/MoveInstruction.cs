using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MoveInstruction
{
    public int from
    {
        get; private set;
    }

    public int to
    {
        get; private set;
    }

    public  MoveInstruction(int from, int to)
    {
        this.from = from;
        this.to = to;
    }

}
