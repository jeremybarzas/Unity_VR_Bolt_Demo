using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class AudioOffset : MonoBehaviour
{
    private AudioSource m_audioSource;
    public List<AudioClip> m_audioClips;
    public List<UnityEvent> m_afterClipActions;
    public float m_endOfClipDelay;
    public float m_endActionDelay;
    public UnityEvent m_onEnd;

    private bool m_isPlaying;

    private int m_index = -1;

    void Start(){
        if(m_afterClipActions.Count == 0){
            foreach(AudioClip ac in m_audioClips){
                m_afterClipActions.Add(new UnityEvent());
            }
        }

        m_audioSource = gameObject.GetComponent<AudioSource>();
        if(m_audioClips.Count == 0)
        {
            m_audioSource.clip = null;
            return;
        }

        m_audioSource.clip = m_audioClips[0];
    }

    public void ReplaceClip(AudioClip ac){
        m_audioSource.clip = ac;
    }

    public void StartAudio()
    {
        if(m_isPlaying){
            StopAllCoroutines();
            m_isPlaying = false;
        }
        StartCoroutine(PlayVoiceOversOneAtATime());
    }

    public IEnumerator PlayVoiceOversOneAtATime(){
        m_isPlaying = true;
        
        foreach(AudioClip ac in m_audioClips){
            m_audioSource.clip = ac;
            m_audioSource.Stop();
            m_audioSource.Play();

            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => m_audioSource.isPlaying == false);
            yield return new WaitForSeconds(m_endOfClipDelay);
            
            if(m_afterClipActions[m_audioClips.IndexOf(ac)] != null){
                m_afterClipActions[m_audioClips.IndexOf(ac)].Invoke();
            }
            
        }

        m_isPlaying = false;
        yield return new WaitForSeconds(m_endActionDelay);
        m_onEnd.Invoke();
    }

    private void OnDisable() {
        StopCoroutine(PlayVoiceOversOneAtATime());
        m_audioSource.Stop();
    }

    private void OnEnable() {
        StopCoroutine(PlayVoiceOversOneAtATime());
        m_audioSource.Stop();
    }

    public void StopVoiceOver(){
        if(m_isPlaying){
                StopAllCoroutines();
                m_isPlaying = false;
        }

        m_audioSource.Stop();
    }
}
