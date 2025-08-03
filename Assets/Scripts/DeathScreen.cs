using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    #region Instance
    public static DeathScreen Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    #endregion
    [SerializeField] GameObject deathScreen;
    public async Task StartDeathScreen()
    {
        await LerpDeathScreenTask(deathScreen, 0, 1, 2);
        //deathScreen.SetActive(true);
        await WaitForSecondsAsync(2);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().TeleportPlayer(new Vector3(0, 1, 5));
        //deathScreen.SetActive(false);
       await LerpDeathScreenTask(deathScreen, 1, 0, 1);
    }
    async Task WaitForSecondsAsync(float delay)
    {
        await Task.Delay(TimeSpan.FromSeconds(delay));
    }
    public static async Task LerpDeathScreenTask(GameObject target, float from, float to, float duration)
    {

        Renderer targetRenderer = target.GetComponent<Renderer>();
        Image _image = targetRenderer.GetComponent<Image>();
        Color startColor = _image.color;


        float elapsed = 0f;

        while (elapsed < duration)
        {
            float lerped = Mathf.Lerp(from, to, elapsed / duration);
            startColor.a = lerped;
            _image.color = startColor;
            elapsed += Time.deltaTime;
            await Task.Yield();
        }

    }
    public IEnumerator LerpDeathScreen(GameObject target, float from, float to, float duration)
    {
        if (target == null) yield break;

        Renderer targetRenderer = target.GetComponent<Renderer>();
        Image _image = targetRenderer.GetComponent<Image>();
        Color startColor = _image.color;
        if (targetRenderer == null) yield break;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float lerped = Mathf.Lerp(from, to, elapsed / duration);
            startColor.a = lerped;
            _image.color = startColor;
            elapsed += Time.deltaTime;
            yield return null;
        }

    }
}
