using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] Dialogue dialogue;
    int dialogueLength = 0;
    int currentDialogueIndex = 0;
    int currentResponseIndex = 0;
    [SerializeField] GameObject dialogueCanvas;
    [SerializeField] TextMeshProUGUI dialogueCanvasTitle;
    [SerializeField] Transform dialogueSpawnTransform;
    [SerializeField] Transform dialogueResponseSpawnTransform;
    [SerializeField] GameObject npcDialoguePrefab;
    [SerializeField] GameObject dialogueResponsePrefab;
    [SerializeField] List<PlayerDialogueResponse> playerDialogueResponses = new();
    [SerializeField] PlayerDialogueResponse currentResponseSelected;
    [SerializeField] DialogueNode currentDialogueNode;
    public bool ResponseSpawned = false;
    public bool CanRespond = false;
    bool dialogueGoing = false;
    bool skipped = false;
    string actualText = "";
    string title = "";
    public PauseInfo PauseInfo;
    Coroutine produceTextCoroutine;
    AudioManager audioManager => AudioManager.Instance;
    void Start()
    {
        ActionNotifier.Instance.NpcInteract += InteractedWithNpc;
    }

    void OnDestroy()
    {
        ActionNotifier.Instance.NpcInteract -= InteractedWithNpc;
    }
    private void InteractedWithNpc(Dialogue npcdialogueNode, string npcName)
    {
        title = npcName;
        StartDialogue(npcdialogueNode.RootNode, npcName);
    }
    void Update()
    {
        if (InputManager.Instance.DialogueSelect)
        {
            if (dialogueGoing && !skipped)
            {
                skipped = true;
                dialogueGoing = false;
                MoveNextInDialogue();
                AudioManager.Instance.StopSound();
                return;
            }
            if (playerDialogueResponses.Count <= 0)
            {
                MoveNextInDialogue();
            }
            else if (currentResponseSelected != null && CanRespond)
            {
                if (currentResponseSelected.SelectedResponse().DialogueText.Count == 0) return; // Selected empty response. 
                audioManager.PlaySelectedInteraction();
                ClearOldResponse();
                currentDialogueIndex = 0;
                StartDialogue(currentResponseSelected.SelectedResponse(), title);
            }
        }
        if (InputManager.Instance.DialogueDown)
        {
            if (playerDialogueResponses.Count <= 0) return;
            currentResponseSelected.SetSelected(false);
            currentResponseSelected = Helper.Down<PlayerDialogueResponse>(playerDialogueResponses, currentResponseSelected);
            currentResponseSelected.SetSelected(true);
            audioManager.PlayInteraction();
        }
        if (InputManager.Instance.DialogueUp)
        {
            if (playerDialogueResponses.Count <= 0) return;
            currentResponseSelected.SetSelected(false);
            currentResponseSelected = Helper.Up<PlayerDialogueResponse>(playerDialogueResponses, currentResponseSelected);
            currentResponseSelected.SetSelected(true);
            audioManager.PlayInteraction();
        }
        if (InputManager.Instance.DialogueLeft)
        {
            if (playerDialogueResponses.Count <= 0) return;
            currentResponseSelected.SetSelected(false);
            currentResponseSelected = Helper.Left<PlayerDialogueResponse>(playerDialogueResponses, currentResponseSelected);
            currentResponseSelected.SetSelected(true);
            audioManager.PlayInteraction();
        }
        if (InputManager.Instance.DialogueRight)
        {
            if (playerDialogueResponses.Count <= 0) return;
            currentResponseSelected.SetSelected(false);
            currentResponseSelected = Helper.Right<PlayerDialogueResponse>(playerDialogueResponses, currentResponseSelected);
            currentResponseSelected.SetSelected(true);
            audioManager.PlayInteraction();
        }
    }
    public void StartDialogue(DialogueNode dialogueNode, string title)
    {
        dialogueGoing = true;
        CanRespond = false;
        dialogueCanvas.SetActive(true);
        dialogueCanvasTitle.text = title;
        InputManager.Instance.EnableDialogue();
        ClearOldDialogue();
        currentDialogueNode = dialogueNode;

        dialogueLength = currentDialogueNode.DialogueText.Count;
        NpcDialogue npcDialogue = SpawnNewDialogue().SetText(currentDialogueNode.DialogueText[currentDialogueIndex]);
        int index = 0;
        ReproduceText(currentDialogueNode.DialogueText[currentDialogueIndex], index, currentDialogueNode, npcDialogue.dialogueText);
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
            return;
        }
        if (currentDialogueIndex >= dialogueLength && !currentDialogueNode.IsLastNode() && currentDialogueNode.Responses.Count > 0)
        {
            foreach (var response in currentDialogueNode.Responses)
            {
                playerDialogueResponses.Add(SpawnResponses().SetText(response));
            }
            playerDialogueResponses[currentResponseIndex].SetSelected(true);
            currentResponseSelected = playerDialogueResponses[currentResponseIndex];
            ResponseSpawned = true;
            StartCoroutine(CanSelectResponse());
            currentDialogueNode.dialogueEventSetter?.Invoke();
            skipped = false;
            return;
        }
        ClearOldDialogue();
        NpcDialogue npcDialogue = SpawnNewDialogue().SetText(currentDialogueNode.DialogueText[currentDialogueIndex]);
        int index = 0;
        ReproduceText(currentDialogueNode.DialogueText[currentDialogueIndex], index, currentDialogueNode, npcDialogue.dialogueText);
        currentDialogueIndex += 1;
    }
    IEnumerator CanSelectResponse()
    {
        yield return new WaitForEndOfFrame();
        CanRespond = true;
    }
    NpcDialogue SpawnNewDialogue()
    {
        GameObject _dialogue = Instantiate(npcDialoguePrefab, dialogueSpawnTransform);
        NpcDialogue _npcDialogue = _dialogue.GetComponent<NpcDialogue>();
        return _npcDialogue;
    }
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
        for (int i = 0; i < dialogueSpawnTransform.childCount; i++)
        {
            Destroy(dialogueSpawnTransform.GetChild(i).gameObject);
        }
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
    private void ReproduceText(string response, int index, DialogueNode node, TextMeshProUGUI textBody)
    {
        if (index < response.Length)
        {
            //get one letter
            char letter = response[index];

            //Actualize on screen
            textBody.text = Write(letter);
            AudioManager.Instance.PlaySound();

            //set to go to the Up
            index += 1;
            produceTextCoroutine = StartCoroutine(PauseBetweenChars(letter, response, index, node, textBody));
        }
        else
        {
            AudioManager.Instance.StopSound();
            dialogueGoing = false;
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
