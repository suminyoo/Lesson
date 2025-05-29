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

    private Label[] trapLabelList = new Label[4];
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

        trapLabelList[0] = root.Q<Label>("Trap01Num");
        trapLabelList[1] = root.Q<Label>("Trap02Num");
        trapLabelList[2] = root.Q<Label>("Trap03Num");
        trapLabelList[3] = root.Q<Label>("Trap04Num");
        totalTrapDamageLabel = root.Q<Label>("TotalTrapDamage");
    }
    private void Start()
    {
        Player.OnPlayerSpeedChangeEvent += ChangeSpeedUI;
        Player.OnPlayerJumpPowChangeEvent += ChangeJumpPowerUI;
        GameManager.OnPaused += Pause;

        jumpLabel.visible = false;
        speedLabel.visible = false;
        ResetTimer();
    }
    private void Update()
    {
        if (isPaused) return;
        ShowTimeUI();
    }
    public void ChangeDifficultyUI(Trap[] trapList, int[] trapNumList, float totalDamage)
    {
        for (int i = 0; i < trapLabelList.Length; i++)
        {
            trapLabelList[i].text = trapList[i].gameObject.name.ToString()+" : "+trapNumList[i].ToString();
        }
        totalTrapDamageLabel.text = "Total Trap Damage: " + totalDamage.ToString();
    }
    private void Pause(bool boo)
    {
        isPaused = boo;
    }
    public void ChangeSpeedUI(float normalSpeed, float curSpeed)
    {
        if (curSpeed > normalSpeed)
        {
            speedLabel.text = "Speed UP!";
            speedLabel.visible = true;
        }
        else if (curSpeed < normalSpeed)
        {
            speedLabel.text = "Speed Down!";
            speedLabel.visible = true;
        }
        else
        {
            speedLabel.visible = false;
        }
    }
    public void ChangeJumpPowerUI(float normalJumpPow, float curJumpPow)
    {
        if (curJumpPow > normalJumpPow){
            jumpLabel.text = "Jump Power UP!";
            jumpLabel.visible = true;
        }
        else if (curJumpPow < normalJumpPow){
            jumpLabel.text = "Jump Power Down!";
            jumpLabel.visible = true;
        }
        else{
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
