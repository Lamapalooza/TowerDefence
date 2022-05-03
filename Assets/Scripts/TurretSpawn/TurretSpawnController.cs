using Fields;
using RunTime;
using Turret;
using UnityEngine;
using Grid = Fields.Grid;

namespace TurretSpawn
{
    public class TurretSpawnController : IController
    {
        private Grid m_Grid;
        private TurretMarket m_Market;

        public TurretSpawnController(Grid grid, TurretMarket market)
        {
            m_Grid = grid;
            m_Market = market;
        }
        
        public void OnStart()
        {
        }

        public void OnStop()
        {
        }

        public void Tick()
        {
            if (m_Grid.HasSelectedNode() && Input.GetMouseButtonDown(0))
            {
                Node selectedNode = m_Grid.GetSelectedNode();

                if (selectedNode.IsOccupied || !m_Grid.CanOccupy(selectedNode))
                {
                    return;
                }

                TurretAsset asset = m_Market.ChosenTurret;

                if (asset != null)
                {
                    m_Market.BuyTurret(asset);
                    SpawnTurret(asset, selectedNode);
                }
                else
                {
                    Debug.Log("Not enough money!");
                }
            }
        }

        private void SpawnTurret(TurretAsset asset, Node node)
        {
            Node selectedNode = m_Grid.GetSelectedNode();
            TurretView view = Object.Instantiate(asset.ViewPrefab);
            TurretData data = new TurretData(asset, node);
            
            data.AttachView(view);
            Game.Player.TurretSpawned(data);

            m_Grid.TryOccupyNode(selectedNode, m_Grid.CanOccupy(selectedNode));
        }
    }
}