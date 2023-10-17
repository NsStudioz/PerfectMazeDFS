using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PerfectMazeDFS
{
    public class Cell : MonoBehaviour
    {

        [SerializeField] private GameObject[] cellWalls = null;

        void Awake() => InitializeCellWalls();

        private void InitializeCellWalls()
        {
            cellWalls = new GameObject[transform.childCount];

            for (int i = 0; i < cellWalls.Length; i++)
                cellWalls[i] = transform.GetChild(i).gameObject;
        }

        public void RemoveWall(int index) => SetWallState(index, false);

        /// <summary>
        /// sets the cell's entire game object visibility
        /// </summary>
        /// <param name="state"></param>
        public void SetCellGameObjectVisibility(bool state)
        {
            gameObject.SetActive(state);
        }

        /// <summary>
        /// sets the cell's specific wall game object visibility
        /// </summary>
        /// <param name="index"></param>
        /// <param name="state"></param>
        private void SetWallState(int index, bool state)
        {
            cellWalls[index].SetActive(state);
        }
    }
}