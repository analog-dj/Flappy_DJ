using UnityEngine;

public class Paralaxer : MonoBehaviour
{
    class PoolObject
    {
        public Transform transform;
        public bool inUse;

        public PoolObject(Transform t) { transform = t; }

        public void Use() { inUse = true; }

        public void Dispose() { inUse = false; }
    }

    [System.Serializable]
    public struct YSpawnRange 
    {
        public float min;
        public float max;
    }

    public GameObject prefab;
    public int poolSize; 
    public float shiftSpeed;
    public float spawnRate;

    public YSpawnRange ySpawnRange;
    public Vector3 defaultSpawnPos;
    public bool spawnImmediate; //particle prewarm
    public Vector3 immediateSpawnPos;
    public Vector2 targetAspectRatio;

    float spawnTimer;
    float targetAspect;
    PoolObject[] poolObjects;
    GameplayManager game;

    void Awake()
    {
        Configure();
    }

    private void Start()
    {
        game = GameplayManager.Instance;
    }

    void OnEnable()
    {
        GameplayManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    void OnDisable()
    {
        GameplayManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }
    void OnGameOverConfirmed() 
    {
        for (int i = 0; i < poolObjects.Length; i++) 
        {
            poolObjects[i].Dispose();
            poolObjects[i].transform.position = Vector3.one * 1000;
        }
        if (spawnImmediate)
        {
            SpawnImmediate();
        }
    }

    void Update()
    {
        if (game.GameOver)
            return;

        Shift();
        spawnTimer += Time.deltaTime;
        if (spawnTimer > spawnRate) 
        {
            Spawn();
            spawnTimer = 0;
        }
    }

    void Configure()
    {
        targetAspect = targetAspectRatio.x / targetAspectRatio.y;
        poolObjects = new PoolObject[poolSize];
        for (int i = 0; i < poolObjects.Length; i++) 
        {
            GameObject go = Instantiate(prefab) as GameObject;
            Transform t = go.transform;
            t.SetParent(transform);
            t.position = Vector3.one * 1000;
            poolObjects[i] = new PoolObject(t);
        }

        if (spawnImmediate) 
        {
            SpawnImmediate();
        }
    }

    void Spawn() 
    {
        Transform t = GetPoolObject();
        if (t == null)
            return; //if true -> indicates that poolSize is too small

        Vector3 pos = Vector3.zero;
        pos.x = (defaultSpawnPos.x * Camera.main.aspect) / targetAspect;
        pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
        t.position = pos;
    }

    void SpawnImmediate()
    {
        Transform t = GetPoolObject();
        if (t == null)
            return; //if true -> indicates that poolSize is too small

        Vector3 pos = Vector3.zero;
        pos.x = (immediateSpawnPos.x * Camera.main.aspect) / targetAspect;
        pos.y = Random.Range(ySpawnRange.min, ySpawnRange.max);
        t.position = pos;
        Spawn();
    }

    void Shift() 
    {
        for (int i = 0; i < poolObjects.Length; i++) 
        {
            poolObjects[i].transform.position += -Vector3.right * shiftSpeed * Time.deltaTime;
            CheckDisposeObject(poolObjects[i]);
        }
    }

    void CheckDisposeObject(PoolObject poolObject)
    {
        if (poolObject.transform.position.x < -defaultSpawnPos.x * Camera.main.aspect / targetAspect)
        {
            poolObject.Dispose();
            poolObject.transform.position = Vector3.one * 1000;
        }
    
    }

    Transform GetPoolObject()
    { 
        for (int i = 0; i < poolObjects.Length; i++)
        {
            if (!poolObjects[i].inUse)
            {
                poolObjects[i].Use();
                return poolObjects[i].transform;
            }

        }
        return null;
    }
}