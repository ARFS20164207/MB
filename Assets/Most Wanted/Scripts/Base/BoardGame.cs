using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class BoardGame : MonoBehaviourPun
{
    public List<BoardPiece> piecesA = new List<BoardPiece>();
    public List<BoardPiece> piecesB = new List<BoardPiece>();
    public List<BoardPlayer> teams = new List<BoardPlayer>();
    public BoardPlayer localPlayer = null;
    public Stack<MoveStats> gameHistory = new Stack<MoveStats>();
    public MoveStats currentPlay = new MoveStats(null);
    public bool isWhiteTurn = true;
    public bool gameOver = false;
    public bool MovePreview = false;

    public BoardPiece selectedPiece;
    public BoardCell selectedCell;

    [SerializeField] Transform containerCell;
    [SerializeField] Transform containerPieceA;
    [SerializeField] Transform containerPieceB;

    public Vector2 gridSize = new(1, 1);
    public Vector2 boardSize = new Vector2((int)8, (int)8);
    public BoardCell[,] board = new BoardCell[9, 7];

    [SerializeField] GameTheme gameTheme;

    public static BoardGame instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {

        if ((PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) ||
            !PhotonNetwork.IsConnected)
        {
            StartCoroutine(Initialize());
        }
            InitializeBoardCell();
            if (MatchStats.instance != null)
                localPlayer = MatchStats.instance.GetLocalPlayer();
    }
    
   
    [ContextMenu("UndoAction")]
    public void UndoAction()
    {
        if (gameHistory.Count == 0) return;
        MoveStats last = gameHistory.Pop();
        last.Atacker.MovePieceUndo(last.from, last.Defender);
    }
    public void DoAction(MoveStats newAction)
    {
        if (gameHistory == null) return;
        gameHistory.Push(newAction);
        newAction.Atacker.MovePiece(newAction.to);
    }

    #region GameBuilder
    IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();
        //1InitializeBoard();

    }
    public void GetBoard()
    {
        if (containerCell == null)
        {
            board = new BoardCell[(int)boardSize.x, (int)boardSize.y];
            return;
        }

        if (containerCell.childCount != (int)boardSize.x * (int)boardSize.y)
        {
            board = new BoardCell[(int)boardSize.x, (int)boardSize.y];
            return;
        }

        for (int i = 0; i < boardSize.x; i++)
            for (int j = 0; j < boardSize.y; j++)
            {   board[i, j] = containerCell.GetChild((i * (int)boardSize.y) + j).GetComponent<BoardCell>();
                board[i, j].SetMaterial(gameTheme.ColorATable, gameTheme.ColorBTable);
            }
    }

    [ContextMenu("StartCells")]
    public void InitializeBoardCell()
    {
        try
        {
            if (board[(int)boardSize.x - 1, (int)boardSize.y - 1] == null)
            {
                GetBoard();
                Debug.LogError("El Mapa de celdas no estaba configurado");
            }else
            {
                print(board[(int)boardSize.x - 1, (int)boardSize.y - 1].name);
            }
        }
        catch (System.Exception)
        {
            GetBoard();
            Debug.LogError("El Mapa de celdas no estaba configurado");
        }
        /*for (int i = 0; i < boardSize.x; i++)
        {
            for (int j = 0; j < boardSize.y; j++)
            {
                if (board[i, j] == null)
                { board[i, j] = Instantiate(gameTheme.prefabCell, new Vector3(gridSize.x * i, containerCell.position.y, gridSize.y * j), Quaternion.Euler(90, 0, 0), containerCell).GetComponent<BoardCell>(); }
                else
                {
                    board[i, j].transform.position = new Vector3(gridSize.x * i, containerCell.position.y, gridSize.y * j);
                    board[i, j].transform.rotation = Quaternion.Euler(90, 0, 0);
                    board[i, j].transform.parent = containerCell;
                }
                board[i, j].cellColor = ((i + j) % 2) == 1;
                board[i, j].SetCoordinates(i, j);
                board[i, j].gameObject.name = "Cell <" + (i + 1) + "H/" + (j + 1) + "V>";
                board[i, j].SetMaterial(gameTheme.ColorATable, gameTheme.ColorBTable);
            }
        }*/
    }
    BoardPiece SetPiece(GameObject prefab, int x, int z, bool isWhite = true, int strenghValue = 0)
    {
        if (x < 0 || z < 0) return null;
        if (x >= boardSize.x || z >= boardSize.y) return null;
        BoardPiece pieza = null;
        GameObject go = null;
        Transform container = isWhite ? containerPieceA : containerPieceB;

        if (PhotonNetwork.IsConnected)
        {
            go = PhotonNetwork.Instantiate(prefab.name, new Vector3(gridSize.x * x, container.position.y, gridSize.y * z), container.rotation);
            pieza = go.GetComponent<BoardPiece>();
        }
        else
            pieza = Instantiate(prefab, new Vector3(gridSize.x * x, container.position.y, gridSize.y * z), container.rotation, container).GetComponent<BoardPiece>();


        if (PhotonNetwork.IsConnected && localPlayer != null)
        {
            pieza = SetLocalPieceRPC(pieza, x, z, isWhite, strenghValue);
            localPlayer.photonView.RPC("SetPieceRPC", RpcTarget.Others, pieza.photonView.ViewID, x, z, isWhite, strenghValue);
        }
        else
            pieza = SetLocalPieceRPC(pieza, x, z, isWhite, strenghValue);

        return pieza;
    }
    [PunRPC]
    BoardPiece SetPieceRPC(int viewID, int x, int z, bool isWhite = true, int strenghValue = 0)
    {
        BoardPiece pieza = PhotonView.Find(viewID).gameObject.GetComponent<BoardPiece>();

        return SetLocalPieceRPC(pieza,x,z,isWhite,strenghValue);
    }
    BoardPiece SetLocalPieceRPC(BoardPiece pieza, int x, int z, bool isWhite = true, int strenghValue = 0)
    {
        pieza.transform.parent = isWhite ? containerPieceA : containerPieceB;
        pieza.isFirst = isWhite;
        pieza.strengh = strenghValue;
        pieza.SetObjectNumeration();

        pieza.SetMaterial(gameTheme.ColorAPiece, gameTheme.ColorBPiece);
        pieza.currentCell = board[x, z];
        pieza.gameObject.name = "P" + pieza.strengh + "<" + x + "-" + z + "> :" + (pieza.isFirst ? "A" : "B");
        try
        {
            Debug.Log($"Set Piece {pieza.gameObject.name}: finished -> the cell in{board[x, z].name}");
        }
        catch (System.Exception)
        {
            Debug.LogError($"Set Piece {pieza.gameObject.name}: Failed -> the cell in is null");
        }
        return pieza;
    }
    public void InitializeBoard()
    {
        int PlayerABackLine = 0;
        int PlayerAFrontLine = 1;
        int PlayerBBackLine = 6;
        int PlayerBFrontLine = 5;

        // Crea las piezas y las posiciona en el tablero
        // Add white pieces
        piecesA.Add(SetPiece(gameTheme.prefabPiece1.gameObject, 0, PlayerAFrontLine, isWhite: true, 1));
        piecesA.Add(SetPiece(gameTheme.prefabPiece2.gameObject, 1, PlayerAFrontLine, isWhite: true, 2));
        piecesA.Add(SetPiece(gameTheme.prefabPiece3.gameObject, 2, PlayerAFrontLine, isWhite: true, 3));
        piecesA.Add(SetPiece(gameTheme.prefabPiece4.gameObject, 3, PlayerAFrontLine, isWhite: true, 4));
        piecesA.Add(SetPiece(gameTheme.prefabPiece4.gameObject, 4, PlayerAFrontLine, isWhite: true, 4));
        piecesA.Add(SetPiece(gameTheme.prefabPiece4.gameObject, 5, PlayerAFrontLine, isWhite: true, 4));
        piecesA.Add(SetPiece(gameTheme.prefabPiece3.gameObject, 6, PlayerAFrontLine, isWhite: true, 3));
        piecesA.Add(SetPiece(gameTheme.prefabPiece2.gameObject, 7, PlayerAFrontLine, isWhite: true, 2));
        piecesA.Add(SetPiece(gameTheme.prefabPiece1.gameObject, 8, PlayerAFrontLine, isWhite: true, 1));

        piecesA.Add(SetPiece(gameTheme.prefabPiece5.gameObject, 0, PlayerABackLine, isWhite: true, 5));
        piecesA.Add(SetPiece(gameTheme.prefabPiece6.gameObject, 1, PlayerABackLine, isWhite: true, 6));
        piecesA.Add(SetPiece(gameTheme.prefabPiece7.gameObject, 2, PlayerABackLine, isWhite: true, 7));
        piecesA.Add(SetPiece(gameTheme.prefabPiece9.gameObject, 3, PlayerABackLine, isWhite: true, 9));
        piecesA.Add(SetPiece(gameTheme.prefabPieceMB.gameObject, 4, PlayerABackLine, isWhite: true, 10));
        piecesA.Add(SetPiece(gameTheme.prefabPiece8.gameObject, 5, PlayerABackLine, isWhite: true, 8));
        piecesA.Add(SetPiece(gameTheme.prefabPiece7.gameObject, 6, PlayerABackLine, isWhite: true, 7));
        piecesA.Add(SetPiece(gameTheme.prefabPiece6.gameObject, 7, PlayerABackLine, isWhite: true, 6));
        piecesA.Add(SetPiece(gameTheme.prefabPiece5.gameObject, 8, PlayerABackLine, isWhite: true, 5));

        // Add Black pieces
        piecesB.Add(SetPiece(gameTheme.prefabPiece1.gameObject, 0, PlayerBFrontLine, isWhite: false, 1));
        piecesB.Add(SetPiece(gameTheme.prefabPiece2.gameObject, 1, PlayerBFrontLine, isWhite: false, 2));
        piecesB.Add(SetPiece(gameTheme.prefabPiece3.gameObject, 2, PlayerBFrontLine, isWhite: false, 3));
        piecesB.Add(SetPiece(gameTheme.prefabPiece4.gameObject, 3, PlayerBFrontLine, isWhite: false, 4));
        piecesB.Add(SetPiece(gameTheme.prefabPiece4.gameObject, 4, PlayerBFrontLine, isWhite: false, 4));
        piecesB.Add(SetPiece(gameTheme.prefabPiece4.gameObject, 5, PlayerBFrontLine, isWhite: false, 4));
        piecesB.Add(SetPiece(gameTheme.prefabPiece3.gameObject, 6, PlayerBFrontLine, isWhite: false, 3));
        piecesB.Add(SetPiece(gameTheme.prefabPiece2.gameObject, 7, PlayerBFrontLine, isWhite: false, 2));
        piecesB.Add(SetPiece(gameTheme.prefabPiece1.gameObject, 8, PlayerBFrontLine, isWhite: false, 1));

        piecesB.Add(SetPiece(gameTheme.prefabPiece5.gameObject, 0, PlayerBBackLine, isWhite: false, 5));
        piecesB.Add(SetPiece(gameTheme.prefabPiece6.gameObject, 1, PlayerBBackLine, isWhite: false, 6));
        piecesB.Add(SetPiece(gameTheme.prefabPiece7.gameObject, 2, PlayerBBackLine, isWhite: false, 7));
        piecesB.Add(SetPiece(gameTheme.prefabPiece9.gameObject, 3, PlayerBBackLine, isWhite: false, 9));
        piecesB.Add(SetPiece(gameTheme.prefabPieceMB.gameObject, 4, PlayerBBackLine, isWhite: false, 10));
        piecesB.Add(SetPiece(gameTheme.prefabPiece8.gameObject, 5, PlayerBBackLine, isWhite: false, 8));
        piecesB.Add(SetPiece(gameTheme.prefabPiece7.gameObject, 6, PlayerBBackLine, isWhite: false, 7));
        piecesB.Add(SetPiece(gameTheme.prefabPiece6.gameObject, 7, PlayerBBackLine, isWhite: false, 6));
        piecesB.Add(SetPiece(gameTheme.prefabPiece5.gameObject, 8, PlayerBBackLine, isWhite: false, 5));
    }
    #endregion

    //selector Method
    public void SelectPiece(BoardCell cell)
    {
        selectedPiece = cell.currentPiece;
        selectedCell = cell;
        if (selectedPiece) selectedPiece.SelectPiece();
        SetTableMoves(cell);
    }
    void SetTableMoves(BoardCell cellOrigin)
    {
        if (!cellOrigin.hasPiece) return;
        bool[,] moves = cellOrigin.currentPiece.PossibleMoves(instance.board);
        for (int i = 0; i < moves.GetLength(0); i++)
            for (int j = 0; j < moves.GetLength(1); j++)
            {
                BoardCell cell = instance.board[i, j];
                if (cell.hasPiece && moves[i, j])
                    cell.currentPiece.CheckPiece();
                else if (!cell.hasPiece && moves[i, j])
                    cell.SetIndicatorMaterial(moves[i, j]);
            }
    }
    void ClearTable()
    {
        for (int i = 0; i < instance.board.GetLength(0); i++)
            for (int j = 0; j < instance.board.GetLength(1); j++)
            {
                BoardCell cell = instance.board[i, j];
                cell.SetIndicatorMaterial(false);
                if (cell.hasPiece)
                    cell.currentPiece.UncheckPiece();
            }
        MovePreview = false;
    }

    public void DeselectPiece()
    {
        if (selectedPiece) selectedPiece.DiselectPiece();
        selectedPiece = null;
        selectedCell = null;
        ClearTable();
    }

    public void ChangeTurn()
    {
        isWhiteTurn = !isWhiteTurn;
    }
}
