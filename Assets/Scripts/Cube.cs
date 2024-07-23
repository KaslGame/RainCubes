using System.Collections;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Cube : MonoBehaviour
{
    [SerializeField] private Material _blackBlueMaterial;
    [SerializeField] private Material _blueMaterial;

    private Spawner _spawner;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Platform>(out Platform platform))
            StartCoroutine(LifeTimer(RandomNumber()));
    }

    public void AddSpawner(Spawner spawner)
    {
        _spawner = spawner;
    }

    private float RandomNumber()
    {
        int minLifeTime = 2;
        int maxLifeTime = 5;

        float radomLifeTime = Random.Range(minLifeTime, maxLifeTime);
        return radomLifeTime;
    }

    private IEnumerator LifeTimer(float time)
    {
        WaitForSeconds wait = new WaitForSeconds(time);

        _meshRenderer.material = _blackBlueMaterial;
        yield return wait;
        _spawner.ReleaseCube(this.gameObject);
        _meshRenderer.material = _blueMaterial;
    }
}
