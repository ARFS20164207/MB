using ARFS.Tools;
using Photon.Pun;
using System.Collections;
using UnityEngine;

public class BoardPiece : MonoBehaviourPun
{

    public int CurrentX { get; set; }
    public int CurrentZ { get; set; }
    [SerializeField] public bool isFirst;
    [SerializeField] BoardPieceDots visualNumbers;
    public int strengh = 0;
    [SerializeReference] protected BoardCell _currentCell;
    [SerializeField] BoardRender pieceRenders = null;

    public bool IsChecked = false;
    public bool IsSelected = false;
    [SerializeField] float velocity = 1f;
    private bool ismoving = false;


    [ContextMenu("Agregar Elementos")]
    public void SetRendersMaterials()
    {
        pieceRenders = GetComponent<BoardRender>();
    }
    public void SetObjectNumeration()
    {
        if (visualNumbers) visualNumbers.SetNumber(strengh);
    }
    public BoardCell currentCell
    {
        get { return _currentCell; }
        set
        {
            _currentCell = value;
            if (value == null) return;
            if (value.currentPiece == this) return;
            _currentCell.currentPiece = this;
        }
    }
    //select like an option for the player
    public void SelectPiece()
    {
        IsSelected = true;
        SeleccionActive();

    }
    public void DiselectPiece()
    {
        IsSelected = false;
        SeleccionActive();
    }
    [ContextMenu("ActiveSelected")]
    private void SeleccionActive() => pieceRenders.SetActiveMaterial(BoardRender.ItemSel.SeleccionColor, IsSelected);
    private void SeleccionActive(bool enable) => pieceRenders.SetActiveMaterial(BoardRender.ItemSel.SeleccionColor, enable);
    //this piece is atacked or not
    public void CheckPiece()
    {
        DiselectPiece();
        pieceRenders.SetActiveMaterial(BoardRender.ItemSel.DangerColor, true);
    }
    public void UncheckPiece()
    {
        pieceRenders.SetActiveMaterial(BoardRender.ItemSel.DangerColor, false);
    }
    public void SetPosition(int x, int z)
    {
        CurrentX = x;
        CurrentZ = z;
    }

    public virtual bool[,] PossibleMoves(BoardCell[,] table)
    {
        bool[,] moves = AllMoves(table);
        for (int i = 0; i < moves.GetLength(0); i++)
        {
            for (int j = 0; j < moves.GetLength(1); j++)
            {
                BoardCell cell = table[i, j];
                if (cell.hasPiece)
                {
                    moves[i, j] = IsValidCapture(cell);
                    cell.currentPiece.IsChecked = moves[i, j];
                }
                else
                    moves[i, j] = IsValidMove(cell);
            }
        }
        return moves;
    }
    public bool[,] AllMoves<T>(T[,] posibilities)
    {
        bool[,] moves = new bool[posibilities.GetLength(0), posibilities.GetLength(1)];
        return moves;
    }

    public bool MovePiece(BoardCell destination)
    {
        if (!IsValidMove(destination)) return false;
        if (ismoving) { return false; }

        StartCoroutine(MovePieceCo(destination));
        return true;
    }
    IEnumerator MovePieceCo(BoardCell destination)
    {
        ismoving = true;
        while (Vector3.Distance(transform.position, destination.transform.position) >= 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, destination.transform.position, velocity * Time.deltaTime);
            yield return null;
        }
        currentCell.currentPiece = null;
        yield return null;
        if (destination.hasPiece)
            destination.currentPiece.CapturePiece();

