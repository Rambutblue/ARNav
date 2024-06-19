using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkArrowAlpha : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    private Material materialInstance;

    void Start()
    {
        /*
        materialInstance = meshRenderer.material;
        float rnd = Random.value;
        Debug.Log("[ALPHA]" + rnd);
        rnd = rnd < 0.5f ? 0.1f : 1f;
        ChangeObjectAlpha(rnd);
        */
    }

    public void ChangeObjectAlpha(float alpha)
    {
        if (materialInstance != null)
        {
            // Get the current color of the material
            Color color = materialInstance.color;

            // Set the alpha value
            color.a = alpha;

            // Apply the new color with the modified alpha
            materialInstance.color = color;
        }
    }
}
