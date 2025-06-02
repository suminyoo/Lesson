using System;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerDeadUI : MonoBehaviour
{
    [SerializeField] UIDocument myUI;
    [SerializeField] GameManager manager;

    private Button respawnButton;
    private Button quitButton;

    private Label playerRemainLife;

    public static event Action OnPlayerRespawnEvent;

    void Awake()
    {
        VisualElement root = myUI.rootVisualElement;

        playerRemainLife = root.Q<Label>("RemainLifeLabel");

        respawnButton = root.Q<Button>("Respawn");
        quitButton = root.Q<Button>("Quit");

        if (respawnButton != null) respawnButton.clicked += PlayerRespawn;
        if (quitButton != null) quitButton.clicked += GameQuit;
    }
    public void ChangePlayerRemainLifeUI(int r_life)
    {
        playerRemainLife.text = "Lives remaining: " + (r_life - 1).ToString();
    }

    public void PlayerRespawn ()
    {
        OnPlayerRespawnEvent.Invoke();
    }
    public void GameQuit()
    {
        manager.GameQuit();
    }
    public void ShowPlayerDeadUI(bool boo)
    {
        myUI.rootVisualElement.visible = boo;
    }
}
