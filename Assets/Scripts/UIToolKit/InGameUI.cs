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

    public void UIShowTime()
    {
        timeLabel.text = Time.time.ToString();
    }

    public void UIChangePlayerHP(int var)
    {
        Debug.Log("UI UIChangePlayerHP: " + var);

        hpbar.value = var;
    }
    public void UIChangeStage(int var)
    {
        Debug.Log("UI UIChangeStage: " + var);

        stageLabel.text = "Stage: " + var.ToString();

    }
    public void UIChangePlayerLife(int var)
    {
        Debug.Log("UI UIChangePlayerLife: " + var);

        lifeLabel.text = "Life: " + var.ToString();
    }

}
