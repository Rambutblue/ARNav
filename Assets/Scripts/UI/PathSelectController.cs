using System.Linq;
using Scripts.Util;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PathSelectController : MonoBehaviour
    {
        [SerializeField] private Fade fade;
        [SerializeField] private TextMeshProUGUI fromText;
        [SerializeField] private TextMeshProUGUI toText;
        [SerializeField] private PlaceSelectController placeSelectController;
        [SerializeField] private UIController uiController;
        [SerializeField] private TextMeshProUGUI destText;
    
        public void Open()
        {
            gameObject.SetActive(true);
            fade.FadeIn();

            fromText.text = uiController.PathController.FindClosestNode()?.Name;
        }

        public void Close()
        {
            fade.FadeOut( () => {gameObject.SetActive(false);});
        }

        public void FromSelect()
        {
            placeSelectController.Open(uiController.PathController.AvailableNodes.Where(item => item.Visible).Select(item => item.Name).ToList(), id => { fromText.text = id; });
        }
    
        public void ToSelect()
        {
            placeSelectController.Open(uiController.seeAliases ? uiController.aliases.Keys.ToList() : uiController.PathController.AvailableNodes.Where(item => item.Visible).Select(item => item.Name).ToList(), id => { toText.text = id; });
        }

        public void StartPath()
        {
            //TODO path not found popup

            string to = uiController.seeAliases ? uiController.aliases[toText.text] : toText.text;
            string from = fromText.text;
            
            if (!uiController.PathController.StartPath(from, to)) return;
            destText.text = toText.text;
            Close();
        }
    }
}
