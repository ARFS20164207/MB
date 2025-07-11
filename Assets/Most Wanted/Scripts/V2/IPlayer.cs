using System;
using System.Collections.Generic;
using Most_Wanted.Scripts.Base;
using UnityEngine;

namespace Most_Wanted.Scripts.V2
{
    [Serializable]
    public abstract class IPlayer: MonoBehaviour
    {
        public bool canPlay;
        public BoardPiece selectedPiece { get; set; }
        public Cell selectedCell { get; set; }
        public List<BoardPiece> pieces { get; set; }
        public abstract bool OnPlayTurn(Cell target, Cell selected);
        public abstract bool OnPlaySelect(Cell target);
        public abstract bool OnInteraction(Cell target, Cell selected);
        public abstract void OnClearSelect();
        public abstract void SetActivePieces(bool state);
        public abstract BoardPiece GetPiece();
    }
}