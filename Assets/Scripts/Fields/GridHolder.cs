using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Fields
{
    public class GridHolder : MonoBehaviour
    {
        [SerializeField]
        private int m_GridWidth;
        [SerializeField]
        private int m_GridHeight;

        [SerializeField] private Vector2Int m_TargetCoordinate;
        [SerializeField] private Vector2Int m_StartCoordinate;

        [SerializeField] private float m_NodeSize;

        private FlowFieldPathfinding m_Pathfinding;

        private Grid m_Grid;

        private Camera m_Camera;

        private Vector3 m_Offset;

        public Vector2Int StartCoordinate => m_StartCoordinate;

        public Grid Grid => m_Grid;

        private void OnValidate()
        {
            // Default plane size is 10 by 10
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            
            transform.localScale = new Vector3(
                width * 0.1f, 
                1f, 
                height * 0.1f);

            m_Offset = transform.position - 
                       (new Vector3(width, 0f, height)) * 0.5f;
        }

        private void Start()
        {
            m_Camera = Camera.main;
            
            // Default plane size is 10 by 10
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;
            
            transform.localScale = new Vector3(
                width * 0.1f, 
                1f, 
                height * 0.1f);

            m_Offset = transform.position - 
                       (new Vector3(width, 0f, height)) * 0.5f;
            
            m_Grid = new Grid(m_GridWidth, m_GridHeight, m_Offset, m_NodeSize, m_TargetCoordinate, m_StartCoordinate);
        }

        private void Update()
        {
            if (m_Grid == null || m_Camera == null)
            {
                return;
            }

            Vector3 mousePosition = Input.mousePosition;

            Ray ray = m_Camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform != transform)
                {
                    return;
                }

                Vector3 hitPosition = hit.point;
                Vector3 difference = hitPosition - m_Offset;

                int x = (int) (difference.x / m_NodeSize);
                int z = (int) (difference.z / m_NodeSize);

                Vector2Int position = new Vector2Int(x, z);

                if (Input.GetMouseButtonDown(0))
                {
                    Grid.TryOccupyNode(position, Grid.Pathfinding.CanOccupy(position));
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (m_Grid == null)
            {
                return;
            }
            
            Gizmos.color = Color.red;
            
            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                if (node.NextNode == null)
                {
                    continue;
                }

                if (node.IsOccupied)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawSphere(node.Position, 0.5f);
                    continue;
                }
                Vector3 start = node.Position;
                Vector3 end = node.NextNode.Position;

                Vector3 dir = end - start;

                start -= dir * 0.25f;
                end -= dir * 0.75f;
                
                Gizmos.DrawLine(start, end);
                Gizmos.DrawSphere(end, 0.1f);
            }
            
            Gizmos.color = Color.blue;
            for (int i = 0; i < m_GridHeight + 1; i++)
            {
                Gizmos.DrawLine(m_Offset + Vector3.forward * i * m_NodeSize,
                    m_Offset + (Vector3.right * m_GridWidth + Vector3.forward * i) * m_NodeSize);
            }
            for (int i = 0; i < m_GridWidth + 1; i++)
            {
                Gizmos.DrawLine(m_Offset + Vector3.right * m_NodeSize * i,
                    m_Offset + (Vector3.forward * m_GridHeight + Vector3.right * i) * m_NodeSize);
            }
        }
    }
}