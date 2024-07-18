using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAction : MonoBehaviourPun
{
    // Start is called before the first frame update
    void Start()
    {
        BoardEvents.Instance.OnHover.AddListener(OnHoverCell);
        BoardEvents.Instance.OnHoverExit.AddListener(OnHoverCell);
        BoardEvents.Instance.OnSelected.AddListener(SelectCell);
    }



    public void OnHoverCell(BoardCell cell, bool setActive)
    {
        print("Onhover "+cell.name+" is " + setActive);
        if (BoardGame.instance.gameOver) return;
        if (cell == null) return;
        if (!setActive) { cell.cellRenders.GetRendMaterialByID(BoardRender.ItemSel.Common).SetColorToAll(cell.originalColor); return; }
        if (!cell.hasPiece) cell.cellRenders.GetRendMaterialByID(BoardRender.ItemSel.Common).SetColorToAll(cell.mouseOverColor);
        else
        {
            if (BoardGame.instance.selectedPiece == null && cell.currentPiece.isFirst == BoardGame.instance.isWhiteTurn)
                cell.cellRenders.GetRendMaterialByID(BoardRender.ItemSel.Common).SetColorToAll(cell.mouseOverColor);
            else if (BoardGame.instance.selectedPiece != null && cell.currentPiece.isFirst != BoardGame.instance.isWhiteTurn)
                cell.cellRenders.GetRendMaterialByID(BoardRender.ItemSel.Common).SetColorToAll(cell.mouseOverColor);
        }
    }
    public void SelectCell(BoardCell cell)
    {
        if (BoardGame.instance.gameOver) return;

        if (cell == null) return;
        if (PhotonNetwork.IsConnected && false)
        {
            if (MatchStats.instance.GetLocalPlayer().myTeam.myTurn == BoardGame.instance.isWhiteTurn) return;
            if (MatchStats.instance.GetOnlinePlayer().myTeam.myTurn == !BoardGame.instance.isWhiteTurn) return;
        }

        if (BoardGame.instance.selectedPiece == null && !cell.hasPiece) { return; }
        if (BoardGame.instance.selectedPiece == cell.currentPiece) { BoardGame.instance.DeselectPiece(); return;}

        if (!cell.hasPiece)//not piece selected previously
        {
            MoveStats newAction = BoardGame.instance.currentPlay.SetMovement(cell, MoveStats.MoveType.Move);
            
            int to = newAction.to.photonView.ViewID;
            int from = newAction.from.photonView.ViewID;
            BoardGame.instance.DoAction(newAction);
            photonView.RPC(nameof(DoActionOnline), RpcTarget.Others,to,from);
            PrintStack();
            BoardGame.instance.DeselectPiece();
            return;
        }

        if (cell.currentPiece.isFirst == BoardGame.instance.isWhiteTurn)//both have pieces, but are allies or enemies?
        { //equipo en turno
            BoardGame.instance.currentPlay = new MoveStats(cell);
            BoardGame.instance.DeselectPiece();
            BoardGame.instance.SelectPiece(cell);
        }
        else
        {
            if (BoardGame.instance.selectedPiece != null)
            {
                MoveStats newAction = BoardGame.instance.currentPlay.SetMovement(cell, MoveStats.MoveType.SucessAttack);
                int to = newAction.to.photonView.ViewID;
                int from = newAction.from.photonView.ViewID;
                BoardGame.instance.DoAction(newAction);
                photonView.RPC(nameof(DoActionAtkOnline), RpcTarget.Others, to, from);
                PrintStack();
            }
            BoardGame.instance.DeselectPiece();
        }
    }

    void PrintStack()
    {
        string txt = "History of Plays \n";
        foreach (var item in BoardGame.instance.gameHistory.ToArray())
        {
            txt += item.GetMoveInfo() + "\n";
        }
        txt += "~End";
        print(txt);
    }
    [PunRPC]
    public void DoActionOnline(int toID, int fromID)
    {
        BoardCell to = PhotonView.Find(toID).GetComponent<BoardCell>();
        BoardCell from = PhotonView.Find(fromID).GetComponent<BoardCell>();

        MoveStats move = new MoveStats(from, to);
        move.SetMovement(to, MoveStats.MoveType.Move);
        BoardGame.instance.DoAction(move);

    }
    [PunRPC]
    public void DoActionAtkOnline(int toID, int fromID)
    {
        BoardCell to = PhotonView.Find(toID).GetComponent<BoardCell>();
        BoardCell from = PhotonView.Find(fromID).GetComponent<BoardCell>();

        MoveStats atk = new MoveStats(from);
        atk.SetMovement(to, MoveStats.MoveType.SucessAttack);
        BoardGame.instance.DoAction(atk);

    }
}
