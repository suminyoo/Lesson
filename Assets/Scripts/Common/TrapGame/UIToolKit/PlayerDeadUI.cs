using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDeadUI : MonoBehaviour
{
    [SerializeField] UIDocument myUI;
    private Button respawnButton;
    private Button quitButton;
    public static event Action OnPlayerRespawnEvent;

    void Awake()
    {
        VisualElement root = myUI.rootVisualElement;
        respawnButton = root.Q<Button>("Respawn");
        quitButton = root.Q<Button>("Quit");

        if (respawnButton != null) respawnButton.clicked += PlayerRespawn;
        if (quitButton != null) quitButton.clicked += GameQuit;
    }
    public void PlayerRespawn() => OnPlayerRespawnEvent.Invoke();
    public void GameQuit() => Application.Quit();
    public void ShowUI(bool boo) => myUI.rootVisualElement.visible = boo;
}
