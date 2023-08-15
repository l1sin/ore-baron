using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private GameObject _soundPrefab;
    [Range(0f, 1f)] public float ClickVolume;
    [Range(0f, 1f)] public float MineVolume;
    public AudioClip ClickSound;
    public AudioClip[] MineSounds;

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void PlayClickSound()
    {
        var newSound = Instantiate(_soundPrefab);
        var audio = newSound.GetComponent<AudioSource>();
        audio.clip = ClickSound;
        audio.volume = ClickVolume;
        audio.Play();
    }

    public void PlayMineSound()
    {
        var newSound = Instantiate(_soundPrefab);
        var audio = newSound.GetComponent<AudioSource>();
        var audioClip = MineSounds[Random.Range(0, MineSounds.Length)];
        audio.clip = audioClip;
        audio.volume = MineVolume;
        audio.Play();
    }
}
