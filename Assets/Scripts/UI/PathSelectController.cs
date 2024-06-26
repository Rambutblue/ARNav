using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Util;
using TMPro;
using UnityEngine;

public class PathSelectController : MonoBehaviour
{
    [SerializeField] private Fade fade;
    [SerializeField] private TextMeshProUGUI fromText;
    [SerializeField] private TextMeshProUGUI toText;
    [SerializeField] private PlaceSelectController placeSelectController;
    [SerializeField] private PathController pathController;

    private void Start()
    {
        Open();
    }

    public void Open()
    {
        gameObject.SetActive(true);
        fade.FadeIn();

        fromText.text = pathController.FindClosestNode()?.Name;
    }

    public void Close()
    {
        fade.FadeOut( () => {gameObject.SetActive(false);});
    }

    public void FromSelect()
    {
        placeSelectController.Open(pathController.AvailableNodes, id => { fromText.text = id; });
    }
    
    public void ToSelect()
    {
        placeSelectController.Open(pathController.AvailableNodes, id => { toText.text = id; });
    }

    public void StartPath()
    {
        //TODO path not found popup
        if (pathController.StartPath(fromText.text, toText.text))
        {
            Close();
        }
    }
}
