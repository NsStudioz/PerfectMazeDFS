using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public bool isCellVisible = false;

        [Header("Grid Elements")]
        [SerializeField] private bool gridGenerated = false;
        [SerializeField] private bool generateMazeInstantly = false;
        private bool mazeGenerated = false;
        //

        [Range(0.0001f, 1f)]
        [Tooltip("Change the animation speed of maze generation")]
        [SerializeField] private float animationSpeed = 0.005f;
        private float fastestGeneration = 0f;
        private float gridAnimationSpeed;

        private enum Direction
        {
            Right, Left, Up, Down
        }

        private void InitializeCellLists()
        {
            totalCells = new List<Cell>();
            visitedCells = new List<Cell>();
        }

        private void Awake()
        {
            InitializeCellLists();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                CleanLists();
                CreateNewMazeGrid(mazeWidth, mazeHeight);
            }

/*            if (Input.GetKeyDown(KeyCode.G))
                GenerateTheMaze();*/
        }

        #region Grid_Cleanup:

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


        #region Grid



        // Create a grid made from cells:
        private void CreateNewMazeGrid(int _width, int _height)
        {
            this.mazeHeight = _height;
            this.mazeWidth = _width;
            //
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Cell newCell = SpawnNewCellInstance(SetNewCellInstancePosition(x, y));
                    newCell.SetCellGameObjectVisibility(isCellVisible);
                    AddToTotalCellsList_NewCellInstance(newCell);
                }
            }
            //
            gridGenerated = true;
            StartCoroutine(StartGridAnimation());
        }

        private Cell SpawnNewCellInstance(Vector3 cellPos)
        {
            return Instantiate(cellPrefab, cellPos, Quaternion.identity);
        }

        private Vector3 SetNewCellInstancePosition(int x, int y)
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

        private IEnumerator StartGridAnimation()
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
                gridAnimationSpeed = fastestGeneration;
            else
                gridAnimationSpeed = animationSpeed;
        }

        #endregion



    }
}

