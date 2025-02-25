using Ozi.Extension;
using UnityEngine;

[System.Serializable]
public class ManagedAudioSource {
    [field: Header("Settings")]
    [field: SerializeField] public float GlobalVolume { get; set; } = 1.0f;
    [field: SerializeField] public float GlobalPitch { get; set; } = 1.0f;
    [field: SerializeField] public bool IsRevolvable { get; set; } = false;

    [field: Header("Debugs")]
    [field: SerializeField] public int Index { get; private set; } = 0;
    [field: SerializeField] public int MaxCount { get; private set; }
    [field: SerializeField] public AudioSource[] AudioSources { get; private set; }

    public int NextIndex => AudioSources.NextIndex(Index);

    public ManagedAudioSource(int max_count, bool is_revolvable = false, Transform parent = null, string header = null) {
        MaxCount = max_count;
        IsRevolvable = is_revolvable;

        AudioSources = new AudioSource[max_count];

        if (header is null) {
            header = $"Audio Source";
        } else {
            header = $"[{header}] Audio Source";
        }

            for (int i = 0; i < max_count; i++) {
                var audio_game_object = new GameObject($"{header} {i}");
                audio_game_object.transform.SetParent(parent);

                AudioSources[i] = audio_game_object.AddComponent<AudioSource>();
            }
    }

    public AudioSource At(int index) => AudioSources[index];

    #region Properties
    public AudioClip Clip {
        get => GetClip(Index);
        set => SetClip(Index, value);
    }
    public float Time {
        get => GetTime(Index);
        set => SetTime(Index, value);
    }
    public float Volume {
        get => GetVolume(Index);
        set => SetVolume(Index, value);
    }
    public float Pitch {
        get => GetPitch(Index);
        set => SetPitch(Index, value);
    }
    public bool Loop {
        get => GetLoop(Index);
        set => SetLoop(Index, value);
    }
    #endregion
    #region GetProperties
    public AudioClip GetClip(int index) => AudioSources[index].clip;
    public float GetTime(int index) => AudioSources[index].time;
    public float GetVolume(int index) => AudioSources[index].volume;
    public float GetPitch(int index) => AudioSources[index].pitch;
    public bool GetLoop(int index) => AudioSources[index].loop;
    #endregion
    #region SetProperties
    public void SetClip(int index, AudioClip clip) => AudioSources[index].clip = clip;
    public float SetTime(int index, float time) => AudioSources[index].time = time;
    public float SetVolume(int index, float volume) => AudioSources[index].volume = volume;
    public float SetPitch(int index, float pitch) => AudioSources[index].pitch = pitch;
    public bool SetLoop(int index, bool is_loop) => AudioSources[index].loop = is_loop;
    #endregion
    #region ControlFunctions
    public void Play(float volume = 1.0f, float pitch = 1.0f, bool? is_loop = null, AudioClip clip = null) => Play(Index, volume, pitch, is_loop, clip);
    public void Play(int index, float volume = 1.0f, float pitch = 1.0f, bool? is_loop = null, AudioClip clip = null) {
        volume *= GlobalVolume;
        pitch *= GlobalPitch;

        var audio_source = AudioSources[index];
        audio_source.volume = volume;
        audio_source.pitch = pitch;

        if (clip != null) {
            audio_source.clip = clip;
        }

        if (is_loop is bool loop) {
            audio_source.loop = loop;
        }

        audio_source.Play();
    }

    public void Stop() => Stop(Index);
    public void Stop(int index) {
        var audio_source = AudioSources[index];

        audio_source.Stop();
    }

    public void Pause() => Pause(Index);
    public void Pause(int index) {
        var audio_source = AudioSources[index];

        audio_source.Pause();
    }

    public void UnPause() => UnPause(Index);
    public void UnPause(int index) {
        var audio_source = AudioSources[index];

        audio_source.UnPause();
    }
    #endregion

    public void TryRevolveIndex() {
        if (IsRevolvable) {
            Index = NextIndex;
        }
    }

    public AudioSource this[int index] => AudioSources[index];
}