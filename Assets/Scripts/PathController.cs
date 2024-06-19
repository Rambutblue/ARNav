using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

public class PathController : MonoBehaviour
{
    [SerializeField] private List<PathNode> availableNodes;
    [SerializeField] private GameObject linkPrefab;

    private void Start()
    {
        var path = FindPath("1", "4");
        foreach (var node in path)
        {
            Debug.Log(node.Name);
        }

        for (int i = 0; i < path.Count - 1; i++)
        {
            CreateLink(path[i], path[i + 1]);
        }
    }

    private void CreateLink(PathNode start, PathNode end)
    {
        Vector3 startPoint = start.transform.position;
        Vector3 endPoint = end.transform.position;
        
        Vector3 midpoint = (startPoint + endPoint) / 2f;
        
        float distance = Vector3.Distance(startPoint, endPoint);

        GameObject newLink = Instantiate(linkPrefab);
        
        newLink.transform.position = midpoint;
        
        newLink.transform.rotation = Quaternion.LookRotation(endPoint - startPoint);

        LinkController newLinkController = newLink.GetComponent<LinkController>();
        newLinkController.Initialize(distance);
    }

    private List<PathNode> FindPath(string start, string end)
    {
        List<PathNode> result = new List<PathNode>();

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

        if (startNode == null) return result;

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