        yield return null;
        currentCell = destination;
        currentCell.currentPiece = this;
        // Change turn
        BoardGame.instance.ChangeTurn();
        ismoving = false;
    }
    
    public bool MovePieceUndo(BoardCell destination, BoardPiece oldpiece = null)
    {
        //if (!IsValidMove(destination)) return false;
        if (ismoving) { return false; }

        StartCoroutine(MovePieceCoUndo(destination, oldpiece));
        return true;
    }
    IEnumerator MovePieceCoUndo(BoardCell destination, BoardPiece oldpiece)
    {
        ismoving = true;
        while (Vector3.Distance(transform.position, destination.transform.position) >= 0.05f)
        {
            transform.position = Vector3.Lerp(transform.position, destination.transform.position, velocity * Time.deltaTime);
            yield return null;
        }

        yield return null;

        if (oldpiece)
            oldpiece.ReturnPiece(currentCell);
        else
            currentCell.currentPiece = null;

        yield return null;
        currentCell = destination;
        currentCell.currentPiece = this;
        // Change turn
        BoardGame.instance.ChangeTurn();
        ismoving = false;
    }

    public virtual void CapturePiece()
    {
        if (currentCell != null)
        {
            currentCell.currentPiece = null;
            currentCell = null;
        }
        gameObject.SetActive(false);
    }
    public virtual void ReturnPiece(BoardCell lastCell)
    {
        gameObject.SetActive(true);

        currentCell = lastCell;
        currentCell.currentPiece = this;
    }
    public virtual bool IsValidMove(BoardCell targetCell)
    {
        int minx = (int)BoardGame.instance.boardSize.x;
        int miny = (int)BoardGame.instance.boardSize.y;
        // Verificar si el movimiento es dentro del tablero
        if (targetCell.x < 0 || targetCell.x >= minx || targetCell.z < 0 || targetCell.z >= miny)
        {
            return false;
        }

        // Verificar si el movimiento es hacia la misma celda
        if (currentCell.x == targetCell.x && currentCell.z == targetCell.z)
        {
            return false;
        }

        // Verificar si la pieza en la celda destino es del mismo color
        BoardPiece targetPiece = targetCell.currentPiece;
        if (targetPiece != null && targetPiece.isFirst == isFirst)
        {
            return false;
        }
        if (targetPiece != null && targetPiece.isFirst != isFirst)
        {
            if (!IsValidCapture(targetCell)) return false;
        }

        return true;
    }
    public virtual bool IsValidPass(BoardCell targetCell)
    {
        int minx = (int)BoardGame.instance.boardSize.x;
        int miny = (int)BoardGame.instance.boardSize.y;
        // Verificar si el movimiento es dentro del tablero
        if (targetCell.x < 0 || targetCell.x >= minx || targetCell.z < 0 || targetCell.z >= miny)
        {
            return false;
        }

        // Verificar si el movimiento es hacia la misma celda
        if (currentCell.x == targetCell.x && currentCell.z == targetCell.z)
        {
            return false;
        }

        // Verificar si la pieza en la celda destino es del mismo color
        BoardPiece targetPiece = BoardGame.instance.board[targetCell.x, targetCell.z].currentPiece;
        if (targetPiece != null) return false;


        return true;
    }
    public virtual bool IsValidCapture(BoardCell targetCell)
    {
        int minx = (int)BoardGame.instance.boardSize.x;
        int miny = (int)BoardGame.instance.boardSize.y;
        // Verificar si el movimiento es dentro del tablero
        if (targetCell.x < 0 || targetCell.x >= minx || targetCell.z < 0 || targetCell.z >= miny) return false;

        // Verificar si el movimiento es hacia la misma celda
        if (currentCell.x == targetCell.x && currentCell.z == targetCell.z) return false;

        // 
        BoardPiece targetPiece = BoardGame.instance.board[targetCell.x, targetCell.z].currentPiece;
        if (targetPiece == null) return false;
        if (targetPiece.isFirst == isFirst) return false;

        int xvalue = Mathf.Abs(Mathf.Abs(currentCell.x) - Mathf.Abs(targetCell.x));
        int yvalue = Mathf.Abs(Mathf.Abs(currentCell.z) - Mathf.Abs(targetCell.z));

        if (xvalue > 1 || yvalue > 1) return false;

        if (targetPiece.strengh > strengh && targetPiece.strengh != 10) return false;
        if (strengh < 7 && targetPiece.strengh == 10) return false;


        return true;
    }

    public void SetMaterial(Material a, Material B)
    {
        pieceRenders.SetMaterial(a, B);
        pieceRenders.ApplyMaterial(BoardRender.ItemSel.FistColor, isFirst);
    }
}
