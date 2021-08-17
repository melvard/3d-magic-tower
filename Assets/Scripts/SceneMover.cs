using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMover : MonoBehaviour
{
    [SerializeField] private GameObject defaultBrick;
    [SerializeField] private GameObject Tower;
    
    public void LowerTower()
    {
        StartCoroutine(LowerTheTower(0.5f));
        //DestroyImmediate(Tower.transform.GetChild(Tower.transform.childCount - 1).gameObject);

    }

    private IEnumerator LowerTheTower(float duration)
    {
        var moveSize = defaultBrick.transform.localScale.y;
        Vector3 startPos = Tower.transform.position;
        Vector3 endPos = Tower.transform.position -= new Vector3(0, moveSize, 0);
        for (float i = 0; i <= duration; i += Time.deltaTime)
        {

            Tower.transform.position = Vector3.Lerp(startPos, endPos, i / duration);
            yield return null;
        }
        Tower.transform.position = Vector3.Lerp(startPos, endPos, 1);
    }
}
