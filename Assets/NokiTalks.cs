using System.Collections;
using UnityEngine;

public class NokiTalks : MonoBehaviour
{
    Material mat;
    [SerializeField] GameObject noki;
    [SerializeField] Texture2D idleNoki;
    [SerializeField] Texture2D talkingNoki;
    [SerializeField] Texture2D blinkNoki;
    [SerializeField] float waitTime = 0.2f;
    [SerializeField] float minWaitTime;
    [SerializeField] float maxWaitTime;
    bool isTalking;
    bool idle = true;
    bool canBlink = true;
    Coroutine blinkRoutine;
    void Start()
    {
        mat = noki.GetComponent<MeshRenderer>().material; //.GetTexture("_Texture2D"));
    }
    public void Talk()
    {
        StopAllCoroutines();
        StartCoroutine(TalkCoroutine());
    }
    void Update()
    {
        if (idle && canBlink)
        {
            StartCoroutine(BlinkCoroutine(UnityEngine.Random.Range(minWaitTime, maxWaitTime)));
            canBlink = false;
        }
    }
    IEnumerator TalkCoroutine()
    {
        yield return new WaitForSeconds(waitTime);
        idle = false;
        isTalking = !isTalking;
        Texture2D newTexture = isTalking ? talkingNoki : idleNoki;
        mat.SetTexture("_Texture2D", newTexture);
    }
    IEnumerator BlinkCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        Texture2D newTexture = blinkNoki;
        mat.SetTexture("_Texture2D", newTexture);
        yield return new WaitForSeconds(0.1f);
        newTexture = idleNoki;
        mat.SetTexture("_Texture2D", newTexture);
        canBlink = true;
        StopAllCoroutines();
    }
    public void StopTalk()
    {
        mat.SetTexture("_Texture2D", idleNoki);
        idle = true;
        canBlink = true;
    }
}
