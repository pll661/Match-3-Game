using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
   public Camera ma;
    public GridManager grid;

    private void Awake()
    {
        ma = Camera.main;
        CameraMove();
    }

    private void Start()
    {

    }
    public void CameraMove()
    {
        float aspect=(float)Screen.width/(float)Screen.height;
        float boardW = grid.width * grid.cellSpacing;
        float boardH= grid.height * grid.cellSpacing;
        float orthoW = boardW / (2*aspect);
        float orthoH = boardH / 2;
        ma.orthographicSize = Mathf.Max(orthoW, orthoH) + 1;
    }

}
