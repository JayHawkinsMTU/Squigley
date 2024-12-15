using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MenuFlicker : MonoBehaviour
{
    [SerializeField] PostProcessVolume volume;
    ChromaticAberration chromatic;
    Bloom bloom;
    float time = 2.25f;
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGetSettings(out chromatic);
        volume.profile.TryGetSettings(out bloom);
        time = 2.25f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        chromatic.intensity.value = .5f + .4f * Mathf.Cos(1f/3f * Mathf.PI * time);
        bloom.intensity.value = Random.Range(2f,3f);
    }
}
