using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _sonidosP;

    private AudioSource audioControlP;

    #region 
    public void EligeAudioP(int ind, float vol)
    {
        audioControlP.PlayOneShot(_sonidosP[ind], vol);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        audioControlP = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
