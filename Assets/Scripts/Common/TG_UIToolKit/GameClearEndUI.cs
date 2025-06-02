using UnityEngine;
using UnityEngine.UIElements;
public class GameClearEndUI : MonoBehaviour
{
    [SerializeField] UIDocument myUI;
    GameManager manager;
    private Button quitButton;

    void Start()
    {
        VisualElement root = myUI.rootVisualElement;

        quitButton = root.Q<Button>("Quit");

        if (quitButton != null) quitButton.clicked += GameQuit;
    }
    public void GameQuit()
    {
        manager.GameQuit();
    }
    public void ShowGameClearUI(bool boo)
    {
        myUI.rootVisualElement.visible = boo;
    }
}
