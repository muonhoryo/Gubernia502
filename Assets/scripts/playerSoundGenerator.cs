using UnityEngine;
using UnityEngine.UI;

public class playerSoundGenerator : soundGenerator//singltone
{
    static playerSoundGenerator singltone = null;
    [SerializeField]
    private Image soundIndicator;
    protected override float SoundLevel
    {
        get => base.SoundLevel;
        set
        {
            if (value < Gubernia502.constData.soundIndicMin)
            {
                soundIndicator.material.SetFloat("fullness", 0);
            }
            else if (value > Gubernia502.constData.soundIndicMax)
            {
                soundIndicator.material.SetFloat("fullness", 1.01f);
            }
            else
            {
                soundIndicator.material.SetFloat("fullness", 
                    (((float)(value - Gubernia502.constData.soundIndicMin)) / Gubernia502.constData.soundIndicMax) + 0.01f);
            }
            base.SoundLevel = value;
        }
    }
    protected override void Start()
    {
        base.Start();
        SoundLevel = SoundLevel;
    }
    private void Awake()
    {
        if (singltone == null)
        {
            singltone = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
