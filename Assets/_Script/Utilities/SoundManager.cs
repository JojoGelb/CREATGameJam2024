using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioMixerGroup _musicMixerGroup;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;


    /*public void PlayMusicAtPosition(Vector3 position, AudioClip clip)
    {
        GameObject sound = ObjectPooler.Instance.GetOrCreateGameObjectFromPool(ObjectPooler.PoolObject.Sound);
        sound.transform.position = position;

        AudioSource audio = sound.GetComponent<AudioSource>();
        audio.outputAudioMixerGroup = _musicMixerGroup;
        audio.clip = clip;
        audio.loop = true;
        audio.Play();
    }*/

    public void PlayVFXAtPosition(Vector3 position, AudioClip clip, float volume = 1f)
    {
        GameObject sound = ObjectPooler.Instance.GetOrCreateGameObjectFromPool(ObjectPooler.PoolObject.Sound);
        sound.transform.position = position;

        AudioSource audio = sound.GetComponent<AudioSource>();
        audio.outputAudioMixerGroup = _sfxMixerGroup;
        audio.clip = clip;
        audio.volume = volume;
        audio.Play();
        StartCoroutine(DisableAudioSource(audio, clip.length));
    }

    IEnumerator DisableAudioSource(AudioSource audioSource, float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        audioSource.gameObject.SetActive(false);
    }
}
