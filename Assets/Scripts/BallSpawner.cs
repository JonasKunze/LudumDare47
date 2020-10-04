using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;

public class BallSpawner : SerializableObject, IInteractable
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float speedFactor = 10;

    private Interactable _interactable;

    private void Start()
    {
        Menu.OnGameStarted.AddListener(() => { SpawnAndResetBeat(); });
        Menu.OnGameStopped.AddListener(() => { Stop(); });
        _interactable = GetComponentInChildren<Interactable>();
    }

    public void SpawnAndResetBeat()
    {
        SoundHandler.Instance.ResetBeat();
        Spawn();
    }

    public Ball Spawn(GameObject go = null)
    {
        if (go == null)
            go = Instantiate(ballPrefab, GetInteractable().Right, quaternion.identity);


        // go.transform.position = transform.position +
        //                         (go.transform.localScale.x + transform.localScale.x) / 2 * transform.right;

        var ball = go.GetComponent<Ball>();
        ball.spawner = this;

        var speed = speedFactor * Mathf.Clamp(transform.localScale.x - 0.2f, 0, 10);

        go.gameObject.GetComponent<Rigidbody2D>().velocity = speed * GetInteractable().GetScale();
        return ball;
    }

    private void Stop()
    {
        foreach (var ball in GameObject.FindObjectsOfType<Ball>())
        {
            Destroy(ball.gameObject);
        }
    }

    public void OnCreationStart(Transform parent, Vector3 startPosition) =>
        GetInteractable().OnCreationStart(parent, startPosition);

    public void OnCreationFinish() => GetInteractable().OnCreationFinish();

    public void OnCreationUpdate(Vector3 newPosition, Vector3 startPosition) =>
        GetInteractable().OnCreationUpdate(newPosition, startPosition);

    public Transform GetTransform() => transform;

    public Interactable GetInteractable()
    {
        if (_interactable == null)
            _interactable = GetComponentInChildren<Interactable>();
        return _interactable;
    }

    public override BlueprintIndex GetBlueprintIndex()
    {
        return BlueprintIndex.Spawner;
    }
}