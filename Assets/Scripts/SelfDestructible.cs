using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructible : MonoBehaviour
{
    [SerializeField] private uint _destroyAfterBeats;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SelfDestroy());
    }

    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds((_destroyAfterBeats-1) * SoundHandler.Instance.BeatTimeDeltaSeconds);

        float t = 0;
        while (t < SoundHandler.Instance.BeatTimeDeltaSeconds)
        {
            t += Time.deltaTime;
            
            var color = _spriteRenderer.color;
            color.a = 1 - t / SoundHandler.Instance.BeatTimeDeltaSeconds;
            _spriteRenderer.color = color;
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
