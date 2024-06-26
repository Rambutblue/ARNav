using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Scripts.Util;
using TMPro;
using UnityEngine;


public delegate void OnPlaceSelected(string id);

public class PlaceSelectController : MonoBehaviour
{
    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject prefab;
    [SerializeField] private TMP_InputField inputText;
    [SerializeField] private Fade fade;

    private OnPlaceSelected onPlaceSelected;
    private List<PathNode> options = new List<PathNode>();

    public void Open(List<PathNode> options, OnPlaceSelected callback)
    {
        gameObject.SetActive(true);
        fade.FadeIn();
        onPlaceSelected = callback;
        this.options = options;
        InstantiateItems(options);
    }

    private void Close()
    {
        fade.FadeOut(() => gameObject.SetActive(false));
    }

    private void InstantiateItems(List<PathNode> items)
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }
        foreach (var item in items)
        {
            GameObject newItem = Instantiate(prefab, contentTransform);
            ItemPrefab newItemPrefab = newItem.GetComponent<ItemPrefab>();
            newItemPrefab.Bind(item.Name, (id =>
            {
                Close();
                onPlaceSelected?.Invoke(id);
            }));
        }
    }

    public void OnStringChanged(string param)
    {
        string filter = inputText.text;
        
        Debug.Log(filter);
        
        List<PathNode> filtered = options
            .Where(item => item.Name.StartsWith(filter, StringComparison.OrdinalIgnoreCase))
            .OrderBy(name => name.Name)
            .ToList();

        InstantiateItems(filtered);
    }
}
