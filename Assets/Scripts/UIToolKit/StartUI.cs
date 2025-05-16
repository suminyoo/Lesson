using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.UIElements;

public class StartUI : MonoBehaviour
{
    [SerializeField] UIDocument myUI;
    [SerializeField] UIDocument startUI;
    [SerializeField] UIDocument optionUI;
    

    private Button startButton;
    private Button continueButton;
    private Button optionButton;
    private Button quitButton;


    private void Start()
    {
        VisualElement root = myUI.rootVisualElement; //hook
        VisualElement m_option = optionUI.rootVisualElement; //hook

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
    }
    void OnContinueButtonClick()
    {
        Debug.Log("Continue Button Clicked!");

    }
    void OnOptionButtonClick()
    {
        Debug.Log("Option Button Clicked!");

        //root.visible = true;
    }
    void OnQuitButtonClick()
    {
        Debug.Log("Quit Button Clicked!");
        Application.Quit();
    }
}
