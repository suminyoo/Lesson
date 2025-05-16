using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TestUI : MonoBehaviour
{
    [SerializeField] UIDocument myUI;
    [SerializeField] CubePlayer myPlayer;

    private Slider mySlider;

    private void Awake()
    {
        VisualElement root = myUI.rootVisualElement; //hook
        mySlider = root.Q<Slider>("ATKSlider");
    }

    private void Start()
    {
        if (mySlider != null)
        {
            mySlider.RegisterValueChangedCallback(v =>
            {
                var oldValue = v.previousValue;
                var newValue = v.newValue;
                Debug.Log(v.newValue);
                myPlayer.changeATK(v.newValue);
            });
        }
        
    }
    public void SliderValueChange(float v)
    {
        if (mySlider != null)
        {
            mySlider.value = v;
        }
    }

}
