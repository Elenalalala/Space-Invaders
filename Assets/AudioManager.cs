using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioSource;
    public List<AudioClip> Beep;

    private int stepCounter = 0;

    private float lastPlayTime = 0f;
    private float minInterval = 0.2f;
    // Start is called before the first frame update


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMovementSound()
    {
        if (audioSource == null || Beep.Count == 0) return;

        if (Time.time - lastPlayTime < minInterval) return;

        audioSource.clip = Beep[stepCounter % 4];
        stepCounter++;

        //if(audioSource.pitch > 0.5f) audioSource.pitch -= 0.02f;

        audioSource.Play();
        lastPlayTime = Time.time;
    }
}
