using UnityEngine;
using UnityEngine.Serialization;

namespace Most_Wanted.Scripts.V2
{
    [CreateAssetMenu(fileName = "CustomMapTable", menuName = "MostWantedGame/CustomMap", order = 1)]
    public class BoardMAp : ScriptableObject
    {
        [FormerlySerializedAs("row1Bteam")] public int[] row1Ateam = new int[9];
        [FormerlySerializedAs("row2Bteam")] public int[] row2Ateam = new int[9];
        [FormerlySerializedAs("row3Bteam")] public int[] row3Ateam = new int[9];
        public int[] row4Neutral = new int[9];
        [FormerlySerializedAs("row5Ateam")] public int[] row5Bteam = new int[9];
        [FormerlySerializedAs("row6Ateam")] public int[] row6Bteam = new int[9];
        [FormerlySerializedAs("row7Ateam")] public int[] row7Bteam = new int[9];
    }
}