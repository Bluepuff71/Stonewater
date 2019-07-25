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
        GetComponent<SoundPlayer>().PlayAsync();
        yield return new WaitForSeconds(5f);
        GetComponent<SoundPlayer>().StopAsync();
        yield return new WaitForSeconds(5f);
        GetComponent<SoundPlayer>().PlayAsync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
