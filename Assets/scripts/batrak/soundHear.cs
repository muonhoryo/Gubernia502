using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundHear : MonoBehaviour
{
    public batrakBehavior batrakBehavior;
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
                StartCoroutine(onHearedSoundDelay(Gubernia502.constData.batrakCheckSoundDelay));
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
                    batrakBehavior.onLostHearedSound(sound.transform.position);
                }
                soundHears.RemoveAt(soundHears.IndexOf(sound));
        }
    }
    IEnumerator onHearedSoundDelay(float delayTime)
    {
        while (soundHears.Count>0)
        {
            batrakBehavior.onHearedSound(soundHears[0].transform.position);
            yield return new WaitForSeconds(delayTime);
        }
        enabled = false;
        yield break;
    }
}
