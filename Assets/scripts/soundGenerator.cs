using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundGenerator : MonoBehaviour
{
    public Gubernia502.fraction Fraction;
    public GameObject owner;
    protected virtual float SoundLevel { get; set; } = 1f;
    public float soundLevel
    {
        get => SoundLevel;
        set
        {
            if (SoundLevel < value)
            {
                SoundLevel = value;
                enabled = true;
                transform.localScale = new Vector3(SoundLevel, transform.localScale.y, SoundLevel);
            }
        }
    }
    private void Update()
    {
        if (SoundLevel <= Gubernia502.constData.NPCMinSoundVolume)
        {
            SoundLevel = Gubernia502.constData.NPCMinSoundVolume;
            enabled = false;
        }
        else
        {
            SoundLevel -= Gubernia502.constData.soundDegressSpeed;
        }
        transform.localScale = new Vector3(SoundLevel, transform.localScale.y, SoundLevel);
    }
    protected virtual void Start()
    {
        soundLevel = SoundLevel;
    }
    public void disableSoundGen()
    {
        enabled = false;
        SoundLevel = 0;
        transform.localScale = Vector3.zero;
    }
}
