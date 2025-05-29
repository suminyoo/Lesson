using System;
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

    private Label trap01Label;
    private Label trap02Label;
    private Label trap03Label;
    private Label trap04Label;

    private Label totalTrapDamageLabel;

    private float time;
    public bool isPaused = false;

    void Awake()
    {
        VisualElement root = myUI.rootVisualElement;
        hpbar = root.Q<ProgressBar>("HP");
        stageLabel = root.Q<Label>("Stage");
        lifeLabel = root.Q<Label>("Life");
        timeLabel = root.Q<Label>("Time");

        jumpLabel = root.Q<Label>("JumpPower");
        speedLabel = root.Q<Label>("Speed");


        trap01Label = root.Q<Label>("Trap01Num");
        trap02Label = root.Q<Label>("Trap02Num");
        trap03Label = root.Q<Label>("Trap03Num");
        trap04Label = root.Q<Label>("Trap04Num");

        totalTrapDamageLabel = root.Q<Label>("TotalTrapDamage");

        jumpLabel.visible = false;
        speedLabel.visible = false;
    }

    private void Start()
    {
        Player.OnPlayerSpeedChangeEvent += ChangeSpeedUI;
        Player.OnPlayerJumpPowChangeEvent += ChangeJumpPowerUI;

        GameManager.OnPaused += Pause;

        ResetTimer();
    }
    private void Update()
    {
        if (isPaused) return;
        ShowTimeUI();
        
    }
    private void Pause(bool boo)
    {
        isPaused = boo;
    }

    
    public void ChangeDifficultyUI(int[] trapNumList, float totalDamage)
    {
        trap01Label.text = "spikeTrap#: " + trapNumList[0].ToString();
        trap02Label.text = "hammerTrap#: " + trapNumList[1].ToString();
        trap03Label.text = "poisonTrap3#: " + trapNumList[2].ToString();
        trap04Label.text = "hiddenBombTrap#: " + trapNumList[3].ToString();

        totalTrapDamageLabel.text = "Total Trap Damage: " + totalDamage.ToString();
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
    public void ShowTimeUI()
    {
        time += Time.deltaTime;
        int min = (int)time / 60;
        int sec = ((int)time - min * 60) % 60;
        timeLabel.text = string.Format("{0:D2}:{1:D2}", min, sec );
    }
    public void ResetTimer()
    {
        time = 0;
    }


    public void ChangePlayerHPUI(int var)
    {
        hpbar.value = var;
    }
    public void ChangeStageUI(int var)
    {
        stageLabel.text = "Stage: " + var.ToString();
    }
    public void ChangePlayerLifeUI(int var)
    {
        lifeLabel.text = "Life: " + var.ToString();
    }

}
