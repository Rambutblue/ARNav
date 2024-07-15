using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkController : MonoBehaviour
{
    [SerializeField] private GameObject bg;
    [SerializeField] private float arrowDelay;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowContainer;
    
    public float size;
    private List<GameObject> objPool = new List<GameObject>();
    private float timeElapsed = 0;
    private PathNode fromNode, toNode;

    public void Initialize(PathNode from, PathNode to)
    {
        fromNode = from;
        toNode = to;
        
        
    }

    private void UpdatePosition()
    {
        size = Vector3.Distance(fromNode.gameObject.transform.position, toNode.gameObject.transform.position);
        
        Vector3 startPoint = fromNode.transform.position;
        Vector3 endPoint = toNode.transform.position;
        
        Vector3 midpoint = (startPoint + endPoint) / 2f;
        
        float distance = Vector3.Distance(startPoint, endPoint);
        
        gameObject.transform.position = midpoint;
        
        gameObject.transform.rotation = Quaternion.LookRotation(endPoint - startPoint);
        
        Vector3 newScale = bg.transform.localScale;
        newScale.z = size;
        bg.transform.localScale = newScale;
    }
    
    void Update()
    {
        UpdatePosition();
        
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
