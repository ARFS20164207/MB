using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BP_MostWanted : BoardPiece
{
    public override bool IsValidMove(BoardCell targetCell)
    {
        if (!base.IsValidMove(targetCell)) return false;
        if (_currentCell.cellColor)//white
        {
            if (Mathf.Abs(currentCell.x - targetCell.x) > 1) return false;
            if (Mathf.Abs(currentCell.z - targetCell.z) > 1) return false;
        }
        else
        {
            BoardPiece targetPiece = targetCell.currentPiece;
            if (targetPiece == null || targetPiece.strengh != 10)
            {
                int xvalue = Mathf.Abs(Mathf.Abs(currentCell.x) - Mathf.Abs(targetCell.x));
                int yvalue = Mathf.Abs(Mathf.Abs(currentCell.z) - Mathf.Abs(targetCell.z));

                if (xvalue != yvalue)
                {
                    if (xvalue > 1) return false;
                    if (yvalue > 1) return false;
                }
                if (xvalue > 1)
                    if (!IsValidPass(targetCell)) return false;
            }
        }
        return true;
    }

    public override bool IsValidPass(BoardCell targetCell)
    {

        Vector2 addValue = new Vector2(0, 0);
        addValue.x = (currentCell.x > targetCell.x) ? 1 : -1;
        addValue.y = (currentCell.z > targetCell.z) ? 1 : -1;
        Debug.Log(addValue);
        BoardCell temp = BoardGame.instance.board[targetCell.x + (int)addValue.x, targetCell.z + (int)addValue.y];

        if (!base.IsValidPass(temp)) return false;

        int xvalue = Mathf.Abs(Mathf.Abs(currentCell.x) - Mathf.Abs(temp.x));

        if (xvalue > 1)
            if (!IsValidPass(temp)) return false;

        return true;
    }
    public override bool IsValidCapture(BoardCell targetCell)
    {
        if (!base.IsValidCapture(targetCell)) return false;

        int xvalue = Mathf.Abs(Mathf.Abs(currentCell.x) - Mathf.Abs(targetCell.x));
        int yvalue = Mathf.Abs(Mathf.Abs(currentCell.z) - Mathf.Abs(targetCell.z));

        BoardPiece targetPiece = targetCell.currentPiece;
        if (strengh != 10) if (xvalue != yvalue) return false;

        return true;
    }
}
