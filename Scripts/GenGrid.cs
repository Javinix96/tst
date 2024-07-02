using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenGrid : MonoBehaviour
{

    private World world;
    [SerializeField]
    private Vector2Int posI;
    [SerializeField]
    private Vector2Int posF;

    [SerializeField]
    private float sizeNode;
    private Node[,] grid;
    [SerializeField]
    private List<Node> temp;
    private List<Node> openList;
    private List<Node> closedList;

    private List<Node> path;


    public int size;

    // Start is called before the first frame update
    void Start()
    {
        world = new World();
        grid = new Node[size, size];
        StartGrid();
        temp = ArrayToList(grid);
        StartCoroutine(world.GeneratePath(posI, posF, grid, GetPath));
    }

    private void StartGrid()
    {
        int id = 0;
        for (int i = 0; i < size; i++)
        {
            for (int e = 0; e < size; e++)
            {
                Vector3 pos = new Vector3(i * sizeNode, 0, e * sizeNode);
                int busy = Physics.CheckSphere(pos, sizeNode) ? 1 : 0;
                Node n = new Node(id, 0, 0, 0, pos, null, busy, new Vector2Int(i, e));
                id++;
                grid[i, e] = n;
            }
        }
    }

    private void GetPath(List<Node> _path, List<Node> _open, List<Node> _closed)
    {
        path = _path;
        openList = _open;
        closedList = _closed;
    }

    private List<T> ArrayToList<T>(T[,] arr)
    {
        List<T> list = new List<T>();
        foreach (T e in arr)
            list.Add(e);
        return list;
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < size; i++)
        {
            for (int e = 0; e < size; e++)
            {
                Vector3 n = new Vector3(i * sizeNode, 0, e * sizeNode);
                if (grid == null)
                {

                    Vector2Int pt = new Vector2Int(i, e);
                    Gizmos.color = Color.gray;
                    Gizmos.DrawCube(n, new Vector3(sizeNode - 0.5f, sizeNode - 0.5f, sizeNode - 0.5f));
                }
                else
                {
                    n = grid[i, e].Pos;

                    if (path != null)
                    {

                        if (path.Contains(grid[i, e]))
                        {
                            Gizmos.color = Color.green;
                            Gizmos.DrawCube(n, new Vector3(sizeNode, sizeNode, sizeNode));
                            continue;
                        }
                    }
                    if (openList == null)
                        continue;


                    if (openList.Contains(grid[i, e]))
                    {
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawCube(n, new Vector3(sizeNode - 0.5f, sizeNode - 0.05f, sizeNode - 0.05f));
                        continue;
                    }
                    if (closedList.Contains(grid[i, e]))
                    {
                        Gizmos.color = Color.yellow;
                        Gizmos.DrawCube(n, new Vector3(sizeNode - 0.05f, sizeNode - 0.05f, sizeNode - 0.05f));
                        continue;
                    }

                    Gizmos.color = grid[i, e].Busy == 1 ? Color.red : Color.gray;
                    Gizmos.DrawCube(n, new Vector3(sizeNode, sizeNode, sizeNode));
                }

                if (i == posI.x && e == posI.y)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawCube(n, new Vector3(0.8f, 0.8f, 0.8f));
                    continue;
                }

                if (i == posF.x && e == posF.y)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawCube(n, new Vector3(sizeNode, sizeNode, sizeNode));
                    continue;
                }
            }
        }
    }

}
