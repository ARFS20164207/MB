using UnityEngine;

namespace Most_Wanted.Scripts.V2
{
    public abstract class BoardFeedback : MonoBehaviour
    {
        public Transform selectContainer;
        public Transform captureContainer;
        public Transform movesContainer;

        public abstract bool ClearSignals(Transform container);
        public abstract bool Signals(bool[,] results);
        public abstract void Signals(bool[,] results, BoardPiece player);
    }
}