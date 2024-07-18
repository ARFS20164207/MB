using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BP_FrontWarrior : BoardPiece
{
    public override bool IsValidMove(BoardCell targetCell)
    {
        if (!base.IsValidMove(targetCell)) return false;

        if (Mathf.Abs(currentCell.x - targetCell.x) > 1) return false;
        if (Mathf.Abs(currentCell.z - targetCell.z) > 1) return false;


        return true;
    }
    public override bool IsValidPass(BoardCell targetCell)
    {
        return base.IsValidPass(targetCell);
    }
    public override bool IsValidCapture(BoardCell targetCell)
    {
        return base.IsValidCapture(targetCell);
    }
}
