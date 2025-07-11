using ARFS.Tools;
using Photon.Pun;
using System.Collections;
using Most_Wanted.Scripts.Base;
using Most_Wanted.Scripts.V2;
using UnityEngine;

public class BoardPiece : MonoBehaviourPun
{

    [SerializeField] BoardPieceDots visualNumbers;
    public int strengh = 0;
    [SerializeField] BoardRender pieceRenders = null;

    [SerializeField] float velocity = 1f;
    private bool _ismoving = false;
    public Cell currentCell;
    public IPlayer controller;

    [ContextMenu("Agregar Elementos")]
    public void SetRendersMaterials()
    {
        pieceRenders = GetComponent<BoardRender>();
    }
    [ContextMenu("Actualizar Fuerza")]
    public void SetObjectNumeration()
    {
        if (visualNumbers) visualNumbers.SetNumber(strengh);
    }
    public void SetObjectNumeration(int newStrengh)
    {
        strengh = newStrengh;
        if (visualNumbers) visualNumbers.SetNumber(newStrengh);
    }
   
    public void UncheckPiece()
    {
        pieceRenders.SetActiveMaterial(BoardRender.ItemSel.DangerColor, false);
    }
 
    public bool[,] AllMoves<T>(T[,] posibilities)
    {
        bool[,] moves = new bool[posibilities.GetLength(0), posibilities.GetLength(1)];
        return moves;
    }

    public bool MovePiece(Cell destination)
    {
        if(_ismoving)return false;
        StartCoroutine(MovePieceCo(destination));
        return true;
    }
    IEnumerator MovePieceCo(Cell destination)
    {
        _ismoving = true;
        Vector3 start = transform.position;
        float timer = 0f;
        while (Vector3.Distance(transform.position, destination.position) >= 0.05f)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(start, destination.position, timer/velocity);
            yield return null;
        }
        _ismoving = false;
    }
    
    public bool MovePieceUndo(Cell destination, BoardPiece oldpiece = null)
    {
        //if (!IsValidMove(destination)) return false;
        if (_ismoving) { return false; }

        StartCoroutine(MovePieceCoUndo(destination, oldpiece));
        return true;
    }
    IEnumerator MovePieceCoUndo(Cell destination, BoardPiece oldpiece)
    {
        _ismoving = true;
        while (Vector3.Distance(transform.position, destination.position) >= 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, destination.position, velocity * Time.deltaTime);
            yield return null;
        }

        yield return null;

        if (oldpiece)
            oldpiece.ReturnPiece(currentCell);
        else
            currentCell.currentPiece = null;
        _ismoving = false;
    }

    public void CapturePiece()
    {
        if (currentCell != null)
        {
            currentCell.currentPiece = null;
            currentCell = null;
        }
        gameObject.SetActive(false);
    }
    public void ReturnPiece(Cell lastCell)
    {
        gameObject.SetActive(true);

        currentCell = lastCell;
        currentCell.currentPiece = this;
    }
    public bool IsValidMove(Cell targetCell,Cell CurrentCell)
    {
        int minx = (int)9;
        int miny = (int)7;
        BoardPiece currentPiece = CurrentCell.currentPiece;
        BoardPiece targetPiece = targetCell.currentPiece;
        
        // Verificar si el movimiento es dentro del tablero
        if (targetCell.x <= 0 || targetCell.x > minx || targetCell.y <= 0 || targetCell.y > miny)
            return false;
        
        // Verificar si el movimiento es hacia la misma celda
        if (currentCell == targetCell)
            return false;
        
        // Verificar si la pieza en la celda destino es del mismo color
        if (targetPiece != null && targetPiece.controller == controller)
            return false;
       

        int xvalue = Mathf.Abs(Mathf.Abs(currentCell.x) - Mathf.Abs(targetCell.x));
        int yvalue = Mathf.Abs(Mathf.Abs(currentCell.y) - Mathf.Abs(targetCell.y));
        switch (currentPiece.strengh)
        {
            case 1:
            case 2: 
            case 3:
            case 4:
                if (Mathf.Abs(currentCell.x - targetCell.x) > 1) return false;
                if (Mathf.Abs(currentCell.y - targetCell.y) > 1) return false;
                return true;
            case 5: 
            case 6: 
            case 7: 
            case 8:
                if (xvalue != yvalue) return false;
                break;
            case 9:
                if (targetCell.currentPiece != null && targetCell.currentPiece.strengh == 10) break;
                if (xvalue != yvalue) return false;
                break;
            case 10: 
                bool strongCell = (targetCell.x+targetCell.y)%2 == 0;
                if (strongCell)
                {
                    if (xvalue != yvalue)
                    {
                        if (xvalue > 1) return false;
                        if (yvalue > 1) return false;
                    }
                }
                else
                {
                    if (Mathf.Abs(currentCell.x - targetCell.x) > 1) return false;
                    if (Mathf.Abs(currentCell.y - targetCell.y) > 1) return false;
                }
                break;
            default: return true;
        }
        return true;
    }
    public bool IsValidPass(Cell targetCell,Cell CurrentCell)
    {
        if(targetCell==null) return false;
        if (!IsValidMove(targetCell,CurrentCell)) return false; 
        Vector2 sub = new Vector2(0, 0);
        sub.x = Mathf.Abs(currentCell.x - targetCell.x);
        if(sub.x <= 1) return true;
        
        Vector2 addValue = new Vector2(0, 0);
        addValue.x = (currentCell.x > targetCell.x) ? 1 : -1;
        addValue.y = (currentCell.y > targetCell.y) ? 1 : -1;
        switch (currentCell.currentPiece.strengh)
        {
            case 5: 
            case 6: 
            case 7: 
            case 8:
            case 9:
            case 10:
                Cell temp = BoardGame.instance.GetCell((targetCell.x + (int)addValue.x), (targetCell.y + (int)addValue.y));
                print($"moving f:{temp.x}.{temp.y}t:{targetCell.x}.{targetCell.y}");
                return IsValidPass(temp, currentCell);
            default: return true;
        }


        return true;
    }
    public bool IsValidCapture(Cell targetCell,Cell CurrentCell)
    {
        BoardPiece targetPiece = targetCell.currentPiece;
        if (targetPiece == null) return false;
        if (targetPiece.controller == controller) return false;

        int xvalue = Mathf.Abs(Mathf.Abs(currentCell.x) - Mathf.Abs(targetCell.x));
        int yvalue = Mathf.Abs(Mathf.Abs(currentCell.y) - Mathf.Abs(targetCell.y));
        if (xvalue > 1 || yvalue > 1) return false;

        if (targetPiece.strengh > strengh && targetPiece.strengh != 10) return false;
        if (strengh < 7 && targetPiece.strengh == 10) return false;

        switch (strengh)
        {
            case 5: 
            case 6: 
            case 7: 
            case 8:
                if (xvalue != yvalue) return false;
                break;
            case 9:
                if (targetCell.currentPiece.strengh != 10 && xvalue != yvalue) return false;
                break;
            default:
                break;
        }
        return true;
    }

    
}
