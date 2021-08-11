using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Slicer))]
public class MoveBrick : MonoBehaviour
{
    [SerializeField] private float moveTime = 10f;
    [SerializeField] private float perfectMatchLoss = 0.1f;
    [SerializeField] private Slicer slicer;

    private SliceResult sliceResult = SliceResult.Success;
    private GameObject currentBrick;
    private AxisDirection direction;
    private int i = 1;

    private void Update()
    {
        if (currentBrick != null)
        {
            Debug.Log(currentBrick.transform.position);
        }
    }

    public SliceResult GetLastSliceStatus()
    {
        return sliceResult;
    }
    public void StartMotion((GameObject, AxisDirection) pair)
    {
        currentBrick = pair.Item1;
        direction = pair.Item2;
        StartCoroutine(MoveCurrentBrick(moveTime));
    }

    public SliceResult TryToSlice(GameObject tower)
    {
        StopAllCoroutines();
        StartCoroutine(SliceBrick(tower));
        return sliceResult;
    }

    private IEnumerator MoveCurrentBrick(float duration)
    {

        Vector3 startPos = currentBrick.transform.position;
        Vector3 endPos = Vector3.zero;
        if (direction == AxisDirection.X)
        {
            endPos = new Vector3(startPos.x * -1, startPos.y, startPos.z);

        }
        else if (direction == AxisDirection.Z)
        {
            endPos = new Vector3(startPos.x, startPos.y, startPos.z * -1);
        }
        while (true)
        {
            for (float i = 0; i <= duration; i += Time.deltaTime)
            {

                currentBrick.transform.position = Vector3.Lerp(startPos, endPos, i / duration);
                yield return null;
            }
            currentBrick.transform.position = Vector3.Lerp(startPos, endPos, 1);
            SwapValues(ref endPos, ref startPos);
        }


    }

    private void SwapValues(ref Vector3 vect1, ref Vector3 vect2)
    {
        var tmp = vect1;
        vect1 = vect2;
        vect2 = tmp;
    }
    private IEnumerator SliceBrick(GameObject tower)
    {
        Vector3 previousBrickPosition = tower.transform.GetChild(1).GetComponent<MeshRenderer>().bounds.center;
        Vector3 previousBrickScale = tower.transform.GetChild(1).GetComponent<MeshRenderer>().bounds.extents;
        Vector3 currentBrickPosition = currentBrick.GetComponent<MeshRenderer>().bounds.center;
        Vector3 currentBrickScale = currentBrick.GetComponent<MeshRenderer>().bounds.extents;

        Debug.Log("Previous pos:" + previousBrickPosition);
        Debug.Log("Previous scale:" + previousBrickScale);
        Debug.Log("Current pos:" + currentBrickPosition);
        Debug.Log("Current scale:" + currentBrickScale);




        if (direction == AxisDirection.X && Mathf.Abs(previousBrickPosition.x - currentBrickPosition.x) <= perfectMatchLoss)
        {
            sliceResult = SliceResult.Perfectmatch;
            currentBrick.transform.position = new Vector3(tower.transform.GetChild(1).transform.position.x, currentBrick.transform.position.y, currentBrick.transform.position.z);
        }
        else if (direction == AxisDirection.Z && Mathf.Abs(previousBrickPosition.z - currentBrickPosition.z) <= perfectMatchLoss)
        {
            sliceResult = SliceResult.Perfectmatch;
            currentBrick.transform.position = new Vector3(currentBrick.transform.position.x, currentBrick.transform.position.y, tower.transform.GetChild(1).transform.position.z);
        }
        else
        {
            Vector3 knifePosition = Vector3.zero;
            Vector3 knifeDirection = Vector3.zero;
            bool reverse = false;
            if (direction == AxisDirection.X)
            {
                var distance = Mathf.Abs(currentBrickPosition.x - previousBrickPosition.x);
                if (distance >= previousBrickScale.x * 2)
                {
                    sliceResult = SliceResult.Failed;
                    currentBrick.GetComponent<Rigidbody>().isKinematic = false;
                    yield break;
                }
                else
                {
                    knifeDirection = slicer.gameObject.transform.right;
                    if (currentBrickPosition.x - previousBrickPosition.x > 0)
                    {
                        reverse = false;
                        knifePosition = new Vector3(previousBrickPosition.x + previousBrickScale.x, previousBrickPosition.y, previousBrickPosition.z);
                    }
                    else
                    {
                        reverse = true;

                        knifePosition = new Vector3(previousBrickPosition.x - previousBrickScale.x, previousBrickPosition.y, previousBrickPosition.z);

                    }
                }
            }
            else if (direction == AxisDirection.Z)
            {
                var distance = Mathf.Abs(currentBrickPosition.z - previousBrickPosition.z);
                if (distance >= previousBrickScale.z * 2)
                {
                    sliceResult = SliceResult.Failed;
                    currentBrick.GetComponent<Rigidbody>().isKinematic = false;
                    yield break;
                }
                else
                {
                    knifeDirection = slicer.gameObject.transform.forward;
                    if (currentBrickPosition.z - previousBrickPosition.z > 0)
                    {
                        reverse = false;
                        knifePosition = new Vector3(previousBrickPosition.x, previousBrickPosition.y, previousBrickPosition.z + previousBrickScale.z);
                    }
                    else
                    {
                        reverse = true;
                        knifePosition = new Vector3(previousBrickPosition.x, previousBrickPosition.y, previousBrickPosition.z - previousBrickScale.z);
                    }
                }
            }
            sliceResult = SliceResult.Success;
            currentBrick = slicer.Slice(reverse, knifePosition, knifeDirection);
        }


    }

