using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject diskFab;
    [SerializeField]
    private GameObject towerFab;

    private const float SIZE_INCREASE_STEP = 0.1f;
    private const float GAP_BETWEEN_TOWERS = 0.5f;
    private const float TOWER_UPPER_POINT_DELTA = 1f;
    private const int TOWERS_AMOUNT = 3;

    private List<Tower> towersList = new List<Tower>();
    private Queue<MoveInstruction> moveList = new Queue<MoveInstruction>();

    public static event Action gameFinished;

    private void Awake()
    {
        DiskBehaviour.movementCompleted += MakeMove;
        MainMenuBehaviour.amountOfDisksChoosed += StartGame;
    }

    private void StartGame(int diskAmount)
    {
        GenerateInstructions(diskAmount);
        CreateGamefield(diskAmount);
        MakeMove();
    }

    private void CreateGamefield(int diskAmount)
    {
        GameObject diskObj = null;
        Vector3 position = Vector3.zero;
        float diskHalfHeight = diskFab.GetComponent<MeshRenderer>().bounds.extents.y;
        position.y = diskHalfHeight;
        Vector3 scale = diskFab.GetComponent<Transform>().localScale;
        float biggestExtent = 0;

        Stack<DiskBehaviour> firstTowerStack = new Stack<DiskBehaviour>();

        for(int index = diskAmount; index > 0; index--)
        {
            diskObj = Instantiate(diskFab, position, Quaternion.identity);
            diskObj.transform.localScale += new Vector3(index * SIZE_INCREASE_STEP, 0, index * SIZE_INCREASE_STEP);
            firstTowerStack.Push(diskObj.GetComponent<DiskBehaviour>());
            if(index == diskAmount)
            {
                biggestExtent = diskObj.GetComponent<MeshRenderer>().bounds.extents.x;
            }
            position.y += (diskHalfHeight * 2);
        }

       
        position.y = 0;
        GameObject towerObj = Instantiate(towerFab, position, Quaternion.identity);
        towerObj.transform.localScale += new Vector3(0, diskAmount * diskObj.transform.localScale.y, 0);
        position.y = towerObj.GetComponent<MeshRenderer>().bounds.extents.y;
        towerObj.transform.position = position;
        Vector3 upperPoint = position;
        upperPoint.y += position.y + TOWER_UPPER_POINT_DELTA;
        towersList.Add(new Tower(position, upperPoint, firstTowerStack));

        for (int index = 1; index < TOWERS_AMOUNT; index++)
        {
            position.x = (biggestExtent * 2 + GAP_BETWEEN_TOWERS) * index;
            towerObj = Instantiate(towerObj, position, Quaternion.identity);
            upperPoint = position;
            upperPoint.y += position.y + TOWER_UPPER_POINT_DELTA;
            towersList.Add(new Tower(position, upperPoint, null));
        }
        
    }

    private void MakeMove()
    {
        if(moveList.Count <= 0)
        {
            Debug.Log("Finished!");
            if(gameFinished != null)
            {
                gameFinished();
            }
            return;
        }

        MoveInstruction instruction = moveList.Dequeue();

        List<Vector3> pathList = new List<Vector3>();
        pathList.Add(towersList[instruction.from].disksStack.Peek().position);
        pathList.Add(towersList[instruction.from].upperPoint);
        pathList.Add(towersList[instruction.to].upperPoint);
        pathList.Add(towersList[instruction.to]
            .GetPositionForDisk(towersList[instruction.from].disksStack.Peek()));

        towersList[instruction.from].disksStack.Peek().Move(pathList);

        towersList[instruction.to].disksStack.Push(towersList[instruction.from].disksStack.Pop());
    }

    private void GenerateInstructions(int diskAmount)
    {
        SolutionHanoibns(diskAmount, 0, 1, 2);
    }

    private void SolutionHanoibns(int k, int a, int b, int c)
    {
        if (k > 1)
        {

            SolutionHanoibns(k - 1, a, c, b);
        }
        moveList.Enqueue(new MoveInstruction(a, b));

        if (k > 1)
        {
            SolutionHanoibns(k - 1, c, b, a);
        }

    }

    private void OnDestroy()
    {
        DiskBehaviour.movementCompleted -= MakeMove;
        MainMenuBehaviour.amountOfDisksChoosed -= StartGame;
    }
}
