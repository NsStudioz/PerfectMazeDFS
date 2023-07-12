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

    // Button Events:
    public static event Action OnClickGenerateMaze;
    public static event Action OnClickDestroyMaze;
    // Silders Events:
    public static event Action<int> OnClickWidthValueChange;
    public static event Action<int> OnClickHeightValueChange;
    // Toggle Events:
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

    #region Buttons:

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

    #endregion


    #region Sliders:

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

    #endregion


    #region Toggle:

    private void SetMazeGenerationMode(bool state)
    {
        isFastestGeneration = !state;
        OnClickMazeGenerationToggleChange?.Invoke(state);
    }

    #endregion


}
