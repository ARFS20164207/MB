using ARFS.Tools;
using System;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(fileName = "Theme",menuName = "MostWanted/Themes",order = 1)]
public class GameTheme : ScriptableObject
{
    [Header("Game Materials")]
    public Material ColorATable;
    public Material ColorBTable;
    public Material ColorAPiece;
    public Material ColorBPiece;
    public Material ColorBasePiece;
    public Material ColorDetailPiece;

    public Material ColorSelected;
    public Material ColorAttacked;
    public Material ColorCellCheckmark;

    [Header("Game Prefabs")]
    public GameObject prefabCell;

    [Tooltip("Add all piece of MB in order from 1 to MB(10)")]
    [SerializeField]private RendMaterials[] prefabPieces = new RendMaterials[10];
    [HideInInspector] public RendMaterials prefabPiece1 { get {return prefabPieces[(int)MathF.Min(0,prefabPieces.Length-1)]; } }
    [HideInInspector] public RendMaterials prefabPiece2 { get { return prefabPieces[(int)MathF.Min(1, prefabPieces.Length - 1)]; } }
    [HideInInspector] public RendMaterials prefabPiece3 { get { return prefabPieces[(int)MathF.Min(2, prefabPieces.Length - 1)]; } }
    [HideInInspector] public RendMaterials prefabPiece4 { get { return prefabPieces[(int)MathF.Min(3, prefabPieces.Length - 1)]; } }
    [HideInInspector] public RendMaterials prefabPiece5 { get { return prefabPieces[(int)MathF.Min(4, prefabPieces.Length - 1)]; } }
    [HideInInspector] public RendMaterials prefabPiece6 { get { return prefabPieces[(int)MathF.Min(5, prefabPieces.Length - 1)]; } }
    [HideInInspector] public RendMaterials prefabPiece7 { get { return prefabPieces[(int)MathF.Min(6, prefabPieces.Length - 1)]; } }
    [HideInInspector] public RendMaterials prefabPiece8 { get { return prefabPieces[(int)MathF.Min(7, prefabPieces.Length - 1)]; } }
    [HideInInspector] public RendMaterials prefabPiece9 { get { return prefabPieces[(int)MathF.Min(8, prefabPieces.Length - 1)]; } }
    [HideInInspector] public RendMaterials prefabPieceMB { get { return prefabPieces[(int)MathF.Min(9, prefabPieces.Length - 1)]; } }



}
