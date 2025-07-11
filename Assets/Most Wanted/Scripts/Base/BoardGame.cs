using System;
using System.Collections;
using System.Collections.Generic;
using Most_Wanted.Scripts.V2;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Most_Wanted.Scripts.Base
{
    [Serializable]
    public class Cell
    {
        public int x;
        [FormerlySerializedAs("z")] public int y;
        public int piece = -1;
        public int team;
        public BoardPiece currentPiece;
        public Vector3 position;
    }

    public class BoardGame : MonoBehaviourPun, IController
    {
        public List<BoardPlayer> teams = new List<BoardPlayer>();
        public BoardPlayer localPlayer;
        public Stack<MoveStats> GameHistory = new Stack<MoveStats>();
        public MoveStats currentPlay = new MoveStats(null);
        public bool gameOver;

        public Cell[,] board = new Cell[9, 7];
        public IPlayer player1;
        public IPlayer player2;

        public int boardValue;
        public BoardMAp defaultMap;
        public GameObject worldContext;
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
            GetBoard();
            BoardEvents.Instance.OnPlayerTurn.AddListener(OnPlayerTurn);
            int first = UnityEngine.Random.Range(0, 2);
            OnPlayerTurn((first == 0)? player1 : player2,true);
            if ((PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected) ||
                !PhotonNetwork.IsConnected)
            {
                StartCoroutine(Initialize());
            }

            if (MatchStats.instance != null)
                localPlayer = MatchStats.instance.GetLocalPlayer();
        }

        public void OnPlayerTurn(IPlayer player, bool state)
        {
            player.canPlay = state;
            if (player != player1) player1.canPlay = !state;
            if (player != player2) player2.canPlay = !state;
            //if(!state)ChangeTurn();
        }

        public void CalculateBoardValue()
        {
            boardValue = 0;
            foreach (var item in player1.pieces)
            {
                if (item.gameObject.activeInHierarchy) boardValue += item.strengh ^ 2;
                else
                {
                    if (item.strengh == 10) gameOver = true;
                }
            }

            foreach (var item in player2.pieces)
            {
                if (item.gameObject.activeInHierarchy) boardValue -= item.strengh ^ 2;
                else
                {
                    if (item.strengh == 10) gameOver = true;
                }
            }

            if (gameOver)
            {
                //BoardEvents.Instance.InvokeVoid(BoardCustomEvents.OnGameOver);
                LocalPlayerStats.instance.coin2 += Math.Abs(boardValue);
                SceneManager.LoadScene("0-Login");
            }
        }

        [ContextMenu("UndoAction")]
        public void UndoAction()
        {
            if (GameHistory.Count == 0) return;
            MoveStats last = GameHistory.Pop();
            //last.Atacker.MovePieceUndo(last.from, last.Defender);
        }

        public void DoAction(MoveStats newAction)
        {
            if (GameHistory == null) return;
            GameHistory.Push(newAction);
        }

        #region GameBuilder

        IEnumerator Initialize()
        {
            yield return new WaitForEndOfFrame();
            //1InitializeBoard();
        }

        public void GetBoard()
        {
            board = new Cell[9, 7];
            for (int i = 0; i < 9; i++)
            for (int j = 0; j < 7; j++)
            {
                board[i, j] = new Cell();
            }

            ITableWorld world;
            worldContext.TryGetComponent(out world);
            player1.SetActivePieces(false);
            player2.SetActivePieces(false);
            for (int i = 0; i < 9; i++)
            {
                //board[i, 0].currentPiece = player1.pieces[i];
                //board[i, 0].currentPiece.SetObjectNumeration(defaultMap.row1Bteam[i]);;
                //board[i, 0].currentPiece.transform.position = world.CellToWorld(1, 0);
                LinkCellPiece(i, 0, player1, defaultMap.row1Ateam, world);
                LinkCellPiece(i, 1, player1, defaultMap.row2Ateam, world);
                LinkCellPiece(i, 2, player1, defaultMap.row3Ateam, world);
                LinkCellPiece(i, 4, player2, defaultMap.row5Bteam, world);
                LinkCellPiece(i, 5, player2, defaultMap.row6Bteam, world);
                LinkCellPiece(i, 6, player2, defaultMap.row7Bteam, world);
            }
        }

        void LinkCellPiece(int index, int jndex, IPlayer player, int[] row, ITableWorld world)
        {
            if (row[index] <= 0) return;
            board[index, jndex].currentPiece = player.GetPiece();
            board[index, jndex].currentPiece.gameObject.SetActive(true);
            board[index, jndex].currentPiece.gameObject.name =
                $"{player.name}:<{index + 1}-{jndex + 1}> Str:{row[index]}";
            board[index, jndex].currentPiece.transform.rotation = player.transform.rotation;
            board[index, jndex].currentPiece.SetObjectNumeration(row[index]);
            board[index, jndex].currentPiece.transform.position = world.CellToWorld(index + 1, jndex + 1);
            board[index, jndex].currentPiece.currentCell = board[index, jndex];
            board[index, jndex].currentPiece.controller= player;
        }

        public Cell GetCell(int x, int y)
        {
            return board[x - 1, y - 1];
        }

        public Cell GetCell(Vector2 position)
        {
            return board[-1 + (int)position.x, -1 + (int)position.y];
        }

/*
    BoardPiece SetPiece(GameObject prefab, int x, int z, bool isWhite = true, int strenghValue = 0)
    {
        if (x < 0 || z < 0) return null;
        if (x >= 9|| z >= 7) return null;
        BoardPiece pieza = null;
        GameObject go = null;
        Transform container = isWhite ? containerPieceA : containerPieceB;

        if (PhotonNetwork.IsConnected)
        {
            go = PhotonNetwork.Instantiate(prefab.name,
                new Vector3(gridSize.x * x, container.position.y, gridSize.y * z), container.rotation);
            pieza = go.GetComponent<BoardPiece>();
        }
        else
            pieza = Instantiate(prefab, new Vector3(gridSize.x * x, container.position.y, gridSize.y * z),
                container.rotation, container).GetComponent<BoardPiece>();


        if (PhotonNetwork.IsConnected && localPlayer != null)
        {
            pieza = SetLocalPieceRPC(pieza, x, z, isWhite, strenghValue);
            localPlayer.photonView.RPC("SetPieceRPC", RpcTarget.Others, pieza.photonView.ViewID, x, z, isWhite,
                strenghValue);
        }
        else
            pieza = SetLocalPieceRPC(pieza, x, z, isWhite, strenghValue);

        return pieza;
    }*/

        /*[PunRPC]
    BoardPiece SetPieceRPC(int viewID, int x, int z, bool isWhite = true, int strenghValue = 0)
    {
        BoardPiece pieza = PhotonView.Find(viewID).gameObject.GetComponent<BoardPiece>();

        return SetLocalPieceRPC(pieza, x, z, isWhite, strenghValue);
    }*/

        /* BoardPiece SetLocalPieceRPC(BoardPiece pieza, int x, int z, bool isWhite = true, int strenghValue = 0)
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
            //Debug.Log($"Set Piece {pieza.gameObject.name}: finished -> the cell in{board[x, z].name}");
        }
        catch (System.Exception)
        {
            Debug.LogError($"Set Piece {pieza.gameObject.name}: Failed -> the cell in is null");
        }

        return pieza;
    }*/

        #endregion

        void SetTableMoves(Cell cellOrigin)
        {
            if (!cellOrigin.currentPiece) return;
            bool[,] moves = new bool[9,7];
            for (int i = 0; i < moves.GetLength(0); i++)
            for (int j = 0; j < moves.GetLength(1); j++)
            {
                Cell cell = instance.board[i, j];
               
                //else if (!cell.currentPiece && moves[i, j])
                // cell.SetIndicatorMaterial(moves[i, j]);
            }
        }

        void ClearTable()
        {
            for (int i = 0; i < instance.board.GetLength(0); i++)
            for (int j = 0; j < instance.board.GetLength(1); j++)
            {
                Cell cell = instance.board[i, j];
                if (cell.currentPiece)
                    cell.currentPiece.UncheckPiece();
            }

        }


        public void ChangeTurn()
        {
            player1.canPlay = !player1.canPlay;
            player2.canPlay = !player2.canPlay;
            BoardEvents.Instance.InvokeVoid(BoardCustomEvents.OnChangeTurn);
        }


        public bool isTurn(IPlayer player)
        {
            if (player1 == player) return player1.canPlay;
            if (player2 == player) return player2.canPlay;
            return false;
        }

        public bool TableInteract(Vector2 cellCoordinates)
        {
            Cell move = GetCell(cellCoordinates);
            if (isTurn(player1))
            {
                print("P1-turn");
                player1.OnInteraction(move, player1.selectedCell);
            }
            else if (isTurn(player2))
            {
                print("P2-turn");
                player2.OnInteraction(move, player2.selectedCell);
            }

            return true;
        }

        public bool CellReference(Vector2 cellCoordinates, Vector3 worldPosition)
        {
            Cell temp = GetCell(cellCoordinates);
            if (temp == null) return false;
            temp.position = worldPosition;
            temp.x = (int)cellCoordinates.x;
            temp.y = (int)cellCoordinates.y;
            return true;
        }
        public bool[,] PossibleMoves(Cell[,] table,IPlayer player)
        {
            bool[,] moves = new bool[9,7];
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 7; j++)
                {
                    if (table[i, j].currentPiece)
                        moves[i, j] = player.selectedPiece.IsValidCapture(table[i, j],player.selectedCell);
                    else
                        moves[i, j] = player.selectedPiece.IsValidMove(table[i, j],player.selectedCell);
                }
            
            return moves;
        }
    }
}