using Data.Audio;
using ScriptableObject.Audio;
using UnityEngine;
using Util.Audio;

[RequireComponent(typeof(AudioSource))]
public class CanvasAudioController : MonoBehaviour
{
    private AudioSource _backgroundAudioSource;
    private AudioSource _uiAudioSource;

    public AudioDataScriptableObject BackgroundAudioData;
    public AudioDataScriptableObject[] AudioData = new AudioDataScriptableObject[4];

    public enum CanvasAudioSoundType 
    {
        Start,
        Tick,
        Select,
        Back,
    }


    void Awake()
    {
        var audioSources = GetComponents<AudioSource>();

        _backgroundAudioSource = audioSources[0];
        _uiAudioSource = audioSources[1];
    }

    void Start()
    {
        if (BackgroundAudioData != null)
        {
            Debug.Log("backgroundaduiodata");
            _backgroundAudioSource.Initialize(BackgroundAudioData.AudioData);
            _backgroundAudioSource.Play();
        }
    }

    public void Play(AudioClip audioClip)
    {
        if (audioClip != null)
            _uiAudioSource.PlayOneShot(audioClip);
    }

    public void Play(AudioData audioData) => Play(audioData?.AudioClip);

    public void Play(CanvasAudioSoundType audioSoundType) => Play(AudioData[(int) audioSoundType]?.AudioData);

    public void FadeOutBackgroundMusic() => StartCoroutine(AudioHelper.FadeOut(_backgroundAudioSource, 1f));
}
