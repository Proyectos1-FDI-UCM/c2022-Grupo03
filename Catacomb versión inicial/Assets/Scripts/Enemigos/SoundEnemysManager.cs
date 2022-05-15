using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEnemysManager : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] _sonidosE;

    private AudioSource audioControlE;

    #region 
    public void EligeAudioE(int ind, float vol)
    {
        audioControlE.PlayOneShot(_sonidosE[ind], vol);
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        audioControlE = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
