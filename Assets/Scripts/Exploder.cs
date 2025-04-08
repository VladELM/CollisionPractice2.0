using System.Collections;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private float _defaultExplosionForce;
    [SerializeField] private float _defaultExplosionRadius;

    public float ExplosionRadius => _defaultExplosionRadius;

    public void Explode(IEnumerable explodableObjects, Vector3 position)
    {
        foreach (Rigidbody explodableObject in explodableObjects)
            explodableObject.AddExplosionForce(_defaultExplosionForce, position, _defaultExplosionRadius);
    }

    public void Explode(IEnumerable explodableObjects, Vector3 position, float cubeScale)
    {
        float correctedForce = (int)(_defaultExplosionForce / cubeScale);
        float correctedRadius = (int)(_defaultExplosionRadius / cubeScale);

        foreach (Rigidbody explodableObject in explodableObjects)
        {
            float distance = Vector3.Distance(position, explodableObject.transform.position);
            float finalForce = (int)(correctedForce / distance);
            explodableObject.AddExplosionForce(finalForce, position, correctedRadius);
        }
    }
}
