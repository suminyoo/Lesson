using UnityEngine;
using UnityEngine.UIElements;
public class GameClearEndUI : MonoBehaviour
{
    [SerializeField] UIDocument myUI;
    private Button quitButton;

    void Start()
    {
        VisualElement root = myUI.rootVisualElement;
        quitButton = root.Q<Button>("Quit");

        if (quitButton != null) quitButton.clicked += GameQuit;
    }
    public void GameQuit() => Application.Quit();
    public void ShowGameClearUI(bool boo) => myUI.rootVisualElement.visible = boo;
}
