using Enemy;
using UnityEngine;

namespace Turret.Weapon.Projectile.Rocket
{
    [CreateAssetMenu(menuName = "Assets/Rocket Projectile Asset", fileName = "Rocket Projectile Asset")]
    public class RocketProjectileAsset : ProjectileAssetBase
    {
        [SerializeField] private RocketProjectile m_RocketPrefab;
        
        [SerializeField] public float Speed;

        [SerializeField] public float Damage;

        [SerializeField] public float ExplosionRadius;

        public override IProjectile CreateProjectile(Vector3 origin, Vector3 originForward, EnemyData enemyData)
        {
            RocketProjectile projectile = Instantiate(m_RocketPrefab, origin, Quaternion.LookRotation(enemyData.View.transform.position - origin, Vector3.up));
            projectile.SetRocketAsset(this);
            return projectile;
        }
    }
}