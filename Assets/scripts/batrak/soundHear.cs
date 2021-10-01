using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundHear : MonoBehaviour
{
    public mobBehavior mobBehavior;
    public List<soundGenerator> soundHears = new List<soundGenerator> { };
    private void OnTriggerEnter(Collider other)
    {
        soundGenerator sound;
        if ( other.TryGetComponent(out sound)&&!soundHears.Contains(sound))
        {
            soundHears.Add(sound);
            if (!enabled&& soundHears.Count >= 1)
            {
                enabled = true;
                StartCoroutine(onHearedSoundDelay(Gubernia502.constData.mobCheckSoundDelay));
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        soundGenerator sound;
        if( other.TryGetComponent(out sound)&& soundHears.Contains(sound))
        {
                if (!enabled && soundHears.Count >= 1)
                {
                    enabled = false;
                StopAllCoroutines();
                    mobBehavior.currentState.onLostHearedSound(mobBehavior,sound.transform.position);
                }
                soundHears.RemoveAt(soundHears.IndexOf(sound));
        }
    }
    IEnumerator onHearedSoundDelay(float delayTime)
    {
        while (soundHears.Count>0)
        {
            mobBehavior.currentState.onHearedSound(mobBehavior,soundHears[0].transform.position);
            yield return new WaitForSeconds(delayTime);
        }
        enabled = false;
        yield break;
    }
}
