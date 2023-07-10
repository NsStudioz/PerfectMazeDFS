using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DFS_MazeGenerator
{
    public class Cell : MonoBehaviour
    {

        [SerializeField] private GameObject[] cellWalls = null;

        void Awake()
        {
            InitializeCellWalls();
        }

        private void InitializeCellWalls()
        {
            cellWalls = new GameObject[transform.childCount];

            for (int i = 0; i < cellWalls.Length; i++)
                cellWalls[i] = transform.GetChild(i).gameObject;
        }

        public void RemoveWall(int index)
        {
            SetWallState(index, false);
        }

        public void SetCellGameObjectVisibility(bool state)
        {
            gameObject.SetActive(state);
        }

        private void SetWallState(int index, bool state)
        {
            cellWalls[index].SetActive(state);
        }
    }
}



/*    public void SetCellVisiblity(bool state)
    {
        SetCellGameObjectVisibility(state);
    }*/