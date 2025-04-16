using UnityEngine;

public class CameraDitherSystem : MonoBehaviour
{
    [SerializeField]
    private Material _material;
    [SerializeField]
    private Transform _target;
    [SerializeField]
    private string _valueName = "_TargetPosition";

    private void Start()
    {
        if (_material == null)
        {
            Debug.LogError("Material is not assigned.");
            return;
        }
        if (_target == null)
        {
            _target = transform;
        }
    }

    void LateUpdate()
    {
        _material.SetVector(_valueName, _target.position);
    }
}
