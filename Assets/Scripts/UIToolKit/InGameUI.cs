using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InGameUI : MonoBehaviour
{
    [SerializeField] UIDocument myUI;
    [SerializeField] Player player;

    private Label hpVar ;
    void Start()
    {
        VisualElement root = myUI.rootVisualElement;
        hpVar = root.Q<Label>("HPVar");
    }

    public void ChangePlayerHP(int var)
    {
        hpVar.text = var.ToString();
    }

}
