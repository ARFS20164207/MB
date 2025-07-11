using UnityEngine;

namespace Most_Wanted.Scripts.V2
{
    public interface ITableWorld
    {
        public Vector3 CellToWorld(int x, int y);
    }
}