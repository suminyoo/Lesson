using UnityEngine;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] UIDocument myUI;
    private Button restartButton;
    private Button quitButton;
    void Start()
    {
        VisualElement root = myUI.rootVisualElement;
        restartButton = root.Q<Button>("Restart");
        quitButton = root.Q<Button>("Quit");
    }
    public void GameOverUIDeactivate()
    {
        myUI.rootVisualElement.visible = false;
    }
    public void GameOverUIActivate()
    {
        myUI.rootVisualElement.visible = true;
    }

}
