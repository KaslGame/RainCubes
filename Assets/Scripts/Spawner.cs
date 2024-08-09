using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private float _repeatingRate = 1f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolSize = 5;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
        createFunc: () => Instantiate(_prefab),
        actionOnGet: (obj) => ActionOnGet(obj),
        actionOnRelease: (obj) => obj.gameObject.SetActive(false),
        actionOnDestroy: (obj) => Destroy(obj),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolSize);
    }

    private void Start()
    {
        StartCoroutine(FillPool(_repeatingRate));
    }

    public void ReleaseCube(Cube obj)
    {
        _pool.Release(obj);
    }

    private void ActionOnGet(Cube obj)
    {
        float randomX = Random.Range(-10, 10);
        float randomZ = Random.Range(-10, 10);

        obj.transform.position = new Vector3(randomX, gameObject.transform.position.y, randomZ);

        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
            rb.velocity = Vector3.zero;

        obj.AddSpawner(this);
        obj.gameObject.SetActive(true);
    }

    private IEnumerator FillPool(float time)
    {
        WaitForSeconds wait = new WaitForSeconds(time);

        while (_pool.CountInactive <= _poolCapacity)
        {
            yield return wait;
            _pool.Get();
        }
    }
}
