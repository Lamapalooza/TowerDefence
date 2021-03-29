using System.Collections;
using System.Collections.Generic;
using Fields;
using RunTime;
using UnityEngine;
using Grid = Fields.Grid;

namespace Enemy
{
    public class FlyingMovementAgent : IMovementAgent
    {
        private float m_Speed;
        private Transform m_Transform;
        private EnemyData m_Data;
        private Grid m_Grid;

        public FlyingMovementAgent(float speed, Transform transform, Grid grid, EnemyData data)
        {
            m_Speed = speed;
            m_Transform = transform;
            m_Data = data;
            m_Grid = grid;
            
            SetTargetNode(grid.GetTargetNode());
            grid.GetStartNode().EnemyDatas.Add(data);
        }

        private const float TOLERANCE = 0.01f;

        private Node m_TargetNode;

        private Vector3 previousPosition = Game.Player.Grid.GetStartNode().Position;
        public void TickMovement()
        {
            if (m_TargetNode == null)
            {
                return;
            }

            Vector3 target = m_TargetNode.Position;
            Vector3 position = m_Transform.position;

            if (!m_Grid.GetNodeAtPoint(position).EnemyDatas.Contains(m_Data))
            {
                m_Grid.GetNodeAtPoint(previousPosition).EnemyDatas.Remove(m_Data);
                m_Grid.GetNodeAtPoint(position).EnemyDatas.Add(m_Data);
                previousPosition = position;
            }
            
            float sqrDistance = new Vector3(target.x-position.x,0f,target.z-position.z).sqrMagnitude;
            if (sqrDistance < TOLERANCE)
            {
                m_TargetNode.EnemyDatas.Remove(m_Data);
                m_TargetNode = m_TargetNode.NextNode;
                return;
            }

            Vector3 dir = new Vector3(target.x-position.x,0f,target.z-position.z).normalized;
            Vector3 delta = dir * (m_Speed * Time.deltaTime);
            m_Transform.Translate(delta);
        }

        private void SetTargetNode(Node node)
        {
            m_TargetNode = node;
        }
    }
}