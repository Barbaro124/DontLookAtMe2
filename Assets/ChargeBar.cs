using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{

    public Slider slider;
    public Image fillImage;

    public void SetCharge(float charge)
    {
        slider.value = charge;
        
    }

    public void SetColor(Color color)
    {
        fillImage.color = color;
    }


}
