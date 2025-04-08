using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    [SerializeField] private int _chance;

    private float _dimension;

    public int Chance => _chance;
    public float Dimension => _dimension;

    private void OnEnable()
    {
        GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV();
        _dimension = transform.localScale.x;
    }

    public void Initialize(int chance)
    {
        _chance = chance;
    }
}
