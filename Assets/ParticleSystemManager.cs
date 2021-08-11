using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem brickPerfectMatchParticleSystem;

    private void Start()
    {
        brickPerfectMatchParticleSystem.Stop();
    }
    public void SetParticlePosition(Vector3 position)
    {
        ParticleSystem.ShapeModule _editableShape = brickPerfectMatchParticleSystem.shape;
        _editableShape.position = position;
    }
    public void SetParticleScale(Vector3 scale)
    {
        var main = brickPerfectMatchParticleSystem.main;
        main.startSize3D = true;
        main.startSizeXMultiplier = scale.x;
        main.startSizeYMultiplier = scale.z;
    }

    public void AnimatePerfectMatchParticle()
    {
        brickPerfectMatchParticleSystem.Play();
    }
}
