using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;

    private string currText;
    private OnPlaceSelected onPlaceSelected;

    public void Bind(string text, OnPlaceSelected callback)
    {
        currText = text;
        textField.text = currText;
        onPlaceSelected = callback;
    }

    public void Select()
    {
        onPlaceSelected?.Invoke(currText);
    }
}
