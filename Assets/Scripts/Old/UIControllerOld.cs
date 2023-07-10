using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerOld : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private MazeGeneratorOld mazeGenerator;

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

    public static event Action OnClickGenerateMaze;
    public static event Action OnClickDestroyMaze;
    //OnClickCheckInstantMazeGenerate?.Invoke(isInstant);
    //public static event Action<bool> OnClickCheckInstantMazeGenerate;
    private bool isInstant = false;

    private void Awake()
    {
        widthSilder.wholeNumbers = true;
        heightSilder.wholeNumbers = true;
        isInstant = false;
    }

    private void OnEnable()
    {
        generateMazeBtn.onClick.AddListener(GenerateMaze);
        playBtn.onClick.AddListener(StartMazeGame);
        backBtn.onClick.AddListener(ReturnToMainMenu);
        //
        MazeGeneratorOld.OnClickWidthValueChange += UpdateWidthText;
        MazeGeneratorOld.OnClickHeightValueChange += UpdateHeightText;
    }

    private void OnDisable()
    {
        generateMazeBtn.onClick.RemoveAllListeners();
        playBtn.onClick.RemoveAllListeners();
        backBtn.onClick.RemoveAllListeners();
        //
        MazeGeneratorOld.OnClickWidthValueChange -= UpdateWidthText;
        MazeGeneratorOld.OnClickHeightValueChange -= UpdateHeightText;
    }

    private void UpdateWidthText(int newValue)
    {
        widthValueText.text = newValue.ToString();
    }

    private void UpdateHeightText(int newValue)
    {
        heightValueText.text = newValue.ToString();
    }

    private void GenerateMaze()
    {
        OnClickGenerateMaze?.Invoke();
    }

    public void InstantMazeGenerateChecker()
    {
        isInstant = !isInstant;
        mazeGenerator.SetInstantGenerateBool(isInstant);
    }

    private void StartMazeGame()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
    }
    private void ReturnToMainMenu()
    {
        gamePanel.SetActive(false);
        menuPanel.SetActive(true);
        //
        OnClickDestroyMaze?.Invoke();
    }


}
