using System;
using System.Collections;
using Assets;
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

        public float Health => m_Health;

        public bool IsDead => m_Health <= 0;

        public event Action<float> HealthChanged; 

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
            if (m_Health < 0)
            {
                m_Health = 0;
            }
            HealthChanged?.Invoke(m_Health);
        }

        public void Die()
        {
            View.Die();
        }

        public void ReachedTarget()
        {
            m_Health = 0;
            View.ReachedTarget();
        }
    }
}