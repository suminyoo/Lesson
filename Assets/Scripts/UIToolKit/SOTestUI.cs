using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TestUI : MonoBehaviour
{
    [SerializeField] UIDocument myUI;

    public PlayerData playerData;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            playerData.HP -= 10;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            playerData.HP = 100;
        }

    }



    //private void Start()
    //{
    //    if (mySlider != null)
    //    {
    //        mySlider.RegisterValueChangedCallback(v =>
    //        {
    //            var oldValue = v.previousValue;
    //            var newValue = v.newValue;
    //            Debug.Log(v.newValue);
    //            myPlayer.changeATK(v.newValue);
    //        });
    //    }

    //}
    //public void SliderValueChange(float v)
    //{
    //    if (mySlider != null)
    //    {
    //        mySlider.value = v;
    //    }
    //}

}
