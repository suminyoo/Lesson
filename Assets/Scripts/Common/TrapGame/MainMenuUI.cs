using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    public static event Action OnGameStart;

    [SerializeField] UIDocument myUI;

    private Button startButton;
    private Button continueButton;
    private Button optionButton;
    private Button quitButton;


    private void Start()
    {
        VisualElement root = myUI.rootVisualElement; //hook

        startButton = root.Q<Button>("StartButton"); //Query, 해당 버튼을 찾기
        continueButton = root.Q<Button>("ContinueButton");
        optionButton = root.Q<Button>("OptionButton");
        quitButton = root.Q<Button>("QuitButton");


        if (startButton != null)
            startButton.clicked += OnStartButtonClick;

        if (continueButton != null)
            continueButton.clicked += OnContinueButtonClick;

        if (optionButton != null)
            optionButton.clicked += OnOptionButtonClick;

        if (quitButton != null)
            quitButton.clicked += OnQuitButtonClick;

    }
    void OnStartButtonClick()
    {
        Debug.Log("Start Button Clicked!");
        OnGameStart.Invoke();
    }
    void OnContinueButtonClick()
    {
        Debug.Log("Continue Button Clicked!");
    }
    void OnOptionButtonClick()
    {
        Debug.Log("Option Button Clicked!");

    }
    void OnQuitButtonClick()
    {
        Debug.Log("Quit Button Clicked!");
        Application.Quit();
    }
    public void ShowUI(bool boo) => myUI.rootVisualElement.visible = boo;

}
