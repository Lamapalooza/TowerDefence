using System.Collections;
using System.Collections.Generic;
using Fields;
using UnityEngine;
using Grid = Fields.Grid;

namespace Enemy
{
    public class FlyingMovementAgent : IMovementAgent
    {
        private float m_Speed;
        private Transform m_Transform;

        public FlyingMovementAgent(float speed, Transform transform, Grid grid)
        {
            m_Speed = speed;
            m_Transform = transform;
            
            SetTargetNode(grid.GetTargetNode());
        }

        private const float TOLERANCE = 0.1f;

        private Node m_TargetNode;

        public void TickMovement()
        {
            if (m_TargetNode == null)
            {
                return;
            }

            Vector3 target = m_TargetNode.Position;
            Vector3 position = m_Transform.position;
            
            float distance = new Vector3(target.x-position.x,0f,target.z-position.z).magnitude;
            if (distance < TOLERANCE)
            {
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