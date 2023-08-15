using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private bool _dontDestroyOnLoad = true;

    private void Start()
    {
        if (_dontDestroyOnLoad)
        {
            DontDestroyOnLoad(this);
        }
    }

    private void Update()
    {
        if (!_audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
