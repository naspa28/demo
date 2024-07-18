using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip introduceSound;
    public AudioClip secondClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlaySequentialAudio());
    }

    IEnumerator PlaySequentialAudio()
    {
        // 첫 번째 오디오 클립 재생
        audioSource.clip = introduceSound;
        audioSource.Play();
        // 첫 번째 클립이 재생될 때까지 대기
        yield return new WaitForSeconds(introduceSound.length);

        // 두 번째 오디오 클립 재생
        audioSource.clip = secondClip;
        audioSource.loop = true; // 두 번째 클립은 루프 재생
        audioSource.Play();
       
    }
    
}
