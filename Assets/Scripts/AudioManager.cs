using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There are two AudioManagers in the scene. There should be only one AudioManager.");
        }

        Instance = this;
    }

    static AudioManager Instance { get; set; }
    public GameObject audioPlayerPrefab;

    public static void Play(AudioClip clip)
    {
        GameObject go = GameObject.Instantiate(Instance.audioPlayerPrefab, Instance.transform.position, Quaternion.identity, Instance.transform);

        AudioSource source = go.GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
    }

}