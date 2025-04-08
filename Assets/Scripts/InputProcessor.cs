using UnityEngine;
using System.Collections.Generic;
using Random = System.Random;

public class InputProcessor : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Exploder _exploder;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _rayLength;
    [SerializeField] private LayerMask _impactRaycastLayer;

    private int _maxChance;
    private Random _random;

    private void OnEnable()
    {
        _inputReader.Pushed += ProcessPushing;
        _maxChance = 100;
        _random = new Random();
    }

    private void OnDisable()
    {
        _inputReader.Pushed -= ProcessPushing;
    }

    private void ProcessPushing(Vector3 position)
    {
        Ray ray = _camera.ScreenPointToRay(position);
        Debug.DrawRay(ray.origin, ray.direction * _rayLength, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, _rayLength, _impactRaycastLayer))
        {
            if (hit.transform.gameObject.TryGetComponent(out Cube cube))
            {
                Transform cubeTransform = hit.transform;
                Destroy(cubeTransform.gameObject);

                if (_random.Next(_maxChance + 1) <= cube.Chance)
                {
                    IEnumerable<Rigidbody> rigidbodies = _spawner.Spawn(cubeTransform.position, cubeTransform.localScale, cube.Chance);
                    _exploder.Explode(rigidbodies, cubeTransform.position);
                }
                else
                {
                    cube.gameObject.TryGetComponent(out Rigidbody cubeRigidbody);
                    List<Rigidbody> rigidbodies = GetRigidbodies(hit.transform.position, cube.Dimension, cubeRigidbody);
                    _exploder.Explode(rigidbodies, cubeTransform.position, cube.transform.localScale.x);
                }
            }
        }
    }

    private List<Rigidbody> GetRigidbodies(Vector3 position, float cubeDimension, Rigidbody destroyedCube)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, _exploder.ExplosionRadius / cubeDimension,
                                                        _impactRaycastLayer);
        List<Rigidbody> rigidbodies = new List<Rigidbody>();

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.TryGetComponent(out Rigidbody rigidbody))
            {
                if (rigidbody == destroyedCube)
                    continue;
                else
                    rigidbodies.Add(rigidbody);
            }
        }

        return rigidbodies;
    }
}
