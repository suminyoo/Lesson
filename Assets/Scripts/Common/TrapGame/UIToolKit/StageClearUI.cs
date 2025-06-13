using System;
using UnityEngine;
using UnityEngine.UIElements;
public class StageClearUI : MonoBehaviour
{
    [SerializeField] UIDocument myUI;
    public static event Action OnNextStageEvent;
    private Button nextStageButton;
    private Button quitButton;

    void Start()
    {
        VisualElement root = myUI.rootVisualElement;

        nextStageButton = root.Q<Button>("NextStage");
        quitButton = root.Q<Button>("Quit");

        if (nextStageButton != null) nextStageButton.clicked += NextStage;
        if (quitButton != null) quitButton.clicked += GameQuit;
    }
    public void NextStage() => OnNextStageEvent.Invoke();
    public void GameQuit() => Application.Quit();
    public void ShowUI(bool boo) => myUI.rootVisualElement.visible = boo;
}
