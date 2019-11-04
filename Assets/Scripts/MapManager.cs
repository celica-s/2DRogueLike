using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

    public GameObject[] outWalls;
    public GameObject[] floors;
    public GameObject[] walls;
    public GameObject[] foods;
    public GameObject[] enemys;
    public GameObject exit;

    public int row = 10;
    public int column = 10;
    public int minWallCount = 3;
    public int maxWallCount = 9;

    private Transform mapHolder;
    private List<Vector3> positionList = new List<Vector3> ();

    private void InitMap (int level) {
        mapHolder = new GameObject ("Map").transform;
        positionList.Clear ();
        for (int i = 0; i < column; i++) {
            for (int j = 0; j < row; j++) {
                if (i == 0 || j == 0 || j == row - 1 || i == column - 1) {
                    GameObject go = Instantiate (randomPrefab (outWalls), new Vector3 (i, j, 0), Quaternion.identity);
                    go.transform.SetParent (mapHolder);
                } else {
                    GameObject go = Instantiate (randomPrefab (floors), new Vector3 (i, j, 0), Quaternion.identity);
                    go.transform.SetParent (mapHolder);
                }

                if (i < column - 2 && i > 1 && j < row - 2 && j > 1) {
                    positionList.Add (new Vector3 (i, j, 0));
                }
            }
        }

        int wallCount = Random.Range (minWallCount, maxWallCount);
        InstantiateItemsInRandomPosition (wallCount, walls);

        int foodCount = Random.Range (2, level * 2 + 1);
        InstantiateItemsInRandomPosition (foodCount, foods);

        int enemyCount = level / 2;
        InstantiateItemsInRandomPosition (enemyCount, enemys);

        GameObject ex = Instantiate (exit, new Vector3 (column - 2, row - 2, 0), Quaternion.identity);
        ex.transform.SetParent (mapHolder);
    }

    public void SetupScene (int level) {
        InitMap (level);
    }

    private void InstantiateItemsInRandomPosition (int count, GameObject[] prefabs) {
        for (int i = 0; i < count; i++) {
            Vector3 position = randomPosition ();
            GameObject go = Instantiate (randomPrefab (prefabs), position, Quaternion.identity);
            go.transform.SetParent (mapHolder);
        }
    }

    private Vector3 randomPosition () {
        int positionIndex = Random.Range (0, positionList.Count);
        Vector3 postion = positionList[positionIndex];
        positionList.RemoveAt (positionIndex);

        return postion;
    }

    private GameObject randomPrefab (GameObject[] prefabs) {
        int index = Random.Range (0, prefabs.Length);
        return prefabs[index];
    }
}