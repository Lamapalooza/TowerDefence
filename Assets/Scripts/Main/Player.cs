using System.Collections.Generic;
using Enemy;
using Fields;
using RunTime;
using Turret;
using TurretSpawn;
using UnityEngine;
using Grid = Fields.Grid;

namespace Main
{
    public class Player
    {
        private List<EnemyData> m_EnemyDatas = new List<EnemyData>();
        public IReadOnlyList<EnemyData> EnemyDatas => m_EnemyDatas;

        private List<TurretData> m_TurretDatas = new List<TurretData>();

        public IReadOnlyList<TurretData> TurretDatas => m_TurretDatas;
        
        public readonly GridHolder GridHolder;
        public readonly Grid Grid;
        public readonly TurretMarket TurretMarket;

        private bool m_AllWavesAreSpawned = false;

        public Player()
        {
            GridHolder = Object.FindObjectOfType<GridHolder>();
            GridHolder.CreateGrid();
            Grid = GridHolder.Grid;

            TurretMarket = new TurretMarket(Game.CurrentLevel.TurretMarketAsset);
        }

        public void EnemySpawned(EnemyData data)
        {
            m_EnemyDatas.Add(data);
        }

        public void TurretSpawned(TurretData data)
        {
            m_TurretDatas.Add(data);
        }

        public void LastWaveSpawned()
        {
            m_AllWavesAreSpawned = true;
        }

        public void EnemyDied(EnemyData data)
        {
            Game.StopPlayer();
            m_EnemyDatas.Remove(data);
        }

        private void GameWon()
        {
            Debug.Log("Win!");
        }

        public void CheckForWin()
        {
            if (m_AllWavesAreSpawned && m_EnemyDatas.Count == 0)
            {
                GameWon();
            } 
        }
    }
}