using Enemy;
using UnityEngine;
using Utils.Pooling;

namespace Turret.Weapon.Projectile.Bullet
{
    [CreateAssetMenu(menuName = "Assets/Bullet Projectile Asset", fileName = "Bullet Projectile Asset")]
    public class BulletProjectileAsset : ProjectileAssetBase
    {
        [SerializeField] private BulletProjectile m_BulletPrefab;

        [SerializeField] public float Speed;

        [SerializeField] public float Damage;
        
        public override IProjectile CreateProjectile(Vector3 origin, Vector3 originForward, EnemyData enemyData)
        {
            BulletProjectile projectile =  GameObjectPool.InstantiatePooled(m_BulletPrefab, origin, Quaternion.LookRotation(originForward, Vector3.up));
            projectile.SetBulletAsset(this);
            return projectile;
        }
    }
}