using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private PathController pathController;
        [SerializeField] private TextMeshProUGUI currentDestText;
        [SerializeField] private TMP_InputField aliasInput;
        [SerializeField] private TextMeshProUGUI mapInput;
        [SerializeField] private PlaceSelectController placeSelectController;
        [SerializeField] private GameObject debugMenu;
        

        public bool seeAliases = false;
        public Dictionary<string, string> aliases = new Dictionary<string, string>();

        public PathController PathController => pathController;
        
        public void SelectMapAlias()
        {
            placeSelectController.Open(pathController.AvailableNodes.Select(item => item.Name).ToList(), id => { mapInput.text = id; });
        }

        public void AddAlias()
        {
            if (aliasInput.text.Length == 0 || mapInput.text.Length == 0 || aliases.ContainsKey(aliasInput.text)) return;
            aliases.Add(aliasInput.text, mapInput.text);
            aliasInput.text = "";
            mapInput.text = "";
        }

        public void RemoveAllAliases()
        {
            aliases = new Dictionary<string, string>();
        }
    
        public void CancelPath()
        {
            pathController.CancelPath();
            currentDestText.text = "";
        }

        public void OnAliasToggle(bool val)
        {
            seeAliases = !seeAliases;
        }

        public void OpenDebug()
        {
            debugMenu.SetActive(true);
        }
        
        public void CloseDebug()
        {
            debugMenu.SetActive(false);
        }
    }
}
