using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InGameUI : MonoBehaviour
{
    [SerializeField] UIDocument myUI;
    [SerializeField] Player player;

    private ProgressBar hpbar;
    private Label stageLabel;
    private Label lifeLabel;
    private Label timeLabel;



    void Awake()
    {
        VisualElement root = myUI.rootVisualElement;
        hpbar = root.Q<ProgressBar>("HP");
        stageLabel = root.Q<Label>("Stage");
        lifeLabel = root.Q<Label>("Life");
        timeLabel = root.Q<Label>("Time");

    }
    private void Update()
    {
        //ShowTimeUI();

    }

    public void ShowTimeUI()
    {
        timeLabel.text = Time.time.ToString();
    }

    public void ChangePlayerHPUI(int var)
    {
        hpbar.value = var;
    }
    public void ChangeStageUI(int var)
    {
        Debug.Log("change stage var is " + var);
        Debug.Log("stageLabel " + stageLabel);
        stageLabel.text = "Stage: " + var.ToString();

    }
    public void ChangePlayerLifeUI(int var)
    {
        lifeLabel.text = "Life: " + var.ToString();
    }

}
