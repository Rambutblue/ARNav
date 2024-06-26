using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using Utils;

public class PathNode : MonoBehaviour
{
   //has to be unique name identifier
   [SerializeField] private string name;
   [SerializeField] private bool visible;
   //approx relative distance, doesnt need to be precise
   [SerializeField] private List<SerializablePair<PathNode, float>> neighbours;
   [SerializeField] private GameObject NameTag;
   [SerializeField] private TextMeshProUGUI NameText;

   private void Start()
   {
      NameTag.SetActive(visible);
      NameText.text = Name;
   }

   public string Name => name;

   public bool Visible => visible;

   public List<SerializablePair<PathNode, float>> Neighbours => neighbours;
   
   
}
