using System.Collections.Generic;
using Enemy;
using Fields;
using Turret.Weapon;
using TurretSpawn;
using UnityEngine;
using Grid = Fields.Grid;

namespace RunTime
{
    public class Player
    {
        private List<EnemyData> m_EnemyDatas = new List<EnemyData>();
        public IReadOnlyList<EnemyData> EnemyDatas => m_EnemyDatas;

        public readonly GridHolder GridHolder;
        public readonly Grid Grid;
        public readonly TurretMarket TurretMarket;
        public readonly EnemySearch EnemySearch;

        public Player()
        {
            GridHolder = Object.FindObjectOfType<GridHolder>();
            GridHolder.CreateGrid();
            Grid = GridHolder.Grid;

            TurretMarket = new TurretMarket(Game.CurrentLevel.TurretMarketAsset);

            EnemySearch = new EnemySearch(m_EnemyDatas);
        }

        public void EnemySpawned(EnemyData data)
        {
            m_EnemyDatas.Add(data);
        }
    }
}