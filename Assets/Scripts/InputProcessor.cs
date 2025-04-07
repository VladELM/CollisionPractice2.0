using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using Random = System.Random;

public class InputProcessor : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _rayLength;
    [SerializeField] private LayerMask _impactRaycastLayer;

    private int _maxChance;
    private Random _random;

    public event Func<Vector3, Vector3, int, IEnumerable<Rigidbody>> Allowed;
    public event Action<IEnumerable, Vector3> Spawned; 

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
                    IEnumerable<Rigidbody> rigidbodies = Allowed?.Invoke(cubeTransform.position, cubeTransform.localScale, cube.Chance);
                    Spawned?.Invoke(rigidbodies, cubeTransform.position);
                }

            }

        }
    }
}
