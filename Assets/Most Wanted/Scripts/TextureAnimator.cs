using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextureAnimator : MonoBehaviour
{
    public float scrollSpeed = 0.5f; // Velocidad de desplazamiento de la textura
    private RawImage raw;
    private Vector2 offset = Vector2.zero;
    [SerializeField] bool x;
    [SerializeField] bool y;

    void Start()
    {
        raw = GetComponent<RawImage>();
    }

    void Update()
    {
        // Calcula el desplazamiento basado en el tiempo y la velocidad
        float offsetX = Time.time * scrollSpeed;
        float offsetY = Time.time * scrollSpeed;

        // Aplica el desplazamiento a las coordenadas de textura
        if(x)offset.x = offsetX;
        if(y)offset.y = offsetY;

        // Aplica el offset al material
        // Aplica el offset a la imagen
        raw.uvRect = new Rect(offset.x, offset.y, raw.uvRect.width, raw.uvRect.height);
    }
}