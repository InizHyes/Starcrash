using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundRepeating : MonoBehaviour
{
    [System.Serializable]
    public struct ParallaxLayer
    {
        public Transform transform;
        public float scrollSpeed;
    }

    public ParallaxLayer[] layers;

    private void Update()
    {
        foreach (var layer in layers)
        {
            Vector3 newPos = layer.transform.position;
            newPos.x -= layer.scrollSpeed * Time.deltaTime;

            // If the layer is out of view, move it to the right to create the illusion of continuous scrolling
            if (newPos.x < -GetLayerWidth(layer.transform))
            {
                newPos.x += GetLayerWidth(layer.transform) * 2;
            }

            layer.transform.position = newPos;
        }
    }

    private float GetLayerWidth(Transform layerTransform)
    {
        SpriteRenderer spriteRenderer = layerTransform.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            return spriteRenderer.bounds.size.x;
        }
        else
        {
            Debug.LogError("SpriteRenderer component not found on the layer!");
            return 0f;
        }
    }
}
