using System.Collections.Generic;
using System.Linq;
using Most_Wanted.Scripts.Base;
using Most_Wanted.Scripts.V2;
using Photon.Pun.Demo.Asteroids;
using UnityEngine;

namespace Most_Wanted.Scripts.Custom
{
    public class WarriorTeam : IPlayer
    {
        
        public BoardPiece selectedPiece { get; set; }
        public Cell selectedCell { get; set; }
        public new List<BoardPiece> pieces = new List<BoardPiece>();

        public override bool OnInteraction(Cell target, Cell selected)
        {
            print($"EventStatus({gameObject.name}):Select<{selected}> To -> <<{target.x},{target.y}>>");
            return (selected!=null) ? OnPlayTurn(target, selected) : OnPlaySelect(target);
        }


        public override bool OnPlayTurn(Cell target, Cell selected)
        {
            if (selected.currentPiece.IsValidMove(target,selected) && selected.currentPiece.IsValidPass(target,selected))
            {
                if (target.currentPiece)
                {
                    if (selected.currentPiece.IsValidCapture(target, selected))
                    {
                        selected.currentPiece.MovePiece(target);
                        target.currentPiece.CapturePiece();
                        target.currentPiece = selected.currentPiece;
                        selected.currentPiece.currentCell = target;
                        selected.currentPiece = null;
                        BoardGame.instance.GameHistory.Push(new MoveStats(selected,target));
                        BoardEvents.Instance.OnPlayerTurn.Invoke(target.currentPiece.controller, false);
                    }
                }
                else
                {
                    selected.currentPiece.MovePiece(target);
                    target.currentPiece = selected.currentPiece;
                    selected.currentPiece.currentCell = target;
                    selected.currentPiece = null;
                    BoardGame.instance.GameHistory.Push(new MoveStats(selected,target));
                    BoardEvents.Instance.OnPlayerTurn.Invoke(target.currentPiece.controller, false);
                }
                
                OnClearSelect();
                return true;
            }

            if (target.currentPiece != null &&
                selected.currentPiece.controller == target.currentPiece.controller)
            {
                OnClearSelect();
                OnPlaySelect(target);
            }
            return false;
        }
        public override bool OnPlaySelect(Cell target)
        {
            if(target ==null) return false;
            if(target.currentPiece == null) return false;
            if (this != target.currentPiece.controller) return false;
            selectedPiece = target.currentPiece;
            selectedCell = target;
            print(this.selectedPiece);
            bool[,] options = BoardGame.instance.PossibleMoves(BoardGame.instance.board, this);
            BoardEvents.Instance.OnPosibleMoves.Invoke(options, selectedPiece);
            return true;
        }

        public override  void OnClearSelect()
        {
            selectedPiece = null;
            selectedCell = null;
        }

        public override void SetActivePieces(bool state)
        {
            foreach (var piece in pieces)
            {
                piece.gameObject.SetActive(state);
            }
        }

        public override BoardPiece GetPiece()
        {
            return pieces.First((x) =>!x.gameObject.activeInHierarchy);
        }
    }
}