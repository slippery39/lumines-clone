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
    private string name;

    [SerializeField]
    private float _bpm;
    [SerializeField]
    private GameBlockPiece _blockPiece;
    [SerializeField]
    private GameObject _background;
    [SerializeField]
    private HighlightedSquare _highlightedSquare;
    [SerializeField]
    private GameObject _beatNumbers;
    [SerializeField]
    private GameObject _scoreBoard;
    [SerializeField]
    private GameObject _grid;

    public AudioSource Music { get { return GetComponent<AudioSource>(); } }
    public float BPM { get { return _bpm; } }
    public GameBlockPiece BlockPiece { get { return _blockPiece; } }

    public GameObject Background { get { return _background; } }
    public HighlightedSquare HighlightedSquare { get { return _highlightedSquare; } }
    public GameObject BeatNumbers { get => _beatNumbers; set => _beatNumbers = value; }
    public GameObject ScoreBoard { get => _scoreBoard; set => _scoreBoard = value; }
    public string Name { get => name; set => name = value; }
    public GameObject Grid { get => _grid; set => _grid = value; }
}


