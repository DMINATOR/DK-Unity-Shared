using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Add this component to the Camera
/// </summary>
[RequireComponent(typeof(BackgroundParallaxLocator))]
public class BackgroundParallax : MonoBehaviour
{
    //private float length;
    //private float startPosition; // Starting position of background layer on X

    //public GameObject camera1; // Reference for the camera
    //public float parallaxEffect;


    //Exposed

    [Header("Locator")]

    [Tooltip("Locator")]
    public BackgroundParallaxLocator Locator;

    // Start is called before the first frame update
    void Start()
    {
        // Calculate initial positions of the sprite renderers
        foreach(var layer in Locator.Layers)
        {
            layer.StartPosition = layer.SpriteRenderer.gameObject.transform.position.x;
            layer.Length = layer.SpriteRenderer.bounds.size.x;
        }

        //startPosition = transform.position.x;
       // length = GetComponent<SpriteRenderer>().bounds.size.x; // Size of sprite in pixels
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var layer in Locator.Layers)
        {
            UpdateLayer(layer);
        }  
    }

    private void UpdateLayer(BackgroundParallaxLayer layer)
    {
        // Calculate camera relative positions
        float cameraPosition = transform.position.x * (1 - layer.ParallaxEffect);
        float distance = transform.position.x * layer.ParallaxEffect;

        // Get current layer position
        var layerTransformPosition = layer.SpriteRenderer.gameObject.transform.position;

        // Calculate new position for the layer
        layer.SpriteRenderer.gameObject.transform.position = new Vector3(layer.StartPosition + distance, layerTransformPosition.y, layerTransformPosition.z);

        // Correct if moved outside of the layer bounds
        if (cameraPosition > layer.StartPosition + layer.Length)
        {
            layer.StartPosition += layer.Length;
        }
        else if (cameraPosition < layer.StartPosition - layer.Length)
        {
            layer.StartPosition -= layer.Length;
        }
    }
}
