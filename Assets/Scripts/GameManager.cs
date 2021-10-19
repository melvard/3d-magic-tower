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
    public SceneMover SceneMoverScript;

    [SerializeField] private GameObject GameOverAlert;
    // Start is called before the first frame update
    void Start()
    {
        GameOverAlert.SetActive(false);
        SceneMoverScript.LowerTower();
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
                    SceneMoverScript.LowerTower();
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
                    var brickAxisPair = SpawnerScript.SpawnBrick();
                    MoveScript.StartMotion(brickAxisPair);
                }
                else
                {
                    GameOverAlert.SetActive(true);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StartAgain();
            StartAgain();
        }
        else if(Input.GetKeyDown(KeyCode.P))
        {
            UnityEditor.EditorApplication.isPaused = true;
        }

    }

    public void StartAgain()
    {
        SceneManager.LoadScene(0);

    }
}
