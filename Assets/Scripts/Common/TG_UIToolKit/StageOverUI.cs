using System;
using UnityEngine;
using UnityEngine.UIElements;

public class StageOverUI : MonoBehaviour
{
    [SerializeField] UIDocument myUI;
    public static event Action OnRestartStageEvent;

    private Button restartButton;
    private Button quitButton;

    void Start()
    {
        VisualElement root = myUI.rootVisualElement;

        restartButton = root.Q<Button>("Restart");
        quitButton = root.Q<Button>("Quit");

        if (restartButton != null) restartButton.clicked += StageRestart;
        if (quitButton != null) quitButton.clicked += GameQuit;
    }
    public void StageRestart() => OnRestartStageEvent.Invoke();
    public void GameQuit() => Application.Quit();
    public void ShowUI(bool boo) => myUI.rootVisualElement.visible = boo;
    
}
