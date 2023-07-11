using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGeneratorOld : MonoBehaviour
{
    [Header("Cell Prefab")]
    [SerializeField] private CellOld cellPrefab;
    [SerializeField] private List<CellOld> totalCells = new List<CellOld>();
    //[SerializeField] private List<int> visibleCells = new List<int>();
    [SerializeField] private List<CellOld> visitedCells = new List<CellOld>();
    [SerializeField] private bool isInstantGenerate = false;

    [SerializeField] private bool isClicked = false;
    [SerializeField] private IEnumerator mazeGridAnimationCoroutine;
    [SerializeField] private IEnumerator mazeGenerationCoroutine;

    [Header("Maze Size")]
    [SerializeField]
    [Range(10, 20)]
    private int mazeWidth;

    [SerializeField]
    [Range(10, 25)]
    private int mazeHeight;

    // EVENTS:
    public static event Action<int> OnClickWidthValueChange;
    public static event Action<int> OnClickHeightValueChange;

    public enum Direction
    {
        Right,
        Left,
        Up,
        Down
    }

    private void Start()
    {
        mazeGridAnimationCoroutine = StartGridAnimation(mazeWidth, mazeHeight);
        mazeGenerationCoroutine = GenerateTheMaze(mazeWidth, mazeHeight);
    }

    private void OnEnable()
    {
        UIControllerOld.OnClickGenerateMaze += SetMaze;
        UIControllerOld.OnClickDestroyMaze += DestroyMaze;
    }

    private void OnDisable()
    {
        UIControllerOld.OnClickGenerateMaze -= SetMaze;
        UIControllerOld.OnClickDestroyMaze -= DestroyMaze;
    }

    private void OnDestroy()
    {
        StopCoroutine(mazeGridAnimationCoroutine);
        StopCoroutine(mazeGenerationCoroutine);
    }

    /*    private void Update()
        {
            if (Input.GetKey(KeyCode.S))
            {
                //StopCoroutine(StartGridAnimation());
                //StopCoroutine(GenerateTheMazeCopy(mazeWidth, mazeHeight));
            }
        }*/

    public void AdjustMazeWidth(float newValue)
    {
        mazeWidth = Mathf.RoundToInt(newValue);
        OnClickWidthValueChange?.Invoke(mazeWidth);
    }

    public void AdjustMazeHeight(float newValue)
    {
        mazeHeight = Mathf.RoundToInt(newValue);
        OnClickHeightValueChange?.Invoke(mazeHeight);
    }

    private void SetMaze()
    {
        if (isClicked)
        {   // Stops animations after first maze generation.
            StopCoroutine(mazeGridAnimationCoroutine);
            StopCoroutine(mazeGenerationCoroutine);
        }

        if (isInstantGenerate) // check if the maze should generate instantly or with animations.
        {   // Generate Instantly:
            CreateTheGrid(mazeWidth, mazeHeight);
            GenerateTheMazeInstant(mazeWidth, mazeHeight);
        }
        else
        {   // Generate the grid, the grid is invisible
            CreateTheGrid(mazeWidth, mazeHeight);
            // Generate the grid with animation
            mazeGridAnimationCoroutine = StartGridAnimation(mazeWidth, mazeHeight);
            StartCoroutine(mazeGridAnimationCoroutine);
        }
    }

    private void DestroyMaze()
    {
        if (isClicked)
        {
            StopCoroutine(mazeGridAnimationCoroutine);
            StopCoroutine(mazeGenerationCoroutine);
        }

        if (totalCells.Count > 0)
        {
            foreach (CellOld cellInstance in totalCells)
                Destroy(cellInstance.gameObject);

            totalCells.Clear();
            visitedCells.Clear();
        }
    }

    private IEnumerator StartGridAnimation(int _width, int _height)
    {
        for (int i = 0; i < totalCells.Count; i++)
        {
            yield return new WaitForSeconds(0.05f);
            totalCells[i].SetCellVisible(true);
        }
        // Start the maze algorithm (Depth First Search):
        mazeGenerationCoroutine = GenerateTheMaze(_width, _height);
        StartCoroutine(mazeGenerationCoroutine);
    }


    private void CreateTheGrid(int _width, int _height)
    {
        isClicked = true;

        if (totalCells.Count > 0)
        {
            foreach (CellOld cellInstance in totalCells)
                Destroy(cellInstance.gameObject);

            totalCells.Clear();
            visitedCells.Clear();
        }

        // Create nodes:
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector3 nodePos = new Vector3(x, 0, y); // Decided not to center the maze.
                CellOld newCell = Instantiate(cellPrefab, nodePos, Quaternion.identity);

                if (isInstantGenerate)
                    newCell.SetCellVisible(true);
                else
                    newCell.SetCellVisible(false);

                totalCells.Add(newCell);
            }
        }
    }

    private IEnumerator GenerateTheMaze(int _width, int _height)
    {
        // using a stack instead of a list:
        Stack<CellOld> pointedCell = new Stack<CellOld>();

        pointedCell.Push(totalCells[UnityEngine.Random.Range(0, totalCells.Count)]); // choose random cell. Add it to the stack.

        while (visitedCells.Count < totalCells.Count)
        {
            // check for possible cell neighbours next to the pointed cell:
            List<int> possibleNeighbours = new List<int>();
            List<Direction> availableDirs = new List<Direction>(); // 0 = pos-x. 1 = neg-x, 2 = pos-z, 3 = neg-z

            int pointedCellIndex = totalCells.IndexOf(pointedCell.Peek());
            int pointedCellX = pointedCellIndex / _height; // example: 46 / 10 (height) = 4 (int). denominates float.
            int pointedCellY = pointedCellIndex % _height; // example: 46 % 10.

            //Debug.Log(pointedCellIndex);
            //Debug.Log(pointedCellX);
            //Debug.Log(pointedCellY);

            // Check right neighbour:
            if (pointedCellX < _width - 1)
            {
                if (CheckNotVisitedAndPointedCells(pointedCell, pointedCellIndex + _height))
                {
                    availableDirs.Add(Direction.Right);
                    possibleNeighbours.Add(pointedCellIndex + _height); // add the possible right neighbor to the list.
                }
            }

            // Check left neighbour:
            if (pointedCellX > 0)
            {
                if (CheckNotVisitedAndPointedCells(pointedCell, pointedCellIndex - _height))
                {
                    availableDirs.Add(Direction.Left); // neg-x.
                    possibleNeighbours.Add(pointedCellIndex - _height); // add the possible left neighbor to the list.
                }
            }

            // Check above neighbour:
            if (pointedCellY < _height - 1) // we use - 1 since it shouldn't count up to max number of height.
            {
                if (CheckNotVisitedAndPointedCells(pointedCell, pointedCellIndex + 1))
                {
                    availableDirs.Add(Direction.Up); // pos-y
                    possibleNeighbours.Add(pointedCellIndex + 1); // add the possible above neighbor to the list.
                }
            }

            // Check below neighbour:
            if (pointedCellY > 0)
            {
                if (CheckNotVisitedAndPointedCells(pointedCell, pointedCellIndex - 1))
                {
                    availableDirs.Add(Direction.Down); // neg-y
                    possibleNeighbours.Add(pointedCellIndex - 1); // add the possible below neighbor to the list.
                }
            }

            // Pick next cell:
            if (availableDirs.Count > 0)
            {
                // Pick a random Direction:
                int pointedDir = UnityEngine.Random.Range(0, availableDirs.Count);

                // Pick the next cell:
                CellOld nextCell = totalCells[possibleNeighbours[pointedDir]];

                // First, remove the walls of the pointed and next cells:
                switch (availableDirs[pointedDir])
                {
                    case Direction.Right: // if pos-x (right)
                        pointedCell.Peek().RemoveWall((int)Direction.Right); // removing the current node's right wall
                        nextCell.RemoveWall((int)Direction.Left); // new wall needs to get rid of left wall
                        break;
                    case Direction.Left: // if neg-x (left)
                        pointedCell.Peek().RemoveWall((int)Direction.Left); // removing the current node's left wall
                        nextCell.RemoveWall((int)Direction.Right); // new wall needs to get rid of right wall
                        break;
                    case Direction.Up: // if pos-z (up)
                        pointedCell.Peek().RemoveWall((int)Direction.Up); // removing current node's top wall
                        nextCell.RemoveWall((int)Direction.Down); // removing new node's bottom wall
                        break;
                    case Direction.Down: // if neg-z (down)
                        pointedCell.Peek().RemoveWall((int)Direction.Down); // removing current node's bottom wall
                        nextCell.RemoveWall((int)Direction.Up); // removing new node's top wall
                        break;
                    default:
                        break;
                }

                // Add the new cell to the stack:
                pointedCell.Push(nextCell);

            }
            else // BACKTRACKING
            {
                visitedCells.Add(pointedCell.Peek()); // Add the top element of the stack to visited list.
                pointedCell.Pop(); // pop top element
            }

            // avoids crashing:
            yield return new WaitForSeconds(0f);
        }
    }


    private void GenerateTheMazeInstant(int _width, int _height)
    {
        // using a stack instead of a list:
        Stack<CellOld> pointedCell = new Stack<CellOld>();

        pointedCell.Push(totalCells[UnityEngine.Random.Range(0, totalCells.Count)]); // choose random cell. Add it to the stack.

        while (visitedCells.Count < totalCells.Count)
        {
            // check for possible cell neighbours next to the pointed cell:
            List<int> possibleNeighbours = new List<int>();
            List<Direction> availableDirs = new List<Direction>(); // 0 = pos-x. 1 = neg-x, 2 = pos-z, 3 = neg-z

            int pointedCellIndex = totalCells.IndexOf(pointedCell.Peek());
            int pointedCellX = pointedCellIndex / _height; // example: 46 / 10 (height) = 4 (int). denominates float.
            int pointedCellY = pointedCellIndex % _height; // example: 46 % 10 = 6 remainder of height.

            // Check right neighbour:
            if (pointedCellX < _width - 1)
            {
                if (CheckNotVisitedAndPointedCells(pointedCell, pointedCellIndex + _height))
                {
                    availableDirs.Add(Direction.Right);
                    possibleNeighbours.Add(pointedCellIndex + _height); // add the possible right neighbor to the list.
                }
            }

            // Check left neighbour:
            if (pointedCellX > 0)
            {
                if (CheckNotVisitedAndPointedCells(pointedCell, pointedCellIndex - _height))
                {
                    availableDirs.Add(Direction.Left); // neg-x.
                    possibleNeighbours.Add(pointedCellIndex - _height); // add the possible left neighbor to the list.
                }
            }

            // Check above neighbour:
            if (pointedCellY < _height - 1) // we use - 1 since it shouldn't count up to max number of height.
            {
                if (CheckNotVisitedAndPointedCells(pointedCell, pointedCellIndex + 1))
                {
                    availableDirs.Add(Direction.Up); // pos-y
                    possibleNeighbours.Add(pointedCellIndex + 1); // add the possible above neighbor to the list.
                }
            }

            // Check below neighbour:
            if (pointedCellY > 0)
            {
                if (CheckNotVisitedAndPointedCells(pointedCell, pointedCellIndex - 1))
                {
                    availableDirs.Add(Direction.Down); // neg-y
                    possibleNeighbours.Add(pointedCellIndex - 1); // add the possible below neighbor to the list.
                }
            }

            // Pick next cell:
            if (availableDirs.Count > 0)
            {
                // Pick a random Direction:
                int pointedDir = UnityEngine.Random.Range(0, availableDirs.Count);

                // Pick the next cell:
                CellOld nextCell = totalCells[possibleNeighbours[pointedDir]];

                // First, remove the walls of the pointed and next cells:
                switch (availableDirs[pointedDir])
                {
                    case Direction.Right: // if pos-x (right)
                        pointedCell.Peek().RemoveWall((int)Direction.Right); // removing the current node's right wall
                        nextCell.RemoveWall((int)Direction.Left); // new wall needs to get rid of left wall
                        break;
                    case Direction.Left: // if neg-x (left)
                        pointedCell.Peek().RemoveWall((int)Direction.Left); // removing the current node's left wall
                        nextCell.RemoveWall((int)Direction.Right); // new wall needs to get rid of right wall
                        break;
                    case Direction.Up: // if pos-z (up)
                        pointedCell.Peek().RemoveWall((int)Direction.Up); // removing current node's top wall
                        nextCell.RemoveWall((int)Direction.Down); // removing new node's bottom wall
                        break;
                    case Direction.Down: // if neg-z (down)
                        pointedCell.Peek().RemoveWall((int)Direction.Down); // removing current node's bottom wall
                        nextCell.RemoveWall((int)Direction.Up); // removing new node's top wall
                        break;
                }
                // Add the new cell to the stack:
                pointedCell.Push(nextCell);
            }
            else // BACKTRACKING
            {
                visitedCells.Add(pointedCell.Peek()); // Add the top element of the stack to visited list.
                pointedCell.Pop(); // pop top element
            }
        }
        return; // avoids crashing:
    }

    // Checks for neighbour cells availability:
    private bool CheckNotVisitedAndPointedCells(Stack<CellOld> pointedCell, int index)
    {
        return !visitedCells.Contains(totalCells[index]) && !pointedCell.Contains(totalCells[index]);
    }

    public void SetInstantGenerateBool(bool state)
    {
        isInstantGenerate = state;
    }


    #region Trash_Code:
    /*    private void CreateTheGridOld(int _width, int _height)
        {
            if (totalCells.Count > 0)
            {
                visitedCells.Clear();

                foreach (var cell in totalCells)
                {
                    Destroy(cell.gameObject);
                }
                totalCells.Clear();
            }

            // Create nodes:
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    Vector3 nodePos = new Vector3(x, 0, y); // Decided not to center the maze.
                    Cell newCell = Instantiate(cellPrefab, nodePos, Quaternion.identity);
                    totalCells.Add(newCell);
                }
            }
        }

        // using a coroutine as animation.
        private IEnumerator GenerateTheMazeOld(int _width, int _height)
        {
            // using a stack instead of a list:
            Stack<Cell> pointedCell = new Stack<Cell>();

            pointedCell.Push(totalCells[UnityEngine.Random.Range(0, totalCells.Count)]); // choose random cell. Add it to the stack.

            //currentPath[0].SetState(NodeState.Current);

            while (visitedCells.Count < totalCells.Count)
            {
                // check for possible cells next to the pointed cell:
                List<int> possibleNeighbours = new List<int>();
                //List<int> availableDirs = new List<int>(); // 0 = pos-x. 1 = neg-x, 2 = pos-z, 3 = neg-z
                List<Direction> availableDirs = new List<Direction>();

                int pointedCellIndex = totalCells.IndexOf(pointedCell.Peek());
                int pointedCellX = pointedCellIndex / _height; // example: 46 / 10 (height) = 4 (int). denominates float.
                int pointedCellY = pointedCellIndex % _height; // example: 46 % 10 = 6 remainder of height.
                Debug.Log(pointedCellIndex);
                Debug.Log(pointedCellX);
                Debug.Log(pointedCellY);

                // Check right neighbour:
                if (pointedCellX < _width - 1)
                {
                    if (CheckNotVisitedAndPointedCells(pointedCell, pointedCellIndex + _height))
                    {
                        availableDirs.Add(Direction.Right);
                        possibleNeighbours.Add(pointedCellIndex + _height); // add the possible right neighbor to the list.
                    }
                }

                // Check left neighbour:
                if (pointedCellX > 0)
                {
                    if (CheckNotVisitedAndPointedCells(pointedCell, pointedCellIndex - _height))
                    {
                        availableDirs.Add(Direction.Left); // neg-x.
                        possibleNeighbours.Add(pointedCellIndex - _height); // add the possible left neighbor to the list.
                    }
                }

                // Check above neighbour:
                if (pointedCellY < _height - 1) // we use - 1 since it shouldn't count up to max number of height.
                {
                    if (CheckNotVisitedAndPointedCells(pointedCell, pointedCellIndex + 1))
                    {
                        availableDirs.Add(Direction.Up); // pos-y
                        possibleNeighbours.Add(pointedCellIndex + 1); // add the possible above neighbor to the list.
                    }
                }

                // Check below neighbour:
                if (pointedCellY > 0)
                {
                    if (CheckNotVisitedAndPointedCells(pointedCell, pointedCellIndex - 1))
                    {
                        availableDirs.Add(Direction.Down); // neg-y
                        possibleNeighbours.Add(pointedCellIndex - 1); // add the possible below neighbor to the list.
                    }
                }

                // Pick next cell:
                if (availableDirs.Count > 0)
                {
                    // Pick a random Direction:
                    int pointedDir = UnityEngine.Random.Range(0, availableDirs.Count);

                    // Pick the next cell:
                    Cell nextCell = totalCells[possibleNeighbours[pointedDir]];

                    // First, remove the walls of the pointed and next cells:
                    switch (availableDirs[pointedDir])
                    {
                        case Direction.Right: // if pos-x (right)
                            pointedCell.Peek().RemoveWall((int)Direction.Right); // removing the current node's right wall
                            nextCell.RemoveWall((int)Direction.Left); // new wall needs to get rid of left wall
                            break;
                        case Direction.Left: // if neg-x (left)
                            pointedCell.Peek().RemoveWall((int)Direction.Left); // removing the current node's left wall
                            nextCell.RemoveWall((int)Direction.Right); // new wall needs to get rid of right wall
                            break;
                        case Direction.Up: // if pos-z (up)
                            pointedCell.Peek().RemoveWall((int)Direction.Up); // removing current node's top wall
                            nextCell.RemoveWall((int)Direction.Down); // removing new node's bottom wall
                            break;
                        case Direction.Down: // if neg-z (down)
                            pointedCell.Peek().RemoveWall((int)Direction.Down); // removing current node's bottom wall
                            nextCell.RemoveWall((int)Direction.Up); // removing new node's top wall
                            break;
                    }

                    // Add the new cell to the stack:
                    pointedCell.Push(nextCell);

                    //chosenNode.SetState(NodeState.Current); // set the enum state to current, not important.
                }
                else // BACKTRACKING
                {
                    visitedCells.Add(pointedCell.Peek()); // Add the top element of the stack to visited list.
                    pointedCell.Pop(); // pop top element
                }

                // avoids crashing:
                yield return new WaitForSeconds(0.01f);
            }
        }*/

    /*    private void StartGridAnimationCopy()
    {
        if(!isInstantGenerate && isClicked)
        {
            for (int i = 0; i < totalCells.Count; i++)
            {
                float timeElapsed = 0;
                timeElapsed += Time.deltaTime;
                Debug.Log(timeElapsed);
                if (timeElapsed >= 0.25f)
                {
                    totalCells[i].SetCellVisible(true);
                }
            }
        }
    }*/
    #endregion

}
