using UnityEngine;

[RequireComponent(typeof(Renderer))]

public class Cube : MonoBehaviour
{
    [SerializeField] private int _chance;

    public int Chance => _chance;

    private void OnEnable()
    {
        GetComponent<Renderer>().material.color = UnityEngine.Random.ColorHSV();
    }

    public void Initialize(int chance)
    {
        _chance = chance;
    }
}
