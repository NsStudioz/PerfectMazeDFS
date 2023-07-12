using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DFS_MazeGenerator;
using UnityEditor.PackageManager;
using UnityEngine.Events;

public class UI : MonoBehaviour
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

}
