using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Spawner : MonoBehaviour
{
    [SerializeField] public GameObject Tower;
    [SerializeField] private GameObject spawnPointNorth;
    [SerializeField] private GameObject spawnPointWest;

    [NonSerialized] public GameObject CurrentBrick;
    private List<(Transform, AxisDirection)> spawnPoints;
    private float defBrickHeight;
    private static bool firstTime = true;

    public (GameObject, AxisDirection) SpawnBrick()
    {
        SetSpawnPointPositions();
        defBrickHeight = Tower.transform.GetChild(0).localScale.y;



        CurrentBrick = Instantiate(Tower.transform.GetChild(0).gameObject);
        var rand = new System.Random();
        int index = rand.Next(spawnPoints.Count);
        //int index = 0;
        var pair = spawnPoints[index];
        Material currentMaterial = CurrentBrick.GetComponent<Renderer>().material;
        //var randMaterial = new Material(currentMaterial);
        //var r = UnityEngine.Random.Range(0f, 1f);
        //var g = UnityEngine.Random.Range(0f, 1f);
        //var b = UnityEngine.Random.Range(0f, 1f);
        //randMaterial.color = new Color(r, g, b, 1);
        CurrentBrick.GetComponent<Renderer>().material = currentMaterial;
        CurrentBrick.transform.position = pair.Item1.position;
        CurrentBrick.transform.SetParent(Tower.transform);
        CurrentBrick.transform.SetAsFirstSibling();
        CurrentBrick.name = "Brick";

        return (CurrentBrick, pair.Item2);
    }

    private void LowerTower(float offset)
    {
        StartCoroutine(LowerTheTower(0.5f, offset));
        DestroyImmediate(Tower.transform.GetChild(Tower.transform.childCount - 1).gameObject);
    }

    private IEnumerator LowerTheTower(float duration, float offset)
    {
        Vector3 startPos = Tower.transform.position;
        Vector3 endPos = Tower.transform.position -= new Vector3(0, offset, 0);
        for (float i = 0; i <= duration; i += Time.deltaTime)
        {

            Tower.transform.position = Vector3.Lerp(startPos, endPos, i / duration);
            yield return null;
        }
        Tower.transform.position = Vector3.Lerp(startPos, endPos, 1);

    }

    private void SetSpawnPointPositions()
    {
        var lastBrickPosition = Tower.transform.GetChild(0).position;
        spawnPointNorth.transform.position = new Vector3(spawnPointNorth.transform.position.x,
                                                         lastBrickPosition.y,
                                                         lastBrickPosition.z);
        spawnPointWest.transform.position = new Vector3(lastBrickPosition.x,
                                                        lastBrickPosition.y,
                                                        spawnPointWest.transform.position.z);
        spawnPoints = new List<(Transform, AxisDirection)>
        {
            (spawnPointNorth.transform, AxisDirection.X),
            (spawnPointWest.transform ,AxisDirection.Z)
        };
    }

}
