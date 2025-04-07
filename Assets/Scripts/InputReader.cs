using System;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    [SerializeField] public int _buttonValue;

    public event Action<Vector3> Pushed;

    private void Update()
    {
        if (Input.GetMouseButtonDown(_buttonValue))
            Pushed?.Invoke(Input.mousePosition);
    }
}
