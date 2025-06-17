using UnityEngine;
using UnityEngine.UIElements;

public class InGameUI : MonoBehaviour
{
    [SerializeField] UIDocument myUI;
    public TG_PlayerData playerData;
    public Player player;

    private Label timeLabel;
    private Label jumpLabel;
    private Label speedLabel;

    //private Label[] trapLabelList = new Label[4];
    private Label totalTrapDamageLabel;

    private float time;
    public bool isPaused = false;

    void Awake()
    {
        VisualElement root = myUI.rootVisualElement;
        timeLabel = root.Q<Label>("Time");

        jumpLabel = root.Q<Label>("JumpPower");
        speedLabel = root.Q<Label>("Speed");

        //trapLabelList[0] = root.Q<Label>("Trap01Num");
        //trapLabelList[1] = root.Q<Label>("Trap02Num");
        //trapLabelList[2] = root.Q<Label>("Trap03Num");
        //trapLabelList[3] = root.Q<Label>("Trap04Num");
        //totalTrapDamageLabel = root.Q<Label>("TotalTrapDamage");
    }
    private void Start()
    {
        player.OnPlayerSpeedChangeEvent += ChangeSpeedUI;
        player.OnPlayerJumpPowChangeEvent += ChangeJumpPowerUI;
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
        //for (int i = 0; i < trapLabelList.Length; i++)
        //{
        //    trapLabelList[i].text = trapList[i].gameObject.name.ToString()+" : "+trapNumList[i].ToString();
        //}
        //totalTrapDamageLabel.text = "Total Trap Damage: " + totalDamage.ToString();
    }
    public void ChangeSpeedUI(float curSpeed)
    {
        if (curSpeed > playerData.normalSpeed)
        {
            speedLabel.text = "Speed UP!";
            speedLabel.visible = true;
        }
        else if (curSpeed < playerData.normalSpeed)
        {
            speedLabel.text = "Speed Down!";
            speedLabel.visible = true;
        }
        else
        {
            speedLabel.visible = false;
        }
    }
    public void ChangeJumpPowerUI(float curJumpPow)
    {
        if (curJumpPow > playerData.normalJumpPow)
        {
            jumpLabel.text = "Jump Power UP!";
            jumpLabel.visible = true;
        }
        else if (curJumpPow < playerData.normalJumpPow)
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
        timeLabel.text = string.Format("{0:D2}:{1:D2}", (int)time / 60, ((int)time - (int)time / 60 * 60) % 60 );
    }
    private void Pause(bool boo) => isPaused = boo;
    public void ResetTimer() => time = 0;
    public void ShowUI(bool boo) => myUI.rootVisualElement.visible = boo;

}
