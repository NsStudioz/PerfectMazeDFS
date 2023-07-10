using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CellOld : MonoBehaviour
{

    [SerializeField] private GameObject[] cellWalls = null;

    private void Awake()
    {
        cellWalls = new GameObject[transform.childCount];

        for (int i = 0; i < cellWalls.Length; i++)
        {
            cellWalls[i] = transform.GetChild(i).gameObject;
        }
    }

    public void RemoveWall(int index)
    {
        cellWalls[index].SetActive(false);
    }

    public void SetCellVisible(bool state)
    {
        if (state == true)
        {
            gameObject.SetActive(true);
            for(int i = 0; i < cellWalls.Count(); i++)
                cellWalls[i].SetActive(true);
        }     
        else
        {
            gameObject.SetActive(false);
        }
    }




    #region Trash_Code:
    //[SerializeField] private bool isVisited = false;

    /*    public Vector3 GetCellPosition()
    {
        return transform.position;
    }*/

    /*    public bool MarkNodeAsVisited(bool state)
    {
        return isVisited = state;
    }

    public void ResetCellState()
    {
        isVisited = false;
    }

    public bool GetCellState()
    {
        return isVisited;
    }*/
    #endregion

}
