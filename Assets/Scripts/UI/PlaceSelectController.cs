using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;


public delegate void OnPlaceSelected(string id);

public class PlaceSelectController : MonoBehaviour
{
    [SerializeField] private Transform contentTransform;
    [SerializeField] private GameObject prefab;
    [SerializeField] private TMP_InputField inputText;

    private OnPlaceSelected onPlaceSelected;
    private List<string> options = new List<string> { "apple", "banana", "apricot", "berry", "avocado", "blueberry" };

    private void Start()
    {
        List<string> words = options;
        Open(words, id => { });
    }

    public void Open(List<string> options, OnPlaceSelected callback)
    {
        gameObject.SetActive(true);
        onPlaceSelected = callback;
        this.options = options;
        InstantiateItems(options);
    }

    private void Close()
    {
        gameObject.SetActive(false);
    }

    private void InstantiateItems(List<string> items)
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }
        foreach (var item in items)
        {
            GameObject newItem = Instantiate(prefab, contentTransform);
            ItemPrefab newItemPrefab = newItem.GetComponent<ItemPrefab>();
            newItemPrefab.Bind(item, (id =>
            {
                Debug.Log("[DEBUG] here");
                Close();
                onPlaceSelected?.Invoke(id);
            }));
        }
    }

    public void OnStringChanged(string param)
    {
        string filter = inputText.text; //String.Copy(inputText.text);
        
        Debug.Log(filter);
        
        List<string> filtered = options
            .Where(word => word.StartsWith(filter, StringComparison.OrdinalIgnoreCase))
            .OrderBy(word => word)
            .ToList();

        filtered.Sort();

        InstantiateItems(filtered);
    }
}
