using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightedSquare : MonoBehaviour, ILuminesGameUpdateable
{

    [SerializeField]
    private BlockTypes gameColor = BlockTypes.Color1;
    public BlockTypes Color { get => gameColor; set => gameColor = value; }
    public Color Color1 { get => color1; set => color1 = value; }
    public Color Color2 { get => color2; set => color2 = value; }

    [SerializeField]
    //Color for the first color type
    private Color color1;

    [SerializeField]
    //Color to show for the second color type.
    private Color color2;
    
    [SerializeField]
    private GameObject square;

    public void LuminesGameUpdate()
    {
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
            throw new System.Exception("Invalid BlockType detected for a Highlighted Square");
        }

    }
}
