using UnityEngine;
using UnityEngine.UIElements;


public class InGameUI : MonoBehaviour
{
    [SerializeField] UIDocument myUI;

    private ProgressBar hpbar;
    private Label stageLabel;
    private Label lifeLabel;
    private Label timeLabel;

    private Label jumpLabel;
    private Label speedLabel;

    void Awake()
    {
        VisualElement root = myUI.rootVisualElement;
        hpbar = root.Q<ProgressBar>("HP");
        stageLabel = root.Q<Label>("Stage");
        lifeLabel = root.Q<Label>("Life");
        timeLabel = root.Q<Label>("Time");

        jumpLabel = root.Q<Label>("JumpPower");
        speedLabel = root.Q<Label>("Speed");
        
        jumpLabel.visible = false;
        speedLabel.visible = false;
    }

    private void Start()
    {
        Player.OnPlayerSpeedChangeEvent += ChangeSpeedUI;
        Player.OnPlayerJumpPowChangeEvent += ChangeJumpPowerUI;
    }
    private void Update()
    {
        //ShowTimeUI();
    }

    public void ChangeSpeedUI(float normalSpeed, float maxSpeed)
    {
        if (maxSpeed > normalSpeed)
        {
            speedLabel.text = "Speed UP!";
            speedLabel.visible = true;
        }
        else if (maxSpeed < normalSpeed)
        {
            speedLabel.text = "Speed Down!";
            speedLabel.visible = true;
        }
        else
        {
            speedLabel.visible = false;
        }
    }
    public void ChangeJumpPowerUI(float normalJumpPow, float maxJumpPow)
    {
        if (maxJumpPow > normalJumpPow)
        {
            jumpLabel.text = "Jump Power UP!";
            jumpLabel.visible = true;
        }
        else if (maxJumpPow < normalJumpPow)
        {
            jumpLabel.text = "Jump Power Down!";
            jumpLabel.visible = true;
        }
        else
        {
            jumpLabel.visible = false;

        }

    }
    public void UIShowTime()
    {
        timeLabel.text = Time.time.ToString();
    }
    public void UIChangePlayerHP(int var)
    {
        hpbar.value = var;
    }
    public void UIChangeStage(int var)
    {
        stageLabel.text = "Stage: " + var.ToString();
    }
    public void UIChangePlayerLife(int var)
    {
        lifeLabel.text = "Life: " + var.ToString();
    }

}
