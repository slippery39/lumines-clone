using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class Skin : MonoBehaviour
{

    [SerializeField]
    private float _bpm;
    [SerializeField]
    private GameBlockPiece _blockPiece;
    [SerializeField]
    private GameObject _background;
    [SerializeField]
    private HighlightedSquare _highlightedSquare;

    public AudioSource Music { get { return GetComponent<AudioSource>(); } }
    public float BPM { get { return _bpm; } }
    public GameBlockPiece BlockPiece { get { return _blockPiece; } }

    public GameObject Background { get { return _background; } }
    public HighlightedSquare HighlightedSquare { get { return _highlightedSquare; } }
   
}


