using System.Collections;
using Assets;
using Enemy;
using RunTime;
using UnityEngine;
using Grid = Fields.Grid;

namespace EnemySpawn
{
    public class EnemySpawnController : IController
    {
        private SpawnWavesAsset m_SpawnWaves;
        private Grid m_Grid;

        private IEnumerator m_SpawnRoutine;

        private float m_WaitTime;

        public EnemySpawnController(SpawnWavesAsset spawnWaves, Grid grid)
        {
            m_SpawnWaves = spawnWaves;
            m_Grid = grid;
        }

        public void OnStart()
        {
            m_WaitTime = Time.time;
            m_SpawnRoutine = SpawnRoutine();
        }

        public void OnStop()
        {
            
        }

        public void Tick()
        {
            if (m_WaitTime > Time.time)
            {
                return;
            }
            
            if (m_SpawnRoutine.MoveNext())
            {
                if (m_SpawnRoutine.Current is CustomWaitForSeconds waitForSeconds)
                {
                    m_WaitTime = Time.time + waitForSeconds.Seconds;
                }
            }
        }

        private IEnumerator SpawnRoutine()
        {
            foreach (SpawnWave wave in m_SpawnWaves.SpawnWaves)
            {
                yield return new CustomWaitForSeconds(wave.TimeBeforeStartWave);

                for (int i = 0; i < wave.Count; i++)
                {
                    SpawnEnemy(wave.EnemyAsset);
                    if (i < wave.Count - 1)
                    {
                        yield return new CustomWaitForSeconds(wave.TimeBetweenSpawns);
                    }
                }
            }
            Game.Player.LastWaveSpawned();
        }

        private void SpawnEnemy(EnemyAsset asset)
        {
            EnemyView view = Object.Instantiate(asset.ViewPrefab);
            Vector3 viewPosition = view.transform.position;
            viewPosition.x = m_Grid.GetStartNode().Position.x;
            viewPosition.z = m_Grid.GetStartNode().Position.z;
            view.transform.position = viewPosition;
            EnemyData data = new EnemyData(asset);
            
            data.AttachView(view);
            view.CreateMovementAgent(m_Grid);
            
            Game.Player.EnemySpawned(data);
        }
        
        private class CustomWaitForSeconds
        {
            public readonly float Seconds;

            public CustomWaitForSeconds(float seconds)
            {
                Seconds = seconds;
            }
        }
    }
}