using System;
using UnityEngine;

public class ButtonBehaviour : MonoBehaviour
{
    private double _value;
    public static double _RATE = 0.05;


    void Update()
    {
        Display();
    }
    public void SetValue(double value)
    {
        _value = value;
    }
    public void MoveValue(int change)
    {
        double value = _value + change * _RATE;
        _value = value - Math.Truncate(value);
    }

    public void UpdateMoveValue()
    {
        int change = 1;
        MoveValue(change);
    }

    private void Display()
    {
        GetComponent<TMPro.TMP_Text>().text = System.Convert.ToString(_value);
    }

    public double GetValue() { return _value; }
}