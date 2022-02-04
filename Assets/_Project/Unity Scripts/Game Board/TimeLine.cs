using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeLine : MonoBehaviour
{
    [SerializeField]
    private int _amountDeleted = 0;
    public int AmountDeleted { get => _amountDeleted; set { _amountDeleted = value; countText.SetText(value.ToString()); } }

    [SerializeField]
    private TextMeshPro countText;
}
