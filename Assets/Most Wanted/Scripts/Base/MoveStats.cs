using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class MoveStats
{
    //the atacker moves to the defender an do the attack, id defender is null it is a pass
    public BoardPiece Atacker;
    public BoardPiece Defender;
    public BoardCell from;
    public BoardCell to;
    public MoveType movement;



    public MoveStats(BoardCell from,BoardCell to)
    {
        this.from = from;
        this.to = to;
        if (from != null) if (from.hasPiece) this.Atacker = from.currentPiece;
        if (to != null) if (to.hasPiece) this.Defender = to.currentPiece;
    }
    public MoveStats(BoardCell from)
    {
        this.from = from;
        this.to = null;
        if (from != null) if (from.hasPiece) this.Atacker = from.currentPiece;
        this.Defender = null;
        this.movement = MoveType.Waiting;
    }

    public MoveStats SetMovement(BoardCell to,MoveType moveType)
    {
        if (from == null) return null;
        this.to = to;
        if (to != null) if (to.hasPiece) this.Defender = to.currentPiece;
        this.movement = moveType;
        return this;
    }
    public enum MoveType
    {
        Waiting,
        TryingAttack,
        SucessAttack,
        FailedAttack,
        Move,
        SpecialMove
    }

    public string GetMoveInfo()
    {
        string result = "";
        if(Atacker) result += Atacker.isFirst ? "X" : "O"; else result += "Null";
        if(from) result += "(" + from.x +","+ from.z + ") -> "; else result += " - ";
        if (Defender) result += Defender.isFirst ? "X" : "O"; else result += "";
        if(to) result += "(" + to.x + "," + to.z + ") = "; else result += " - ";
        result += movement;


        return result;
    }
}
