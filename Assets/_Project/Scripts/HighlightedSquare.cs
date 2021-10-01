using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightedSquare : MonoBehaviour
{

    [SerializeField]
    private BlockTypes gameColor = BlockTypes.Color1;
    public BlockTypes Color { get => gameColor; set => gameColor = value; }

    [SerializeField]
    //Color for the first color type
    private Color color1;

    [SerializeField]
    //Color to show for the second color type.
    private Color color2;

    [SerializeField]
    Renderer render;
    
    [SerializeField]
    private GameObject square;

    void Start()
    {

        render = square.GetComponent<Renderer>();
    }

    void Update()
    {
        Debug.Log("Is update firing?");
        Debug.Log(gameColor);
        if (gameColor == BlockTypes.Color1)
        {
               
            square.GetComponent<Renderer>().material.color=color1;
        }
        else if (gameColor == BlockTypes.Color2)
        {
            square.GetComponent<Renderer>().material.color = color2;
        }
        else
        {
            Debug.Log("Is update firing?");
            throw new System.Exception("Invalid BlockType detected for a Highlighted Square");
        }

    }
}
