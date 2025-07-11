using ARFS.Tools;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCell : MonoBehaviourPun
{/*
    [HideInInspector] public int x;
    [HideInInspector] public int z;
    public bool hasPiece
    {
        get { return (_currentPiece != null); }
    }
    [SerializeReference] private BoardPiece _currentPiece;
    public BoardPiece currentPiece
    {
        get { return _currentPiece; }
        set
        {
            _currentPiece = value;
            if (value == null) return;
            if (value.currentCell == this) return;
            _currentPiece.currentCell = this;
        }
    }
    public Color originalColor;
    public Color mouseOverColor = Color.yellow;
    public bool cellColor = false;

    [SerializeField]public BoardRender cellRenders = null;

    public void SetMaterial(Material a, Material B)
    {
        if (cellRenders == null) return;
        cellRenders.SetMaterial(a, B);
        originalColor = cellRenders.ApplyMaterial(BoardRender.ItemSel.Common, cellColor);
    }
    public void SetIndicatorMaterial(bool enable)
    {
        cellRenders.SetActiveMaterial(BoardRender.ItemSel.SeleccionColor, enable);
    }

    public void SetCoordinates(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

*/
}
