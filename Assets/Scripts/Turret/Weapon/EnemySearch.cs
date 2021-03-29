using System.Collections.Generic;
using Enemy;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;

namespace Turret.Weapon
{
    public class EnemySearch
    {
        private IReadOnlyList<EnemyData> m_EnemyDatas;


        public EnemySearch(IReadOnlyList<EnemyData> enemyDatas)
        {
            m_EnemyDatas = enemyDatas;
        }

        [CanBeNull]
        public EnemyData GetClosestEnemy(Vector3 center, float maxDistanse)
        {
            float maxSqrDistance = maxDistanse * maxDistanse;

            float minSqrDistance = float.MaxValue;
            EnemyData closestEnemy = null;
            
            foreach (EnemyData enemyData in m_EnemyDatas)
            {
                float sqrDistance = (enemyData.View.transform.position - center).sqrMagnitude;
                if (sqrDistance > maxSqrDistance)
                {
                    continue;
                }

                if (sqrDistance < minSqrDistance)
                {
                    minSqrDistance = sqrDistance;
                    closestEnemy = enemyData;
                }
            }
            
            return closestEnemy;
        }
    }
}