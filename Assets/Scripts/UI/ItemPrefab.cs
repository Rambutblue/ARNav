using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemPrefab : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textField;

    private string currText;
    //TODO callback

    public void Bind(string text)
    {
        currText = text;
        textField.text = currText;
    }

    public void Select()
    {
        //callback
    }
}
