using Photon.Pun;
using System.Collections;
using UnityEngine;

public class Board : MonoBehaviour
{
     
    public int gridSize = 1;
    public Vector2 boardSize = new Vector2((int)8, (int)8);
    [SerializeField] GameTheme gameTheme;

    public BoardPiece selectedPiece;
    public BoardCell selectedCell;
    public BoardCell[,] board = new BoardCell[8, 8];

    [SerializeField] Transform containerCell;


    void Start()
    {
        board = new BoardCell[(int)boardSize.x, (int)boardSize.y];
        StartCoroutine(Initialize());
        InitializeBoardCell();

    }
    #region GameBuilder
    IEnumerator Initialize()
    {
        yield return new WaitForEndOfFrame();

    }



    [PunRPC]
    public void InitializeBoardCell()
    {
        for (int i = 0; i < boardSize.x; i++)
        {
            for (int j = 0; j < boardSize.y; j++)
            {
                board[i, j] = PhotonNetwork.Instantiate(gameTheme.prefabCell.name, new Vector3(gridSize * i, 0, j), Quaternion.Euler(90, 0, 0)).GetComponent<BoardCell>();
                board[i, j].transform.parent = containerCell;
                board[i, j].cellColor = ((i + j) % 2 == 1);
                board[i, j].SetCoordinates(i, j);
                board[i, j].gameObject.name = "Cell <" + i + "/" + j + ">";
                board[i, j].SetMaterial(gameTheme.ColorATable, gameTheme.ColorBTable);
                //board[i, j].SetIndicatorMaterial(gameTheme.ColorCellCheckmark);
            }
        }
    }
}
#endregion