using System.Collections.Generic;
using Enemy;
using Fields;
using RunTime;
using UnityEngine;

namespace Turret.Weapon.Projectile.Rocket
{
    public class RocketProjectile : MonoBehaviour, IProjectile
    {
        private float m_Speed = 5f;
        private int m_Damage = 15;
        private float m_ExplosionRadius = 4f;
        private bool m_DidHit = false;
        private EnemyData m_HitEnemy = null;
        
        public void TickApproaching()
        {
            transform.Translate(transform.forward * (m_Speed * Time.deltaTime), Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            m_DidHit = true;
            if (other.CompareTag("Enemy"))
            {
                EnemyView enemyView = other.GetComponent<EnemyView>();
                if (enemyView != null)
                {
                    m_HitEnemy = enemyView.Data;
                }
            }
        }
        
        public bool DidHit()
        {
            return m_DidHit;
        }

        public void DestroyProjectile()
        {
            if (m_HitEnemy != null)
            {
                List<Node> nodes =
                    Game.Player.Grid.GetNodesInCircle(m_HitEnemy.View.transform.position, m_ExplosionRadius);
                foreach (Node node in nodes)
                {
                    foreach (EnemyData enemyData in node.EnemyDatas)
                    {
                        enemyData.GetDamage(m_Damage);
                    }
                }
            }
            Destroy(gameObject);
        }
    }
}