using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource fxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip clickFX;
    [SerializeField] private AudioClip mainTheme;
    // Start is called before the first frame update
    void Start()
    {
        fxSource = GetComponent<AudioSource>();
        fxSource.clip = clickFX;

        musicSource.clip = mainTheme;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            fxSource.Play();
        }
    }

    public void PlayMain()
    {
        musicSource.Play();
    }
}
