using System;
using System.Collections;
using System.Collections.Generic;
using Most_Wanted.Scripts.Base;
using UnityEngine;
[Serializable]
public class MoveStats
{
    //the atacker moves to the defender an do the attack, id defender is null it is a pass
    public BoardPiece Atacker;
    public BoardPiece Defender;
    public Cell from;
    public Cell to;
    public MoveType movement;



    public MoveStats(Cell from,Cell to)
    {
        this.from = from;
        this.to = to;
        if (from != null) if (from.currentPiece) this.Atacker = from.currentPiece;
        if (to != null) if (to.currentPiece) this.Defender = to.currentPiece;
    }
    public MoveStats(Cell from)
    {
        this.from = from;
        this.to = null;
        if (from != null) if (from.currentPiece) this.Atacker = from.currentPiece;
        this.Defender = null;
        this.movement = MoveType.Waiting;
    }

    public MoveStats SetMovement(Cell to,MoveType moveType)
    {
        if (from == null) return null;
        this.to = to;
        if (to != null) if (to.currentPiece) this.Defender = to.currentPiece;
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
        if(Atacker) result += Atacker.controller.name; else result += "Null";
        if(from!=null) result += "(" + from.x +","+ from.y + ") -> "; else result += " - ";
        if (Defender) result += Defender.controller.name; else result += "";
        if(to!=null) result += "(" + to.x + "," + to.y + ") = "; else result += " - ";
        result += movement;


        return result;
    }
}
