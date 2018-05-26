using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower
{
    public Vector3 upperPoint { get; private set; }
    public Vector3 position { get; private set; }
    public Stack<DiskBehaviour> disksStack { get; private set; }
	

    public Tower(Vector3 position, Vector3 upperPoint, Stack<DiskBehaviour> stack = null)
    {
        this.position = position;
        this.upperPoint = upperPoint;
        if(stack == null)
        {
            disksStack = new Stack<DiskBehaviour>();
        }
        else
        {
            disksStack = stack;
        }
    }

    public Vector3 GetPositionForDisk(DiskBehaviour diskBehaviour)
    {
        Vector3 diskPosition = position;

        if (disksStack.Count == 0)
        {            
            diskPosition.y = diskBehaviour.yExtent;
        }
        else
        {
            diskPosition = disksStack.Peek().position;
            diskPosition.y += 2 * diskBehaviour.yExtent;
        }

        return diskPosition;
    }
}
