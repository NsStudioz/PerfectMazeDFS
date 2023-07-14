using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PerfectMazeDFS
{
    public class Backup : MonoBehaviour
    {
        [Header("Maze Size")]
        [SerializeField]
        [Range(10, 200)]
        private int mazeWidth_Slider;

        [SerializeField]
        [Range(10, 200)]
        private int mazeHeight_Slider;
        //
        private int mazeWidth;
        private int mazeHeight;

        [Header("Cell Elements")]
        [SerializeField] private Cell cellPrefab;
        [SerializeField] private List<Cell> totalCells;
        [SerializeField] private List<Cell> visitedCells;
        [SerializeField] private bool isCellVisible = false;

        [Header("Grid Elements")]
        private bool gridGenerated = false;
        private bool isFastestMazeGeneration = false;
        private bool gridAnimationsGenerated = false;

        [Range(0.0001f, 1f)]
        [Tooltip("Change the animation speed of maze generation")]
        [SerializeField] private float animationSpeed = 0.005f;
        private float fastestGeneration = 0f;
        private float gridAnimationSpeed;

        private enum Direction
        {
            Right, Left, Up, Down
        }

        private Direction OppositeDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.Right: return Direction.Left;
                case Direction.Up: return Direction.Down;
                case Direction.Down: return Direction.Up;
                case Direction.Left: return Direction.Right;
                default: return Direction.Up; // BECUASE WHY THE FUCK NOT
            }
        }

        private void Awake()
        {
            InitializeCellLists();
        }

        private void OnEnable()
        {
            // Buttons:
            UI.OnClickGenerateMaze += BeginMazeSimulation;
            UI.OnClickDestroyMaze += DestroyMazeSimulation;
            // Sliders:
            UI.OnClickWidthValueChange += ChangeMazeWidth;
            UI.OnClickHeightValueChange += ChangeMazeHeight;
            // Toggle:
            UI.OnClickMazeGenerationToggleChange += ChangeMazeGenerationMode;
        }

        private void OnDisable()
        {
            // Buttons:
            UI.OnClickGenerateMaze -= BeginMazeSimulation;
            UI.OnClickDestroyMaze -= DestroyMazeSimulation;
            // Sliders:
            UI.OnClickWidthValueChange -= ChangeMazeWidth;
            UI.OnClickHeightValueChange -= ChangeMazeHeight;
            // Toggle:
            UI.OnClickMazeGenerationToggleChange += ChangeMazeGenerationMode;
        }

        #region Initializations:
        private void InitializeCellLists()
        {
            totalCells = new List<Cell>();
            visitedCells = new List<Cell>();
        }

        private void InitializeMazeSizeValues()
        {
            mazeWidth = mazeWidth_Slider;
            mazeHeight = mazeHeight_Slider;
        }

        #endregion

        #region Grid_Cleanup:

        private void CleanLists()
        {
            if (gridGenerated)
            {
                StopAllCoroutines();
                DestroyCellsInLists();
                ClearCellLists();
            }

            gridAnimationsGenerated = false;
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

        #region Grid:

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

            gridAnimationsGenerated = true;
            BeginDepthFirstSearchMazeSimulation();
        }

        private void CheckAnimationSpeed()
        {
            if (isFastestMazeGeneration)
                gridAnimationSpeed = fastestGeneration;
            else
                gridAnimationSpeed = animationSpeed;
        }

        private void BeginDepthFirstSearchMazeSimulation()
        {
            if (gridAnimationsGenerated)
                GenerateTheMaze();
        }

        #endregion

        #region MazeGeneration:

        private void GenerateTheMaze()
        {
            Stack<Cell> pointedCell = new Stack<Cell>();

            pointedCell.Push(totalCells[UnityEngine.Random.Range(0, totalCells.Count)]);

            StartCoroutine(CalculateMazeGeneration(pointedCell));
        }

        private IEnumerator CalculateMazeGeneration(Stack<Cell> pointedCell)
        {
            while (visitedCells.Count < totalCells.Count)
            {
                // check for possible cell neighbours next to the pointed cell:
                List<int> possibleNeighbours = new List<int>();
                List<Direction> availableDirs = new List<Direction>();
                int pointedCellIndex = totalCells.IndexOf(pointedCell.Peek());
                //
                CheckAvailableNeighbors(pointedCell, availableDirs, possibleNeighbours, pointedCellIndex);
                PickNextCellForWork(availableDirs, possibleNeighbours, pointedCell);
                // avoids crashing:
                yield return new WaitForSeconds(gridAnimationSpeed);
            }
        }

        private void CheckAvailableNeighbors(Stack<Cell> pointedCell, List<Direction> availableDirections, List<int> possibleNeighbours, int cellIndex)
        {
            // Calculate the indexes
            int cellX = cellIndex / mazeHeight; // example: 46 / 10 (height) = 4 (int). denominates float.
            int cellY = cellIndex % mazeHeight; // example: 46 % 10 = 6 remainder of height.

            // Checking available neighbors.
            if (cellX < mazeWidth - 1)  // Check right neighbour:
                CheckTheFollowingNeighbour(pointedCell, availableDirections, Direction.Right, possibleNeighbours, cellIndex + mazeHeight);

            if (cellX > 0) // Check left neighbour:
                CheckTheFollowingNeighbour(pointedCell, availableDirections, Direction.Left, possibleNeighbours, cellIndex - mazeHeight);

            if (cellY < mazeHeight - 1) // Check above neighbour: we use - 1 since it shouldn't count up to max number of height.
                CheckTheFollowingNeighbour(pointedCell, availableDirections, Direction.Up, possibleNeighbours, cellIndex + 1);

            if (cellY > 0) // Check below neighbour:
                CheckTheFollowingNeighbour(pointedCell, availableDirections, Direction.Down, possibleNeighbours, cellIndex - 1);
        }

        private void CheckTheFollowingNeighbour(Stack<Cell> pointedCell, List<Direction> directions, Direction dir, List<int> neighbours, int Index)
        {
            if (ThisNeighbourCellIsNotVisitedAndPointed(pointedCell, Index))
            {
                directions.Add(dir);
                neighbours.Add(Index);
            }
        }

        private bool ThisNeighbourCellIsNotVisitedAndPointed(Stack<Cell> pointedCell, int index)
        {
            return !visitedCells.Contains(totalCells[index]) && !pointedCell.Contains(totalCells[index]);
        }


        private void PickNextCellForWork(List<Direction> availableDirections, List<int> possibleNeighbours, Stack<Cell> pointedCell)
        {
            // Pick next cell:
            if (availableDirections.Count > 0)
            {
                // Pick a random Direction:
                int pointedDir = UnityEngine.Random.Range(0, availableDirections.Count);

                // Pick the next cell:
                Cell nextCell = totalCells[possibleNeighbours[pointedDir]];
                RemoveCellWalls(pointedCell, nextCell, availableDirections[pointedDir]);
                pointedCell.Push(nextCell);
            }
            else // BACKTRACKING
            {
                visitedCells.Add(pointedCell.Peek()); // Add the top element of the stack to visited list.
                pointedCell.Pop(); // pop top element
            }
        }

        private void RemoveCellWalls(Stack<Cell> pointedCell, Cell nextCell, Direction pointedCellDir)
        {
            pointedCell.Peek().RemoveWall((int)pointedCellDir);
            nextCell.RemoveWall((int)OppositeDirection(pointedCellDir));
        }

        #endregion

        #region UI_Listeners:

        private void BeginMazeSimulation()
        {
            CleanLists();
            InitializeMazeSizeValues();
            CreateNewMazeGrid(mazeWidth, mazeHeight);
        }

        private void DestroyMazeSimulation() => CleanLists();

        private void ChangeMazeWidth(int value)
        {
            mazeWidth_Slider = value;
        }
        private void ChangeMazeHeight(int value)
        {
            mazeHeight_Slider = value;
        }

        private void ChangeMazeGenerationMode(bool state)
        {
            isFastestMazeGeneration = state;
        }

        #endregion


    }

    public class UI_Backup
    {
        // Events:
        public static event Action<int> OnClickWidthValueChange;
        public static event Action<int> OnClickHeightValueChange;
        public static event Action<bool> OnClickMazeGenerationToggleChange;


        [Header("Panels")]
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private GameObject gamePanel;

        [Header("Maze Value Texts")]
        [SerializeField] private TMP_Text widthValueText;
        [SerializeField] private TMP_Text heightValueText;

        [Header("Sliders")]
        [SerializeField] private Slider widthSilder;
        [SerializeField] private Slider heightSilder;

        [Header("Buttons")]
        [SerializeField] private Button generateMazeBtn;
        [SerializeField] private Button playBtn;
        [SerializeField] private Button backBtn;
        [SerializeField] private Toggle generationToggle;

        public static event Action OnClickGenerateMaze;
        public static event Action OnClickDestroyMaze;
        private bool isFastestGeneration = false;

        private void Awake() => ConvertSlidersToInt();

        private void ConvertSlidersToInt()
        {
            widthSilder.wholeNumbers = true;
            heightSilder.wholeNumbers = true;
        }

        private void OnEnable()
        {
            // Buttons:
            generateMazeBtn.onClick.AddListener(GenerateMaze);
            playBtn.onClick.AddListener(StartMazeGame);
            backBtn.onClick.AddListener(ReturnToMainMenu);
            // Sliders:
            widthSilder.onValueChanged.AddListener(AdjustMazeWidth);
            heightSilder.onValueChanged.AddListener(AdjustMazeHeight);
            // Toggle:
            generationToggle.onValueChanged.AddListener(SetMazeGenerationMode);
        }

        private void OnDisable()
        {
            // Buttons:
            generateMazeBtn.onClick.RemoveAllListeners();
            playBtn.onClick.RemoveAllListeners();
            backBtn.onClick.RemoveAllListeners();
            // Sliders:
            widthSilder.onValueChanged.RemoveAllListeners();
            heightSilder.onValueChanged.RemoveAllListeners();
            // Toggle:
            generationToggle.onValueChanged.RemoveAllListeners();
        }

        void AdjustMazeWidth(float value)
        {
            SetMazeWidth(Mathf.RoundToInt(value));
        }
        void AdjustMazeHeight(float value)
        {
            SetMazeHeight(Mathf.RoundToInt(value));
        }

        private void SetMazeWidth(int widthValue)
        {
            widthSilder.value = widthValue;
            AdjustWidthText(widthValue);
            OnClickWidthValueChange?.Invoke(widthValue);
        }
        private void SetMazeHeight(int heightValue)
        {
            heightSilder.value = heightValue;
            AdjustHeightText(heightValue);
            OnClickHeightValueChange?.Invoke(heightValue);
        }

        private void AdjustWidthText(int widthValue)
        {
            widthValueText.text = widthValue.ToString();
        }
        private void AdjustHeightText(int heightValue)
        {
            heightValueText.text = heightValue.ToString();
        }


        private void GenerateMaze()
        {
            OnClickGenerateMaze?.Invoke();
        }

        private void StartMazeGame() => ShowGamePanel();

        private void ReturnToMainMenu()
        {
            ShowMenuPanel();
            OnClickDestroyMaze?.Invoke();
        }

        private void ShowGamePanel()
        {
            menuPanel.SetActive(false);
            gamePanel.SetActive(true);
        }
        private void ShowMenuPanel()
        {
            menuPanel.SetActive(true);
            gamePanel.SetActive(false);
        }

        private void SetMazeGenerationMode(bool state)
        {
            isFastestGeneration = !state;
            OnClickMazeGenerationToggleChange?.Invoke(state);
        }
    }


}


