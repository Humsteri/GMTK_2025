using System.Collections;
using UnityEngine;

public class NpcTalks : MonoBehaviour
{
    Material mat;
    [SerializeField] GameObject npc;
    [SerializeField] Texture2D idleTexture;
    [SerializeField] Texture2D talkingTexture;
    [SerializeField] Texture2D blinkTexture;
    [SerializeField] float waitTime = 0.2f;
    [SerializeField] float minWaitTime;
    [SerializeField] float maxWaitTime;
    bool isTalking;
    bool idle = true;
    bool canBlink = true;
    Coroutine blinkRoutine;
    void Start()
    {
        mat = npc.GetComponent<MeshRenderer>().material; //.GetTexture("_Texture2D"));
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
        Texture2D newTexture = isTalking ? talkingTexture : idleTexture;
        mat.SetTexture("_Texture2D", newTexture);
    }
    IEnumerator BlinkCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        Texture2D newTexture = blinkTexture;
        mat.SetTexture("_Texture2D", newTexture);
        yield return new WaitForSeconds(0.1f);
        newTexture = idleTexture;
        mat.SetTexture("_Texture2D", newTexture);
        canBlink = true;
        StopAllCoroutines();
    }
    public void StopTalk()
    {
        mat.SetTexture("_Texture2D", idleTexture);
        idle = true;
        canBlink = true;
    }
}
