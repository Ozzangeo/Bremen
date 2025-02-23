using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance {
        get {
            if (!IsInstanceAllocated
                && _instance == null) {
                _instance = GameObject.FindAnyObjectByType<AudioManager>();

                if (_instance == null) {
                    var instance = new GameObject("Audio Manager");

                    _instance = instance.AddComponent<AudioManager>();
                }

                IsInstanceAllocated = true;

                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    private static AudioManager _instance = null;
    public static bool IsInstanceAllocated { get; private set; } = false;   // MonoBehaviour Fake Null Blocking Variable From Unity

    public static AudioSource AudioSource => Instance._mainAudioSource;
    public static AudioClip Clip {
        get => AudioSource.clip;
        set => AudioSource.clip = value;
    }
    public static float Time {
        get => AudioSource.time;
        set => AudioSource.time = value;
    }
    public static float Volume {
        get => AudioSource.volume;
        set => AudioSource.volume = value;
    }
    public static float Pitch {
        get => AudioSource.pitch;
        set => AudioSource.pitch = value;
    }

    [Header("Requires")]
    [SerializeField] private AudioSource _mainAudioSource;

    private void Awake() {
        _mainAudioSource = gameObject.AddComponent<AudioSource>();

        if (IsInstanceAllocated) {
            gameObject.SetActive(false);
        }
    }

    public static void Play(float? time = null, float? volume = null, float? pitch = null) {
        AudioSource.Play();

        if (time is float t) {
            Time = t;
        }

        if (volume is float v) {
            Volume = v;
        }

        if (pitch is float p) {
            Pitch = p;
        }
    }
    public static void Play(AudioClip clip, float? time = null, float? volume = null, float? pitch = null) {
        Clip = clip;

        Play(time, volume, pitch);
    }

    public static void Stop() => AudioSource.Play();
    public static void Pause() => AudioSource.Pause();
    public static void UnPause() => AudioSource.UnPause();

    public static void PlayOneShot(AudioClip clip) => AudioSource.PlayOneShot(clip);
    public static void PlayClipAtPoint(AudioClip clip, Vector3 position) => AudioSource.PlayClipAtPoint(clip, position);
}