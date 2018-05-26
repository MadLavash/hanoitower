using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DiskBehaviour : MonoBehaviour
{
    private Transform thisTransform;
    private MeshRenderer meshRenderer;
    public Vector3 position
    {
        get
        {
            if(thisTransform == null)
            {
                thisTransform = transform;
            }

            return transform.position;
        }
    }
    public float yExtent
    {
        get
        {
            if(meshRenderer == null)
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }
            return meshRenderer.bounds.extents.y;
        }
    }

    public static event Action movementCompleted;

    private void Awake()
    {
        thisTransform = transform;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void Move(List<Vector3> pathList)
    {
        StartCoroutine(MoveRoutine(pathList));
    }

    private IEnumerator MoveRoutine(List<Vector3> pathList)
    {
        float progress = 0;
        while(pathList.Count > 1)
        {
            progress = 0;

            while(progress < 1)
            {
                progress += Time.deltaTime;
                thisTransform.position = Vector3.Lerp(pathList[0], pathList[1], Mathf.SmoothStep(0, 1, progress));
                yield return null;
            }

            pathList.RemoveAt(0);
        }

        if(movementCompleted != null)
        {
            movementCompleted();
        }

    }
}
