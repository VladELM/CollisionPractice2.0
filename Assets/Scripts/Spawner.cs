using Random = System.Random;
using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] private InputProcessor _manager;
    [SerializeField] private Cube _cube;
    [SerializeField] private int _minAmount;
    [SerializeField] private int _maxAmount;
    [SerializeField] private int _scaleDivider;
    [SerializeField] private int _chanceDivider;

    private Random _random;

    private void OnEnable()
    {
        _manager.Allowed += Spawn;
        _random = new Random();
    }

    private void OnDisable()
    {
        _manager.Allowed -= Spawn;
    }

    private IEnumerable<Rigidbody> Spawn(Vector3 position, Vector3 scale, int chance)
    {
        List<Rigidbody> rigidbodies = new List<Rigidbody>();

        int amount = _random.Next(_minAmount, _maxAmount);
        int reducedChance = chance / _chanceDivider;

        for (int i = 0; i < amount; i++)
        {
            var cube = Instantiate(_cube, position, Quaternion.identity);
            cube.transform.localScale = scale / _scaleDivider;
            cube.Initialize(reducedChance);
                
            if (cube.TryGetComponent(out Rigidbody rigidbody))
                rigidbodies.Add(rigidbody);
        }

        return rigidbodies;
    }
}
