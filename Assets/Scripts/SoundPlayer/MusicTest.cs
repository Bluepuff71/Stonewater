using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MusicWait());
    }

    IEnumerator MusicWait()
    {
        GetComponent<SoundPlayer>().Play();
        yield return new WaitForSeconds(5f);
        GetComponent<SoundPlayer>().Stop();
        yield return new WaitForSeconds(5f);
        GetComponent<SoundPlayer>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
