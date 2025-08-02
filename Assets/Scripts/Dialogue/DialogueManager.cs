using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    #region Instance
    public static DialogueManager Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    #endregion
    [SerializeField] Dialogue playerAttackedAndHasWeapon;
    [SerializeField] Dialogue playerAttackedAndDoesntHaveWeapon;
    int dialogueLength = 0;
    int currentDialogueIndex = 0;
    int currentResponseIndex = 0;
    [SerializeField] GameObject dialogueCanvas;
    [SerializeField] TextMeshProUGUI dialogueCanvasTitle;
    [SerializeField] Transform dialogueSpawnTransform;
    [SerializeField] Transform dialogueResponseSpawnTransform;
    //[SerializeField] GameObject npcDialoguePrefab;
    [SerializeField] TextMeshProUGUI npcDialogueText;
    [SerializeField] GameObject dialogueResponsePrefab;
    [SerializeField] List<PlayerDialogueResponse> playerDialogueResponses = new();
    [SerializeField] PlayerDialogueResponse currentResponseSelected;
    [SerializeField] DialogueNode currentDialogueNode;
    public bool ResponseSpawned = false;
    public bool CanRespond = false;
    [HideInInspector] public bool DialogueGoing = false;
    bool skipped = false;
    string actualText = "";
    string title = "";
    public PauseInfo PauseInfo;
    Coroutine produceTextCoroutine;
    AudioManager audioManager => AudioManager.Instance;
    NPC currentNpc;
    void Start()
    {
        ActionNotifier.Instance.NpcInteract += InteractedWithNpc;
    }

    void OnDestroy()
    {
        ActionNotifier.Instance.NpcInteract -= InteractedWithNpc;
    }
    private void InteractedWithNpc(Dialogue npcdialogueNode, NPC npc,string npcName)
    {
        title = npcName;
        currentNpc = npc;
        ActionNotifier.Instance.DialogueEnable?.Invoke(true);
        StartDialogue(npcdialogueNode.RootNode, npcName);
    }
    void Update()
    {
        if (InputManager.Instance.DialogueSelect)
        {
            if (DialogueGoing && !skipped && currentDialogueIndex != currentDialogueNode.DialogueText.Count)
            {
                skipped = true;
                DialogueGoing = false;
                MoveNextInDialogue();
                return;
            }
            if (playerDialogueResponses.Count <= 0)
            {
                MoveNextInDialogue();
            }
            else if (currentResponseSelected != null && CanRespond)
            {
                SpecialEncounter();
                if (currentResponseSelected.SelectedResponse().DialogueText.Count == 0) return; // Selected empty response. 
                audioManager.PlaySelectedInteraction();
                ClearOldResponse();
                currentDialogueIndex = 0;   
                StartDialogue(currentResponseSelected.SelectedResponse(), title);
            }
        }
        if (InputManager.Instance.DialogueDown)
        {
            ChangeResponse(Helper.Down);
        }
        if (InputManager.Instance.DialogueUp)
        {
            ChangeResponse(Helper.Up);
        }
        if (InputManager.Instance.DialogueLeft)
        {
            ChangeResponse(Helper.Left);
        }
        if (InputManager.Instance.DialogueRight)
        {
            ChangeResponse(Helper.Right);
        }
    }
    public delegate T NavigationFunction<T>(IList<T> list, T item);
    void ChangeResponse(NavigationFunction<PlayerDialogueResponse> navigationFunction)
    {
        if (playerDialogueResponses.Count <= 0) return;
        currentResponseSelected.SetSelected(false);
        currentResponseSelected = navigationFunction(playerDialogueResponses, currentResponseSelected);
        currentResponseSelected.SetSelected(true);
        audioManager.PlayInteraction();
    }
    public void StartDialogue(DialogueNode dialogueNode, string title)
    {
        DialogueGoing = true;
        CanRespond = false;
        dialogueCanvas.SetActive(true);
        dialogueCanvasTitle.text = title;
        InputManager.Instance.EnableDialogue();
        ClearOldDialogue();
        currentDialogueNode = dialogueNode;

        dialogueLength = currentDialogueNode.DialogueText.Count;
        npcDialogueText.text = currentDialogueNode.DialogueText[currentDialogueIndex];
        int index = 0;
        ReproduceText(currentDialogueNode.DialogueText[currentDialogueIndex], index, currentDialogueNode, npcDialogueText);
        currentDialogueIndex += 1;
    }
    public void MoveNextInDialogue()
    {
        if (ResponseSpawned) return;
        if (currentDialogueNode.IsLastNode() && currentDialogueIndex >= dialogueLength) // Was last dialogue and there are no responses
        {
            skipped = false;
            ClearOldDialogue();
            ClearOldResponse();
            currentDialogueNode.dialogueEventSetter?.Invoke();
            currentDialogueNode = null;
            InputManager.Instance.DisableDialogue();
            dialogueCanvas.SetActive(false);
            currentDialogueIndex = 0;
            ActionNotifier.Instance.DialogueEnable?.Invoke(false);
            return;
        }
        if (currentDialogueIndex >= dialogueLength && !currentDialogueNode.IsLastNode() && currentDialogueNode.Responses.Count > 0)
        {
            skipped = false;
            foreach (var response in currentDialogueNode.Responses)
            {
                playerDialogueResponses.Add(SpawnResponses().SetText(response));
            }
            playerDialogueResponses[currentResponseIndex].SetSelected(true);
            currentResponseSelected = playerDialogueResponses[currentResponseIndex];
            ResponseSpawned = true;
            StartCoroutine(CanSelectResponse());
            currentDialogueNode.dialogueEventSetter?.Invoke();
            return;
        }
        skipped = false;
        DialogueGoing = true;
        ClearOldDialogue();
        //NpcDialogue npcDialogue = SpawnNewDialogue().SetText(currentDialogueNode.DialogueText[currentDialogueIndex]);
        npcDialogueText.text = currentDialogueNode.DialogueText[currentDialogueIndex];
        int index = 0;
        ReproduceText(currentDialogueNode.DialogueText[currentDialogueIndex], index, currentDialogueNode, npcDialogueText);
        currentDialogueIndex += 1;
    }
    void SpecialEncounter()
    {
        if (currentResponseSelected.GetResponseText() == "Attack" && currentResponseSelected.SelectedResponse().DialogueText.Count == 0)
        {
            audioManager.PlaySelectedInteraction();
            ClearOldResponse();
            currentDialogueIndex = 0;
            if (Items.Instance.HasWeapon)
            {
                StartDialogue(playerAttackedAndHasWeapon.RootNode, title);
            }
            else
            {
                StartDialogue(playerAttackedAndDoesntHaveWeapon.RootNode, title);
            }
        }
    }
    IEnumerator CanSelectResponse()
    {
        yield return new WaitForEndOfFrame();
        CanRespond = true;
    }
    /* NpcDialogue SpawnNewDialogue()
    {
        GameObject _dialogue = Instantiate(npcDialoguePrefab, dialogueSpawnTransform);
        NpcDialogue _npcDialogue = _dialogue.GetComponent<NpcDialogue>();
        return _npcDialogue;
    } */
    PlayerDialogueResponse SpawnResponses()
    {
        GameObject _dialogue = Instantiate(dialogueResponsePrefab, dialogueResponseSpawnTransform);
        PlayerDialogueResponse _response = _dialogue.GetComponent<PlayerDialogueResponse>();
        return _response;
    }
    public void ClearOldDialogue()
    {
        if (produceTextCoroutine != null)
        {
            StopCoroutine(produceTextCoroutine);
            produceTextCoroutine = null;
        }
        actualText = "";
        npcDialogueText.text = actualText;
        /* for (int i = 0; i < dialogueSpawnTransform.childCount; i++)
        {
            Destroy(dialogueSpawnTransform.GetChild(i).gameObject);
        } */
    }
    public void ClearOldResponse()
    {
        for (int i = 0; i < dialogueResponseSpawnTransform.childCount; i++)
        {
            Destroy(dialogueResponseSpawnTransform.GetChild(i).gameObject);
        }
        playerDialogueResponses.Clear();
        currentResponseIndex = 0;
        ResponseSpawned = false;
    }
    #region Text letter by letter
    private string Write(char letter)
    {
        actualText += letter;
        return actualText;
    }
    public void PlayTextSound()
    {
        StartCoroutine(TextSoundWaiter());
    }
    IEnumerator TextSoundWaiter()
    {
        yield return new WaitForSeconds(PauseInfo.textPause);
        AudioManager.Instance.PlaySound();
    }
    private void ReproduceText(string response, int index, DialogueNode node, TextMeshProUGUI textBody)
    {

        if (index < response.Length)
        {
            //get one letter
            char letter = response[index];

            //Actualize on screen
            textBody.text = Write(letter);
            PlayTextSound();
            NpcTalks _npcTalks = currentNpc.GetComponent<NpcTalks>();
            if (_npcTalks != null)
            {
                _npcTalks.Talk();
            }
            //set to go to the Up
            index += 1;
            produceTextCoroutine = StartCoroutine(PauseBetweenChars(letter, response, index, node, textBody));
        }
        else
        {
            AudioManager.Instance.StopSound();
            NpcTalks _npcTalks = currentNpc.GetComponent<NpcTalks>();
            if (_npcTalks != null)
            {
                _npcTalks.StopTalk();
            }
            DialogueGoing = false;
            if (playerDialogueResponses.Count > 0) return;

            if (currentDialogueIndex >= dialogueLength && !currentDialogueNode.IsLastNode() && currentDialogueNode.Responses.Count > 0)
            {
                foreach (var responseNode in currentDialogueNode.Responses)
                {
                    playerDialogueResponses.Add(SpawnResponses().SetText(responseNode));
                }
                playerDialogueResponses[currentResponseIndex].SetSelected(true);
                currentResponseSelected = playerDialogueResponses[currentResponseIndex];
                ResponseSpawned = true;
                StartCoroutine(CanSelectResponse());
                return;
            }
            //SpawnResponseButtons(title, node);
        }
    }
    private IEnumerator PauseBetweenChars(char letter, string response, int index, DialogueNode node, TextMeshProUGUI textBody)
    {
        switch (letter)
        {
            case '.':
                yield return new WaitForSeconds(PauseInfo.dotPause);
                ReproduceText(response, index, node, textBody);
                yield break;
            case ',':
                yield return new WaitForSeconds(PauseInfo.commaPause);
                ReproduceText(response, index, node, textBody);
                yield break;
            case ' ':
                yield return new WaitForSeconds(PauseInfo.spacePause);
                ReproduceText(response, index, node, textBody);
                yield break;
            default:
                yield return new WaitForSeconds(PauseInfo.normalPause);
                ReproduceText(response, index, node, textBody);
                yield break;
        }
    }
    #endregion
    #region Dialogue events
    public void TestDialogueEvent()
    {
        print("Got to event");
    }
    #endregion
}
[Serializable]
public class PauseInfo
{
    public float textPause;
    public float dotPause;
    public float commaPause;
    public float spacePause;
    public float normalPause;
}
public static class Helper
{
    public static T Up<T>(this IList<T> list, T item)
    {
        int index = list.IndexOf(item);
        int newIndex = index - 2;
        return newIndex >= 0 ? list[newIndex] : item; // stay in place if out of bounds
    }
    public static T Down<T>(this IList<T> list, T item)
    {
        int index = list.IndexOf(item);
        int newIndex = index + 2;
        return newIndex < list.Count ? list[newIndex] : item; // stay in place if out of bounds
    }
    public static T Left<T>(this IList<T> list, T item)
    {
        int index = list.IndexOf(item);
        if (index % 2 == 1) // Only allow left if not first in row
            return list[index - 1];
        return item; // stay in place
    }
    public static T Right<T>(this IList<T> list, T item)
    {
        int index = list.IndexOf(item);
        if (index % 2 == 0 && index + 1 < list.Count) // Only allow right if not last in row
            return list[index + 1];
        return item; // stay in place
    }
}
