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

        [Header("Grid Elements")]
        [SerializeField] private bool mazeGenerated = false;
        [SerializeField] private bool gridGenerated = false;
        [SerializeField] private bool generateMazeInstantly = false;
        //
        [SerializeField] private float gridAnimationSpeed;
        [SerializeField] private float defaultAnimationSpeed = 0.005f;
        private float InstantGeneration = 0f;

        private void Awake()
        {
            InitializeCellLists();
        }

        void Start()
        {

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                CreateNewMazeGrid(mazeWidth, mazeHeight);
            }
        }

        #region Grid

        private void InitializeCellLists()
        {
            totalCells = new List<Cell>();
            visitedCells = new List<Cell>();
        }

        private void CleanLists()
        {
            if (mazeGenerated)
            {
                DestroyCellsInLists();
                ClearCellLists();
            }

            mazeGenerated = false;
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


        // Create a grid made from cells:
        private void CreateNewMazeGrid(int _width, int _height)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Cell newCell = SpawnNewCellInstance(SetCellInstancePosition(x, y));
                    newCell.SetCellGameObjectVisibility(false);
                    AddToTotalCellsList_NewCellInstance(newCell);
                }
            }

            StartCoroutine(StartGridAnimation(mazeWidth, mazeHeight));
        }

        private Cell SpawnNewCellInstance(Vector3 cellPos)
        {
            return Instantiate(cellPrefab, cellPos, Quaternion.identity);
        }

        private Vector3 SetCellInstancePosition(int x, int y)
        {
            Vector3 cellPos = new Vector3(x, 0, y);
            return cellPos;
        }

        private void AddToTotalCellsList_NewCellInstance(Cell instance)
        {
            totalCells.Add(instance);
        }

        #endregion

        #region Grid_Animations:

        private IEnumerator StartGridAnimation(int _width, int _height)
        {
            CheckAnimationSpeed();

            for (int i = 0; i < totalCells.Count; i++)
            {
                yield return new WaitForSeconds(gridAnimationSpeed);
                totalCells[i].SetCellGameObjectVisibility(true);
            }
        }

        private void CheckAnimationSpeed()
        {
            if (generateMazeInstantly)
                gridAnimationSpeed = InstantGeneration;
            else
                gridAnimationSpeed = defaultAnimationSpeed;
        }

        #endregion

    }
}

/*        private void SetMazeGeneratedBoolState(bool state)
        {
            mazeGenerated = state;
        }*/


/*                    Vector3 cellPos = new Vector3(x, 0, y); // Decided not to center the maze.
                    Cell newCell = SpawnNewCellInstance(cellPos);
                    newCell.SetCellGameObjectVisibility(false);
                    totalCells.Add(newCell);*/

