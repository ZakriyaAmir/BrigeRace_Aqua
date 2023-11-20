using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class BridgeTileScript : MonoBehaviour
{
    public int colorIndex;
    [SerializeField] Material[] brickMaterials;
    [SerializeField] private Renderer brickRenderer;

    Color targetColor;

    private void Awake()
    {
        colorIndex = -1;
    }

    public void ColorBrick(int _targetColorIndex)
    {
        brickRenderer.enabled = true;
        brickRenderer.material.SetColor("_Color", brickMaterials[_targetColorIndex].color);

        colorIndex = _targetColorIndex;

        targetColor = brickMaterials[_targetColorIndex].color;

        transform.GetChild(0).GetComponent<BoxCollider>().enabled = false;
        //Remove collider object
        //transform.GetChild(0).gameObject.SetActive(false);
    }

}
