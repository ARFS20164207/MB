using UnityEngine;

namespace Most_Wanted.Scripts.V2
{
    
    public interface IController
    {
        public bool isTurn(IPlayer player);
        public bool TableInteract(Vector2 CellCoordinates);
        public bool CellReference(Vector2 CellCoordinates,Vector3 worldPosition);
    }
}