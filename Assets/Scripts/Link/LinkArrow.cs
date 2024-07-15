using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public delegate void OnArrowEnd();

public class LinkArrow : MonoBehaviour
{
    [SerializeField] private LinkArrowAlpha arrow1, arrow2;
    [SerializeField] private float speed = 1;

    private LinkController linkController;   
    private OnArrowEnd onArrowEnd;

    public void Initialize(LinkController linkController, OnArrowEnd callback)
    {
        gameObject.SetActive(true);
        this.linkController = linkController;
        onArrowEnd = callback;
        transform.localPosition = new Vector3(0, 0, -(this.linkController.size / 2));
    }
    
    void Update()
    {
        if (transform.localPosition.z <= (linkController.size / 2))
        {
            Vector3 movement = Vector3.forward * (speed * Time.deltaTime);

            transform.Translate(movement, Space.Self);
        }
        else
        {
            onArrowEnd?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
