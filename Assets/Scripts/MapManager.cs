using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour {

    public GameObject[] floors;
    public GameObject[] outWalls;
    public GameObject[] walls;
    public GameObject[] foods;
    public GameObject[] enemy;

    public int rows = 10;
    public int cols = 10;
    public int minCountWalls = 6;
    public int maxCountWalls = 12;

    private Transform mapHolder;
    public GameObject exit;

    private List<Vector2> positions = new List<Vector2>();
    
    public void InitMap()
    {
        mapHolder = new GameObject("Map").transform;
        //Set outwalls and floors
        for(int x = 0; x < cols; x++)
        {
            for(int y = 0; y < rows; y++)
            {
                if (x == 0 || y == 0 || x == cols - 1 || y == rows - 1)
                {
                    int index = Random.Range(0, outWalls.Length);
                    GameObject temp=GameObject.Instantiate(outWalls[index], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                    temp.transform.SetParent(mapHolder);
                }else
                {
                    int index = Random.Range(0, floors.Length);
                    GameObject temp=GameObject.Instantiate(floors[index], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                    temp.transform.SetParent(mapHolder);
                }
            }
        }
        //Store all positions in list
        positions.Clear();
        for(int x = 2; x < cols-2; x++)
        {
            for(int y = 2; y < rows - 2; y++)
            {
                positions.Add(new Vector2(x, y));
            }
        }
        //Set Walls
        SetItems(Random.Range(minCountWalls, maxCountWalls + 1), walls);
      
        //Set foods
        SetItems(Random.Range(0, GameManager.Instance.level + 1), foods);
      
        //Set Enemy
        SetItems(GameManager.Instance.level / 2, enemy);
        //Set exit and player
        GameObject temp1=GameObject.Instantiate(exit, new Vector2(cols - 2, rows - 2), Quaternion.identity) as GameObject;
        temp1.transform.SetParent(mapHolder);
    }  


    private Vector2 RandomPosition()
    {
        int index = Random.Range(0, positions.Count);
        Vector2 temp = positions[index];
        positions.RemoveAt(index);
        return temp;
    }

    private GameObject RandomPrefabs(GameObject[] prefabs)
    {
        int index = Random.Range(0, prefabs.Length);
        return prefabs[index];
    }

    private void SetItems(int count, GameObject[] array)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject temp = GameObject.Instantiate(RandomPrefabs(array), RandomPosition(), Quaternion.identity) as GameObject;
            temp.transform.SetParent(mapHolder);
        }
    }

}
