using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Script
{
    public class Cell
    {
        public Vector3[] Vertices = new Vector3[3];
        public Cell[] Neighbours = new Cell[3];
        
        bool isPolluted = false;
    }
    
    public class CellsManager : MonoBehaviour
    {
        public Mesh planetMesh;

        void Start()
        {
            Vector3[] vertices = planetMesh.vertices;
            int[] triangles = planetMesh.triangles;

            // Step 1: Create cells for each triangle
            int numTriangles = triangles.Length / 3;
            Cell[] cells = new Cell[numTriangles];

            for (int i = 0; i < numTriangles; i++)
            {
                cells[i] = new Cell
                {
                    Vertices = new Vector3[]
                    {
                        vertices[triangles[i * 3]],
                        vertices[triangles[i * 3 + 1]],
                        vertices[triangles[i * 3 + 2]]
                    }
                };
            }

            // Step 2: Create a dictionary to find neighbouring cells
            Dictionary<(Vector3, Vector3), List<Cell>>
                edgeToCellsMap = new Dictionary<(Vector3, Vector3), List<Cell>>();

            for (int i = 0; i < numTriangles; i++)
            {
                Vector3 v1 = cells[i].Vertices[0];
                Vector3 v2 = cells[i].Vertices[1];
                Vector3 v3 = cells[i].Vertices[2];

                AddEdgeToMap(edgeToCellsMap, v1, v2, cells[i]);
                AddEdgeToMap(edgeToCellsMap, v2, v3, cells[i]);
                AddEdgeToMap(edgeToCellsMap, v3, v1, cells[i]);
            }

            // Step 3: Assign neighbours to each cell
            foreach (var cell in cells)
            {
                Vector3 v1 = cell.Vertices[0];
                Vector3 v2 = cell.Vertices[1];
                Vector3 v3 = cell.Vertices[2];

                cell.Neighbours[0] = FindNeighbour(edgeToCellsMap, v1, v2, cell);
                cell.Neighbours[1] = FindNeighbour(edgeToCellsMap, v2, v3, cell);
                cell.Neighbours[2] = FindNeighbour(edgeToCellsMap, v3, v1, cell);
            }

            // Now 'cells' contains all the cells with their neighbours populated
        }

        void AddEdgeToMap(Dictionary<(Vector3, Vector3), List<Cell>> map, Vector3 v1, Vector3 v2, Cell cell)
        {
            (Vector3, Vector3) edge = v1.x < v2.x || (v1.x == v2.x && (v1.y < v2.y || (v1.y == v2.y && v1.z < v2.z)))
                ? (v1, v2)
                : (v2, v1);

            if (!map.ContainsKey(edge))
            {
                map[edge] = new List<Cell>();
            }

            map[edge].Add(cell);
        }

        Cell FindNeighbour(Dictionary<(Vector3, Vector3), List<Cell>> map, Vector3 v1, Vector3 v2, Cell currentCell)
        {
            (Vector3, Vector3) edge = v1.x < v2.x || (v1.x == v2.x && (v1.y < v2.y || (v1.y == v2.y && v1.z < v2.z)))
                ? (v1, v2)
                : (v2, v1);

            if (map.ContainsKey(edge))
            {
                foreach (var cell in map[edge])
                {
                    if (cell != currentCell)
                    {
                        return cell;
                    }
                }
            }

            return null;
        }
    }
}