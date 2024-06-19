using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform camTransform;
    private Vector3 offset = new Vector3(0, 180, 0);
    void Start()
    {
        camTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(camTransform);
        transform.Rotate(offset);
    }
}
