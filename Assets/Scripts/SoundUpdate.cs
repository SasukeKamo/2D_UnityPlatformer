using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUpdate : MonoBehaviour
{
    public AudioSource musicAudio;
    public Slider musicSlider;

    private void Start()
    {
        musicSlider.value = 0.5f;
    }

    public void UpdateSound()
    {
        musicAudio.volume = musicSlider.value;
    }
}
