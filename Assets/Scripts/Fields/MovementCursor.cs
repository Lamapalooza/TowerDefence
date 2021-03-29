using System;
using Enemy;
using UnityEngine;

namespace Fields
{
    public class MovementCursor : MonoBehaviour
    {
        private IMovementAgent m_MovementAgent;
        [SerializeField] private GameObject m_Cursor;
        
        [SerializeField] private int m_GridWidth;
        [SerializeField] private int m_GridHeight;

        [SerializeField] private float m_NodeSize;

        private Camera m_Camera;

        private Vector3 m_Offset;
        
        private void Start()
        {
            m_Camera = Camera.main;
            
            // Default plane size is 10 by 10
            float width = m_GridWidth * m_NodeSize;
            float height = m_GridHeight * m_NodeSize;

            m_Offset = transform.position - 
                       (new Vector3(width, 0f, height)) * 0.5f;
        }

        private void Update()
        {
            if (m_Camera == null)
            {
                return;
            }
            
            Vector3 mousePosition = Input.mousePosition;

            Ray ray = m_Camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {

                if (hit.transform != transform || m_Cursor == null || m_MovementAgent == null) 
                {
                    return;
                }
                
                m_Cursor.SetActive(true);
                
                Vector3 hitPosition = hit.point;
                Vector3 difference = hitPosition - m_Offset;

                int x = (int) (difference.x / m_NodeSize);
                int z = (int) (difference.z / m_NodeSize);

                Vector3 centreOfNode = m_Offset + (new Vector3((x + 0.5f), 0f, (z + 0.5f)) * m_NodeSize);
                
                m_Cursor.transform.position = centreOfNode;
                
                if (Input.GetMouseButtonDown(1))
                {
                    
                }
            }
            else
            {
                m_Cursor.SetActive(false);
            }
        }
    }
}