using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [Header("Slider Deðiþkenleri")]
    [SerializeField]
    private float currentValue;
    [SerializeField]
    private float maxValue;

    [Space]
    [SerializeField]
    private Slider slider;

    public void SetMaxValue(float value)
    {
        Debug.Log("Set Max Value : " + value);

        maxValue = value;

        if (slider)
            slider.maxValue = maxValue;
    }

    public void SetCurrentValue(float value)
    {
        Debug.Log("Set Current Value : " + value);

        currentValue = value;

        if (slider)
            slider.value = currentValue;
    }
}

