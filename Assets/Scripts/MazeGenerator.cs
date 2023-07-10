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

        private void ClearCellLists()
        {
            if (gridGenerated)
            {
                totalCells.Clear();
                visitedCells.Clear();
            }

            gridGenerated = false;
        }





        #endregion

    }
}

