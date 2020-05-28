using System;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    private double _value;
    public static double _RATE = 0.05;

    public void SetValue(double value)
    {
        _value = value;
        GetComponent<TMPro.TMP_Text>().text = _value.ToString("0.####");
    }
    private void MoveValue(int change)
    {
        double value = _value + change * _RATE;
        SetValue(value - Math.Truncate(value));
    }

    public void UpdateMoveValue()
    {
        int change = 1;
        MoveValue(change);
    }

    public double GetValue() { return _value; }
}