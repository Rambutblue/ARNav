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
    [SerializeField] private TextMeshProUGUI inputText;

    private OnPlaceSelected onPlaceSelected;
    private List<string> options = new List<string> { "apple", "banana", "apricot", "berry", "avocado", "blueberry" };

    private void Start()
    {
        List<string> words = new List<string> { "apple", "banana", "apricot", "berry", "avocado", "blueberry" };
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
                Close();
                onPlaceSelected?.Invoke(id);
            }));
        }
    }

    public void OnStringChanged(string param)
    {
        string filter = inputText.text;
        
        Debug.Log(filter);
        
        List<string> filtered = options
            .Where(word => word.StartsWith(filter))
            .OrderBy(word => word)
            .ToList();

        foreach (var item in filtered)
        {
            Debug.Log(item);
        }
        
        InstantiateItems(filtered);
    }
}
