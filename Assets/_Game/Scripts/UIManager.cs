using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : GOSingleton<UIManager>
{
    [SerializeField] GameObject waitPanel;
    [SerializeField] GameObject endGamePanel;
    [SerializeField] MapBuilder mapBuilder;
    [SerializeField] LevelManager levelManager;
    [SerializeField] TextMeshProUGUI pointText;
    [SerializeField] GameObject upLevelButton;
    public TextMeshProUGUI PointText { get => pointText; set => pointText = value; }
    public GameObject UpLevelButton { get => upLevelButton; set => upLevelButton = value; }

    public void Start()
    {
        waitPanel.SetActive(true);
    }
    public void StartGame()
    {
        waitPanel.SetActive(false);
        MapBuilder.GetInstance().OnInit();
    }
    public void EndGameMenu(bool flag)
    {
        endGamePanel.SetActive(flag);
    }
    
    
}
