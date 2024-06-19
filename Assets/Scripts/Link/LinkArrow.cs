using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public delegate void OnArrowEnd();

public class LinkArrow : MonoBehaviour
{
    [SerializeField] private LinkArrowAlpha arrow1, arrow2;
    [SerializeField] private float speed = 1;
    
    private float size = 0;
    private OnArrowEnd onArrowEnd;

    public void Initialize(float size, OnArrowEnd callback)
    {
        gameObject.SetActive(true);
        this.size = size;
        onArrowEnd = callback;
        transform.localPosition = new Vector3(0, 0, -(this.size / 2));
    }
    
    void Update()
    {
        if (transform.localPosition.z <= (size / 2))
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
