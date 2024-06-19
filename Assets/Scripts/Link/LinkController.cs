using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkController : MonoBehaviour
{
    [SerializeField] private GameObject bg;
    [SerializeField] private float arrowDelay;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowContainer;
    
    private float size;
    private List<GameObject> objPool = new List<GameObject>();
    private float timeElapsed = 0;

    public void Initialize(float size)
    {
        this.size = size;
        
        Vector3 newScale = bg.transform.localScale;
        newScale.z = size; 
        bg.transform.localScale = newScale;
    }
    
    void Update()
    {
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
            
            arrow.Initialize(size, () => {objPool.Add(curr);});
        }
        else
        {
            timeElapsed += Time.deltaTime;
        }
    }
}
