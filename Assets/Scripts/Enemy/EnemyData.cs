using System.Collections;
using Assets;
using RunTime;
using UnityEngine;

namespace Enemy
{
    public class EnemyData
    {
        private EnemyView m_View;
        private EnemyAsset m_Asset;
        private float m_Health;

        public EnemyView View => m_View;

        public EnemyAsset Asset => m_Asset;

        public bool IsDead => m_Health <= 0;

        public EnemyData(EnemyAsset asset)
        {
            m_Asset = asset;
            m_Health = asset.StartHealth;
        }
        
        public void AttachView(EnemyView view)
        {
            m_View = view;
            m_View.AttachData(this);
        }

        public void GetDamage(float damage)
        {
            if (IsDead)
            {
                return;
            }
            m_Health -= damage;
        }

        public void Die()
        {
            View.Die();
            m_View.MovementAgent.Die();
        }
    }
}