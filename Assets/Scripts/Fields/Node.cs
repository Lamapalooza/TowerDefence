using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace Fields
{
    public class Node
    {
        public Vector3 Position;
        
        public Node NextNode;
        public bool IsOccupied;

        public float PathWeight;

        public OccupationAvailability m_OccupationAvailability;

        public List<EnemyData> EnemyDatas = new List<EnemyData>();

        public Node(Vector3 position)
        {
            Position = position;
        }
        
        public void ResetWeight()
        {
            PathWeight = float.MaxValue;
        }
    }
}