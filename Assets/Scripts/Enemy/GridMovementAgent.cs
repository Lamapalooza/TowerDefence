using System.Collections;
using System.Collections.Generic;
using Fields;
using UnityEngine;
using Grid = Fields.Grid;

namespace Enemy
{
    public class GridMovementAgent : IMovementAgent
    {
        private float m_Speed;
        private Transform m_Transform;
        private EnemyData m_Data;
        private Grid m_Grid;
        private Node m_Node;

        public GridMovementAgent(float speed, Transform transform, Grid grid, EnemyData data)
        {
            m_Speed = speed;
            m_Transform = transform;
            m_Data = data;
            m_Grid = grid;
            
            SetTargetNode(grid.GetStartNode());
            grid.GetStartNode().EnemyDatas.Add(data);
        }

        private const float TOLERANCE = 0.01f;

        private Node m_TargetNode;

        public void TickMovement()
        {
            if (m_TargetNode == null)
            {
                return;
            }

            Vector3 target = m_TargetNode.Position;
            Vector3 position = m_Transform.position;
            float sqrDistance = new Vector3(target.x-position.x,0f,target.z-position.z).sqrMagnitude;
            if (sqrDistance < TOLERANCE)
            {
                m_TargetNode.EnemyDatas.Remove(m_Data);
                m_TargetNode = m_TargetNode.NextNode;
                if (m_TargetNode != null)
                {
                    m_TargetNode.EnemyDatas.Add(m_Data);
                }
                return;
            }

            Vector3 dir = new Vector3(target.x-position.x,0f,target.z-position.z).normalized;
            Vector3 delta = dir * (m_Speed * Time.deltaTime);
            m_Transform.Translate(delta);
        }

        public Node GetCurrentNode()
        {
            return m_Grid.GetNodeAtPoint(m_Transform.position);
        }

        private void SetTargetNode(Node node)
        {
            m_TargetNode = node;
        }
    }
}