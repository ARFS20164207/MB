using UnityEngine;

namespace Most_Wanted.Scripts.Custom
{
    [CreateAssetMenu(fileName = "warrior", menuName = "MostWantedGame/Entities", order = 0)]
    public class MBWarrior : ScriptableObject
    {
        [SerializeField]public IWarrior type;
        public int id;
    }
}