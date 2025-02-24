using System.Collections.Generic;
using UnityEngine;

public enum PlayType {
    BGM,
    SFX,
    UI,
}

public class AudioManager : MonoBehaviour {
    public const int MAX_COUNT_OF_ID_IN_BGM = 2;
    public const int MAX_COUNT_OF_ID_IN_SFX = 8;
    public const int MAX_COUNT_OF_ID_IN_UI = 8;

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

    public Dictionary<PlayType, ManagedAudioSource> AudioSourceByType { get; private set; } = new();

    public static float GlobalVolume { get; set; } = 1.0f;

    private void Awake() {
        if (IsInstanceAllocated) {
            Destroy(gameObject);

            return;
        }

        AudioSourceByType[PlayType.BGM] = new ManagedAudioSource(MAX_COUNT_OF_ID_IN_BGM, false, transform, "BGM");
        AudioSourceByType[PlayType.SFX] = new ManagedAudioSource(MAX_COUNT_OF_ID_IN_SFX, true , transform, "SFX");
        AudioSourceByType[PlayType.UI]  = new ManagedAudioSource(MAX_COUNT_OF_ID_IN_UI , true , transform, "UI" );
    }

    public static ManagedAudioSource GetAudioSource(PlayType play_type) => Instance.AudioSourceByType[play_type];

    #region SetProperties
    public static void SetGlobalVolume(PlayType play_type, float global_volume) => GetAudioSource(play_type).GlobalVolume = global_volume;
    public static void SetGlobalPitch(PlayType play_type, float global_pitch) => GetAudioSource(play_type).GlobalPitch = global_pitch;
    #endregion
    #region GetProperties
    public static float GetGlobalVolume(PlayType play_type) => GetAudioSource(play_type).GlobalVolume;
    public static float GetGlobalPitch(PlayType play_type) => GetAudioSource(play_type).GlobalPitch;
    #endregion
    #region ControlFunctions
    public static int Play(PlayType play_type, float volume = 1.0f, float pitch = 1.0f, bool? is_loop = null, AudioClip clip = null) {
        var audio_source = GetAudioSource(play_type);

        var id = audio_source.Index;

        audio_source.Play(volume * GlobalVolume, pitch, is_loop, clip);
        audio_source.TryRevolveIndex();

        return id;
    }
    public static void Play(PlayType play_type, int id, float volume = 1.0f, float pitch = 1.0f, bool? is_loop = null, AudioClip clip = null) {
        var audio_source = GetAudioSource(play_type);

        audio_source.Play(id, volume * GlobalVolume, pitch, is_loop, clip);
    }

    public static void Stop(PlayType play_type, int id) => GetAudioSource(play_type).Stop(id);
    public static void Pause(PlayType play_type, int id) => GetAudioSource(play_type).Pause(id);
    public static void UnPause(PlayType play_type, int id) => GetAudioSource(play_type).UnPause(id);
    #endregion
}