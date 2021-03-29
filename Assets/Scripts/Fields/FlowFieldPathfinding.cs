using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fields
{
    public class FlowFieldPathfinding
    {
        private Grid m_Grid;
        private Vector2Int m_Target;
        private Vector2Int m_Start;


        public FlowFieldPathfinding(Grid grid, Vector2Int target, Vector2Int start)
        {
            m_Grid = grid;
            m_Target = target;
            m_Start = start;
        }

        private struct Connection
        {
            public float weight;
            public Vector2Int coordinate;

            public Connection(Vector2Int coordinate, float weight)
            {
                this.coordinate = coordinate;
                this.weight = weight;
            }
        }

        public void UpdateField()
        {

            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                node.ResetWeight();
            }

            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            
            queue.Enqueue(m_Target);
            m_Grid.GetNode(m_Target).PathWeight = 0f;

            while (queue.Count > 0)
            {
                Vector2Int current = queue.Dequeue();
                Node currentNode = m_Grid.GetNode(current);

                foreach (Connection neighbour in GetNeighbours(current))
                {
                    Node neighbourNode = m_Grid.GetNode(neighbour.coordinate);
                    if (currentNode.PathWeight + neighbour.weight < neighbourNode.PathWeight)
                    {
                        neighbourNode.NextNode = currentNode;
                        neighbourNode.PathWeight = currentNode.PathWeight + neighbour.weight;
                        queue.Enqueue(neighbour.coordinate);
                    }
                }
            }
            
            Node countNode = m_Grid.GetNode(m_Start);

            foreach (Node node in m_Grid.EnumerateAllNodes())
            {
                node.m_OccupationAvailability = OccupationAvailability.CanOccupy;
            }

            while (countNode != m_Grid.GetNode(m_Target))
            {
                countNode.m_OccupationAvailability = OccupationAvailability.Undefined;
                countNode = countNode.NextNode;
                if ((countNode.Position+new Vector3(0,0,1)).z < m_Grid.GetNode(0,  m_Grid.Height - 1).Position.z)
                {
                    m_Grid.GetNodeAtPoint(countNode.Position+new Vector3(0,0,1)).m_OccupationAvailability = OccupationAvailability.Undefined;
                }
                if ((countNode.Position+new Vector3(-1,0,0)).x >= 0 )
                {
                    m_Grid.GetNodeAtPoint(countNode.Position+new Vector3(-1,0,0)).m_OccupationAvailability = OccupationAvailability.Undefined ;
                }
                if ((countNode.Position+new Vector3(0,0,-1)).z >= 0 )
                {
                    m_Grid.GetNodeAtPoint(countNode.Position+new Vector3(0,0,-1)).m_OccupationAvailability = OccupationAvailability.Undefined ;
                }
                if ((countNode.Position+new Vector3(1,0,0)).x < m_Grid.GetNode(m_Grid.Width - 1,  0).Position.x )
                {
                    m_Grid.GetNodeAtPoint(countNode.Position+new Vector3(1,0,0)).m_OccupationAvailability = OccupationAvailability.Undefined;
                }
            }

            m_Grid.GetNode(m_Start).m_OccupationAvailability = OccupationAvailability.CanNotOccupy;
            m_Grid.GetNode(m_Target).m_OccupationAvailability = OccupationAvailability.CanNotOccupy;
        }

        private IEnumerable<Connection> GetNeighbours(Vector2Int coordinate)
        {
            
            
            Vector2Int rightCoordinate = coordinate + Vector2Int.right;
            Vector2Int leftCoordinate = coordinate + Vector2Int.left;
            Vector2Int upCoordinate = coordinate + Vector2Int.up;
            Vector2Int downCoordinate = coordinate + Vector2Int.down;
            
            Vector2Int rightUpCoordinate = coordinate + Vector2Int.right + Vector2Int.up;
            Vector2Int rightDownCoordinate = coordinate + Vector2Int.right + Vector2Int.down;
            Vector2Int leftUpCoordinate = coordinate + Vector2Int.left + Vector2Int.up;
            Vector2Int leftDownCoordinate = coordinate + Vector2Int.left + Vector2Int.down;

            bool hasRightNode = rightCoordinate.x < m_Grid.Width && !m_Grid.GetNode(rightCoordinate).IsOccupied;
            bool hasLeftNode = leftCoordinate.x >= 0 && !m_Grid.GetNode(leftCoordinate).IsOccupied;
            bool hasUpNode = upCoordinate.y < m_Grid.Height && !m_Grid.GetNode(upCoordinate).IsOccupied;
            bool hasDownNode = downCoordinate.y >= 0 && !m_Grid.GetNode(downCoordinate).IsOccupied;
            
            bool hasRightUpNode = rightCoordinate.x < m_Grid.Width && upCoordinate.y < m_Grid.Height &&
                                  !m_Grid.GetNode(rightCoordinate).IsOccupied &&
                                  !m_Grid.GetNode(upCoordinate).IsOccupied &&
                                  !m_Grid.GetNode(rightUpCoordinate).IsOccupied;
            bool hasRightDownNode = rightCoordinate.x < m_Grid.Width && downCoordinate.y >= 0 && 
                                    !m_Grid.GetNode(rightCoordinate).IsOccupied &&
                                    !m_Grid.GetNode(downCoordinate).IsOccupied &&
                                    !m_Grid.GetNode(rightDownCoordinate).IsOccupied;
            bool hasLeftUpNode = leftCoordinate.x >= 0 && upCoordinate.y < m_Grid.Height &&
                                 !m_Grid.GetNode(leftCoordinate).IsOccupied &&
                                 !m_Grid.GetNode(upCoordinate).IsOccupied &&
                                 !m_Grid.GetNode(leftUpCoordinate).IsOccupied;
            bool hasLeftDownNode = leftCoordinate.x >= 0 && downCoordinate.y >= 0 &&
                                   !m_Grid.GetNode(leftCoordinate).IsOccupied &&
                                   !m_Grid.GetNode(downCoordinate).IsOccupied &&
                                   !m_Grid.GetNode(leftDownCoordinate).IsOccupied;;
            if (hasRightNode)
            {
                yield return new Connection(rightCoordinate, 1f);
            }
            if (hasLeftNode)
            {
                yield return new Connection(leftCoordinate, 1f);;
            }
            if (hasUpNode)
            {
                yield return new Connection(upCoordinate, 1f);;
            }
            if (hasDownNode)
            {
                yield return new Connection(downCoordinate, 1f);;
            }
            if (hasLeftDownNode)
            {
                yield return new Connection(leftDownCoordinate, Mathf.Sqrt(2f));
            }
            if (hasLeftUpNode)
            {
                yield return new Connection(leftUpCoordinate, Mathf.Sqrt(2f));
            }
            if (hasRightDownNode)
            {
                yield return new Connection(rightDownCoordinate, Mathf.Sqrt(2f));
            }
            if (hasRightUpNode)
            {
                yield return new Connection(rightUpCoordinate, Mathf.Sqrt(2f));
            }
        }
    }
}