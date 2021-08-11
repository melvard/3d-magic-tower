using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Spawner))]
[RequireComponent(typeof(MoveBrick))]
public class GameManager : MonoBehaviour
{
    public Spawner SpawnerScript;
    public MoveBrick MoveScript;
    public ScoreCalculator ScoreCalculatorScript;
    public ParticleSystemManager ParticleSystemManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        MoveScript.StartMotion(SpawnerScript.SpawnBrick());
    }

    // Update is called once per frame
    void Update()
    {

        if (MoveScript.GetLastSliceStatus() != SliceResult.Failed)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                SliceResult result = MoveScript.TryToSlice(SpawnerScript.Tower);
                if (result != SliceResult.Failed)
                {
                    if (result == SliceResult.Perfectmatch)
                    {
                        var meshRenderer = SpawnerScript.Tower.transform.GetChild(1).GetComponent<MeshRenderer>();
                        var position = meshRenderer.bounds.center;
                        var scale = meshRenderer.bounds.extents *4;
                        ParticleSystemManagerScript.SetParticlePosition(position);
                        ParticleSystemManagerScript.SetParticleScale(scale);
                        ParticleSystemManagerScript.AnimatePerfectMatchParticle();
                    }
                    ScoreCalculatorScript.AddScore(1);
                    MoveScript.StartMotion(SpawnerScript.SpawnBrick());
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(0);
        }

    }
}
