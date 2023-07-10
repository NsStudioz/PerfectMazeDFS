using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DFS_MazeGenerator
{
    public class MazeGenerator : MonoBehaviour
    {
        [Header("Maze Size")]
        [SerializeField]
        [Range(10, 250)]
        private int mazeWidth;

        [SerializeField]
        [Range(10, 250)]
        private int mazeHeight;

        [Header("Cell Elements")]
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private List<Cell> totalCells;
        [SerializeField] private List<Cell> visitedCells;

        [SerializeField] private bool gridGenerated = false;

        private void Awake()
        {
            InitializeCellLists();
        }

        void Start()
        {

        }

        #region Grid

        private void InitializeCellLists()
        {
            totalCells = new List<Cell>();
            visitedCells = new List<Cell>();
        }

        private void CleanLists()
        {
            if (gridGenerated)
            {
                DestroyCellsInLists();
                ClearCellLists();
            }

            gridGenerated = false;
        }

        private void DestroyCellsInLists()
        {
            foreach (Cell cellInstance in totalCells)
                Destroy(cellInstance.gameObject);

            foreach (Cell cellInstance in visitedCells)
                Destroy(cellInstance.gameObject);
        }

        private void ClearCellLists()
        {
            totalCells.Clear();
            visitedCells.Clear();
        }


        #endregion

    }
}


/*                    Vector3 cellPos = new Vector3(x, 0, y); // Decided not to center the maze.
                    Cell newCell = SpawnNewCellInstance(cellPos);
                    newCell.SetCellGameObjectVisibility(false);
                    totalCells.Add(newCell);*/

