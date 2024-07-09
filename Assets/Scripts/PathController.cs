using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Utils;

public class PathController : MonoBehaviour
{
    [SerializeField] private List<PathNode> availableNodes;
    [SerializeField] private GameObject linkPrefab;
    [SerializeField] private Transform linkContainerPrev;
    [SerializeField] private Transform linkContainerNext;
    [SerializeField] private GameObject player;

    private List<PathNode> currentPath = new List<PathNode>();
    private int currClosest = -5;
    public List<PathNode> AvailableNodes => availableNodes;

    public bool StartPath(string start, string end)
    {
        if (start == end) return false;
        currentPath = FindPath(start, end);
        currClosest = -2;
        return currentPath.Count > 0;
    }

    public void CancelPath()
    {
        currentPath = new List<PathNode>();
        foreach (Transform child in linkContainerPrev)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in linkContainerNext)
        {
            Destroy(child.gameObject);
        }
    }

    private void Update()
    {
        int closest = FindClosestPathIndex();
        if (closest < 0) return;
        if (currentPath.Count <= 0 || closest == currClosest) return;
        if (currClosest + 1 == closest)
        {
            foreach (Transform child in linkContainerPrev)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform path in linkContainerNext)
            {
                path.transform.SetParent(linkContainerPrev);
            }
            if (closest != currentPath.Count - 1)
            {
                CreateLink(currentPath[closest], currentPath[closest + 1], linkContainerNext);
            }
        }
        else if (currClosest - 1 == closest)
        {
            foreach (Transform child in linkContainerNext)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform path in linkContainerPrev)
            {
                path.transform.SetParent(linkContainerNext);
            }
            if (closest != 0)
            {
                CreateLink(currentPath[closest - 1], currentPath[closest], linkContainerPrev);
            }
        }
        else
        {
            UpdateFullPath(closest);
        }

        currClosest = closest;
    }

    private void UpdateFullPath(int closest)
    {
        foreach (Transform child in linkContainerPrev)
        {
            Destroy(child.gameObject);
        }
                
        foreach (Transform child in linkContainerNext)
        {
            Destroy(child.gameObject);
        }
            
        if (closest != 0)
        {
            CreateLink(currentPath[closest - 1], currentPath[closest], linkContainerPrev);
        }
        if (closest != currentPath.Count - 1)
        {
            CreateLink(currentPath[closest], currentPath[closest + 1], linkContainerNext);
        }
    }

    public PathNode FindClosestNode()
    {
        List<PathNode> currNodes = availableNodes.Where(node => node.Visible).ToList();
        if (currNodes.Count <= 0)
        {
            return null;
        }

        PathNode best = currNodes[0];
        float bestDist = Vector3.Distance(currNodes[0].transform.position, player.transform.position);

        foreach (var node in currNodes)
        {
            float currDist = Vector3.Distance(node.transform.position, player.transform.position);
            if (currDist < bestDist)
            {
                best = node;
                bestDist = currDist;
            }
        }

        return best;
    }

    private int FindClosestPathIndex()
    {
        if (currentPath.Count <= 0)
        {
            return -1;
        }

        int best = 0;
        float bestDist = Vector3.Distance(currentPath[0].transform.position, player.transform.position);

        for (int i = 0; i < currentPath.Count; i++)
        {
            float currDist = Vector3.Distance(currentPath[i].transform.position, player.transform.position);
            if (currDist < bestDist)
            {
                best = i;
                bestDist = currDist;
            }
        }

        return best;
    }

    private void CreateLink(PathNode start, PathNode end, Transform linkContainer)
    {
        Vector3 startPoint = start.transform.position;
        Vector3 endPoint = end.transform.position;
        
        Vector3 midpoint = (startPoint + endPoint) / 2f;
        
        float distance = Vector3.Distance(startPoint, endPoint);

        GameObject newLink = Instantiate(linkPrefab, linkContainer);
        
        newLink.transform.position = midpoint;
        
        newLink.transform.rotation = Quaternion.LookRotation(endPoint - startPoint);

        LinkController newLinkController = newLink.GetComponent<LinkController>();
        newLinkController.Initialize(distance);
    }

    private List<PathNode> FindPath(string start, string end)
    {
        List<PathNode> result = new List<PathNode>();

        if (availableNodes.Count <= 0) return result;

        PathNode startNode = null;
        PathNode endNode = null;

        var queue = new PriorityQueue<PathNode>();

        Dictionary<PathNode, PathNode> visited = new Dictionary<PathNode, PathNode>();
        Dictionary<PathNode, float> distances = new Dictionary<PathNode, float>();
        
        foreach (var node in availableNodes)
        {
            if (end == node.Name)
            {
                endNode = node;
            }
            if (start == node.Name)
            {
                startNode = node;
                distances[node] = 0;
                continue;
            }

            distances[node] = -1;
        }

        if (startNode == null || endNode == null) return result;

        queue.Enqueue(startNode, 0);

        while (!queue.Empty())
        {
            var curr = queue.Dequeue();
            if (curr.Name == end) break;
            
            foreach (var next in curr.Neighbours)
            {
                float newCost = distances[curr] + next.second;
                if (visited.ContainsKey(next.first) && !(newCost < distances[next.first])) continue;
                
                distances[next.first] = newCost;
                float priority = newCost;
                queue.Enqueue(next.first, priority);
                visited[next.first] = curr;
            }
            
        }

        if (distances[endNode] < 0)
        {
            return result;
        }

        var reconstructingNode = endNode;
        while (reconstructingNode != startNode)
        {
            result.Add(reconstructingNode);
            reconstructingNode = visited[reconstructingNode];
        }
        result.Add(startNode);
        result.Reverse();

        return result;
    }
}


//unity .NET standard doesnt include pqueue :))
public class PriorityQueue<T>
{
    private List<KeyValuePair<T, float>> elements = new List<KeyValuePair<T, float>>();

    public int Count => elements.Count;

    public void Enqueue(T item, float priority)
    {
        elements.Add(new KeyValuePair<T, float>(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Value < elements[bestIndex].Value)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Key;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }

    public bool Empty()
    {
        return Count <= 0;
    }
}