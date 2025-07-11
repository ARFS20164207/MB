using System.Collections;
using System.Collections.Generic;
using Most_Wanted.Scripts.Base;
using UnityEngine;

public class BoardTrigger : MonoBehaviour
{
    public Cell cell;

    private void OnMouseEnter() => BoardEvents.Instance.InvokeCell(BoardCustomEvents.OnHover, cell);
    private void OnMouseExit() => BoardEvents.Instance.InvokeCell(BoardCustomEvents.OnHoverExit, cell);
    private void OnMouseDown() => BoardEvents.Instance.InvokeCell(BoardCustomEvents.OnSelected, cell);

    private void Awake()
    {
        try
        {
            //cell.name += "";
        }
        catch (System.Exception)
        {
            cell = GetComponent<Cell>();
            throw;
        }
    }
}
