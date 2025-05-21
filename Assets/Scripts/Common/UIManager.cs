using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI hpText;


    void Start()
    {

    }
    public void ChangePlayerHp()
    {
        Debug.Log(player.hp);
        hpText.text = player.hp.ToString();
    }




}
