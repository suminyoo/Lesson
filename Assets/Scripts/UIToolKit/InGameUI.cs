using UnityEditor.SceneManagement;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static System.Net.Mime.MediaTypeNames;
using System;


public class InGameUI : MonoBehaviour
{
    [SerializeField] UIDocument myUI;

    private ProgressBar hpbar;
    private Label stageLabel;
    private Label lifeLabel;
    private Label timeLabel;

    private Label jumpLabel;
    private Label speedLabel;

    public int normalSpeed;
    public int normalJumpPow;

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

    public void ChangeSpeedUI(int speed)
    {
        if (speed == normalSpeed) return;
        else if(speed > normalSpeed)
        {
            speedLabel.text = "Speed UP!";
            speedLabel.visible = true;
        }
        else if (speed < normalSpeed)
        {
            speedLabel.text = "Speed Down!";
            speedLabel.visible = true;
        }

    }
    public void ChangeJumpPowerUI(int jumpPow)
    {
        if (jumpPow == normalJumpPow) return;
        else if (jumpPow > normalJumpPow)
        {
            jumpLabel.text = "Jump Power UP!";
            jumpLabel.visible = true;
        }
        else if (jumpPow < normalJumpPow)
        {
            jumpLabel.text = "Jump Power Down!";
            jumpLabel.visible = true;
        }

        
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