    /*private void SetBrick()
    {
        var previousPosition = spawner.Tower.transform.GetChild(1).position;
        var currentPosition = currentBrick.transform.position;
        var previousScale = spawner.Tower.transform.GetChild(1).localScale;
        var currentScale = currentBrick.transform.localScale;
        if ((previousPosition - currentPosition).magnitude <= new Vector3(0.15f, 0f, 0.15f).magnitude)
        {
            currentBrick.transform.position = new Vector3(previousPosition.x, currentPosition.y, previousPosition.z);
        }
        else
        {


            var takenBrickScale = Vector3.zero;
            var lostBrickScale = Vector3.zero;
            var takenBrickPosition = Vector3.zero;
            var lostBrickPosition = Vector3.zero;


            if (direction == AxisDirection.X)
            {
                var distance = System.Math.Abs(previousPosition.x - currentPosition.x);
                if (distance >= previousScale.x)
                {
                    isGameOver = true;
                }

                var halfOfScale = previousScale.x / 2;

                takenBrickScale = new Vector3(distance - 2 * (distance - halfOfScale),
                                              currentScale.y,
                                              currentScale.z);
                lostBrickScale = new Vector3(currentScale.x - takenBrickScale.x,
                                             currentScale.y,
                                             currentScale.z);
                takenBrickPosition = new Vector3((previousPosition.x + currentPosition.x) / 2,
                                                  currentPosition.y,
                                                  currentPosition.z);

                float x = 0f;
                if (previousPosition.x - currentPosition.x < 0)
                {

                    x = takenBrickPosition.x + takenBrickScale.x / 2 + lostBrickScale.x / 2;
                }
                else
                {
                    x = takenBrickPosition.x - takenBrickScale.x / 2 - lostBrickScale.x / 2;
                }
                lostBrickPosition = new Vector3(x, currentPosition.y, currentPosition.z);

            }
            else if (direction == AxisDirection.Z)
            {
                var distance = System.Math.Abs(previousPosition.z - currentPosition.z);
                if (distance >= previousScale.z)
                {
                    isGameOver = true;
                }
                var halfOfScale = previousScale.z / 2;
                takenBrickScale = new Vector3(currentScale.x,
                                              currentScale.y,
                                              distance - 2 * (distance - halfOfScale));
                lostBrickScale = new Vector3(currentScale.x,
                                             currentScale.y,
                                             currentScale.z - takenBrickScale.z);
                takenBrickPosition = new Vector3(currentPosition.x,
                                                 currentPosition.y,
                                                 (previousPosition.z + currentPosition.z) / 2);

                float z = 0f;
                if (previousPosition.z - currentPosition.z < 0)
                {

                    z = takenBrickPosition.z + takenBrickScale.z / 2 + lostBrickScale.z / 2;
                }
                else
                {
                    z = takenBrickPosition.z - takenBrickScale.z / 2 - lostBrickScale.z / 2;
                }
                lostBrickPosition = new Vector3(currentPosition.x, currentPosition.y, z);
            }
            

            if (!isGameOver)
            {
                GameObject lostBrick = Instantiate(currentBrick);
                lostBrick.transform.position = lostBrickPosition;
                lostBrick.transform.localScale = lostBrickScale;
                lostBrick.GetComponent<Renderer>().material = loseMaterial;
                lostBrick.GetComponent<Rigidbody>().isKinematic = false;
                //lostBrick.SetActive(false);

                GameObject takenBrick = Instantiate(currentBrick);
                takenBrick.transform.position = takenBrickPosition;
                takenBrick.transform.localScale = takenBrickScale;
                Material mat = new Material(currentBrick.GetComponent<Renderer>().material);
                Debug.Log("Old" + mat.mainTextureOffset.x);
                mat.mainTextureScale = new Vector2(mat.mainTextureScale.x * (takenBrickScale.x / currentScale.x), mat.mainTextureScale.y * (takenBrickScale.z / currentScale.z));
                Debug.Log("Middle" + mat.mainTextureOffset.x);
                if (currentPosition.x - takenBrickPosition.x > 0)
                {
                    var lossOffset = (spawner.Tower.transform.GetChild(i).lossyScale.x - currentBrick.transform.lossyScale.x) / spawner.Tower.transform.GetChild(i).lossyScale.x/2;
                    Debug.Log(lossOffset);
                    mat.mainTextureOffset = new Vector2(lossOffset + lostBrick.transform.lossyScale.x/spawner.Tower.transform.GetChild(i).lossyScale.x, 0);
                    i++;
                }
                else
                {
                    mat.mainTextureOffset = new Vector2(mat.mainTextureOffset.x, 0);
                }
                Debug.Log("New" + mat.mainTextureOffset.x);
                takenBrick.transform.SetParent(spawner.Tower.transform);
                takenBrick.transform.SetAsFirstSibling();
                //currentMaterial.mainTextureOffset = new Vector2(currentMaterial.mainTextureOffset.x + Offset(currentPosition.x - takenBrickPosition.x) * (currentScale.x - takenBrickScale.x)/currentScale.x, currentMaterial.mainTextureOffset.y + Offset(currentPosition.z - takenBrickPosition.z) * (currentScale.z - takenBrickScale.z) / currentScale.z);
                takenBrick.GetComponent<Renderer>().material = mat;
                //UnityEditor.EditorApplication.isPaused = true;

                DestroyImmediate(spawner.CurrentBrick);
                spawner.CurrentBrick = takenBrick;
            }
            else
            {
                currentBrick.GetComponent<Rigidbody>().isKinematic = false;
            }
            
        }

    }*/
}
