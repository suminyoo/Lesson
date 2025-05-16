using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class MainUI : MonoBehaviour
{
    [SerializeField] UIDocument myUI;
    private VisualElement m_Root;
    private VisualElement m_Slider;
    private VisualElement m_Dragger;
    private VisualElement m_Bar;



    private Button topButton;
    private Button middleButton;
    private Button bottomButton;

    private Slider mySlider;

    private void Start()
    {
        VisualElement root = myUI.rootVisualElement; //hook

        topButton = root.Q<Button>("TopButton"); //Query, 해당 버튼을 찾기
        middleButton = root.Q<Button>("MiddleButton");
        bottomButton = root.Q<Button>("BottomButton");

        mySlider = root.Q<Slider>("MySlider");

        if (topButton != null)
            topButton.clicked += OnMyButtonClick;

        if (middleButton != null)
            middleButton.clicked += OnMyButtonClick02;

        if (mySlider != null)
        {
            Debug.Log("slider is "+ mySlider.value);

            mySlider.RegisterValueChangedCallback(v =>
            {
                var oldValue = v.previousValue;
                var newValue = v.newValue;
                Debug.Log(v.newValue);
            });

        }

    }


    private void OnMyButtonClick()
    {
        StartCoroutine("ZeroToHundredCor");

    }

    IEnumerator ZeroToHundredCor()
    {
        mySlider.value = 0f;
        for (int i = 0; i <= 100; i++)
        {
            mySlider.value += i;
            yield return new WaitForSeconds(0.1f);
        }
    }
    private void OnMyButtonClick02()
    {
        Debug.Log("MiddleButton Clicked");
    }

}
