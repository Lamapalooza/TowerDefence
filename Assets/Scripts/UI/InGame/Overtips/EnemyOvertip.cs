using System;
using Enemy;
using UnityEngine;

namespace UI.InGame.Overtips
{
    public class EnemyOvertip : MonoBehaviour
    {
        [SerializeField] private RectTransform m_RectTransform;

        private float m_StartHealth;

        public void SetData(EnemyData enemyData)
        {
            m_StartHealth = enemyData.Asset.StartHealth;

            enemyData.HealthChanged += SetHealth;
            SetHealth(enemyData.Health);
        }

        private void SetHealth(float health)
        {
            SetHealthBar(health / m_StartHealth);
        }

        private void SetHealthBar(float percentage)
        {
            percentage = Mathf.Clamp01(percentage);
            
            m_RectTransform.anchorMin = Vector2.zero;
            m_RectTransform.anchorMax = new Vector2(percentage, 1f);
        }
    }
}