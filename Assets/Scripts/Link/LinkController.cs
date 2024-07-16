using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkController : MonoBehaviour
{
    [SerializeField] private GameObject bg;
    [SerializeField] private float arrowDelay;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowContainer;
    [SerializeField] private GameObject inactiveLink, activeLink;
    
    public float size;
    private List<GameObject> objPool = new List<GameObject>();
    private float timeElapsed = 0;
    private PathNode fromNode, toNode;
    private Vector3 relativePos;

    public void Initialize(PathNode from, PathNode to, Vector3 relativePos)
    {
        fromNode = from;
        toNode = to;
        this.relativePos = relativePos;
    }

    private void UpdatePosition()
    {
        Vector3 startPoint = fromNode.transform.position;
        Vector3 endPoint = toNode.gameObject.activeInHierarchy
            ? toNode.transform.position
            : fromNode.transform.position + relativePos;
        
        size = Vector3.Distance(startPoint, endPoint);
        
        Vector3 midpoint = (startPoint + endPoint) / 2f;

        gameObject.transform.position = midpoint;
        
        gameObject.transform.rotation = Quaternion.LookRotation(endPoint - startPoint);
        
        Vector3 newScale = bg.transform.localScale;
        newScale.z = size;
        bg.transform.localScale = newScale;
        inactiveLink.transform.localScale = newScale;
    }
    
    void Update()
    {
        UpdatePosition();

        inactiveLink.SetActive(!toNode.gameObject.activeInHierarchy);
        activeLink.SetActive(toNode.gameObject.activeInHierarchy);
        arrowContainer.gameObject.SetActive(toNode.gameObject.activeInHierarchy);
        

        if (timeElapsed > arrowDelay)
        {
            timeElapsed = 0;
            GameObject curr;

            if (objPool.Count > 0)
            {
                curr = objPool[objPool.Count - 1];
                objPool.RemoveAt(objPool.Count - 1);
            }
            else
            {
                curr = Instantiate(arrowPrefab, arrowContainer);
            }

            LinkArrow arrow = curr.GetComponent<LinkArrow>();
            
            arrow.Initialize(this, () => {objPool.Add(curr);});
        }
        else
        {
            timeElapsed += Time.deltaTime;
        }
    }
}
