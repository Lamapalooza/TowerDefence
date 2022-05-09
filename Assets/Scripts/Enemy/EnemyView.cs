using UI.InGame.Overtips;
using UnityEngine;
using Grid = Fields.Grid;

namespace Enemy
{
    public class EnemyView : MonoBehaviour
    {
        [SerializeField] private EnemyOvertip m_Overtip;
        
        
        private EnemyData m_Data;
        private IMovementAgent m_MovementAgent;

        public EnemyData Data => m_Data;

        public IMovementAgent MovementAgent => m_MovementAgent;

        public void AttachData(EnemyData data)
        {
            m_Data = data;
            m_Overtip.SetData(m_Data);
        }

        public void CreateMovementAgent(Grid grid)
        {
            if (m_Data.Asset.IsFlyingEnemy)
            {
                m_MovementAgent = new FlyingMovementAgent(m_Data.Asset.Speed, transform, grid, m_Data);
            }
            else
            {
                m_MovementAgent = new GridMovementAgent(m_Data.Asset.Speed, transform, grid, m_Data);
            }
        }

        public void Die()
        {
            Destroy(gameObject, 3f);
        }

        public void ReachedTarget()
        {
            Destroy(gameObject, 3f);
        }
    }
}