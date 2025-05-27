using UnityEngine;
using UnityEngine.UIElements;

public class GameClearUI : MonoBehaviour
{
    [SerializeField] UIDocument myUI;
    private Button restartButton;
    private Button quitButton;
    void Start()
    {
        VisualElement root = myUI.rootVisualElement;
        restartButton = root.Q<Button>("NextStage");
        quitButton = root.Q<Button>("Quit");
    }
    public void ClearUIDeactivate()
    {
        myUI.rootVisualElement.visible = false;
    }
    public void ClearUIActivate()
    {
        myUI.rootVisualElement.visible = true;
    }

}
