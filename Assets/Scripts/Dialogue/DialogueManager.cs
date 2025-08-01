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
    [SerializeField] Transform dialogueSpawnTransform;
    [SerializeField] Transform dialogueResponseSpawnTransform;
    [SerializeField] GameObject npcDialoguePrefab;
    [SerializeField] GameObject dialogueResponsePrefab;
    [SerializeField] List<PlayerDialogueResponse> playerDialogueResponses = new();
    [SerializeField] PlayerDialogueResponse currentResponseSelected;
    [SerializeField] DialogueNode currentDialogueNode;
    public bool ResponseSpawned = false;
    public bool CanRespond = false;
    string actualText = "";
    public PauseInfo PauseInfo;
    Coroutine produceTextCoroutine;
    void Start()
    {

    }
    void Update()
    {
        if (InputManager.Instance.DialogueSelect)
        {
            if (playerDialogueResponses.Count <= 0)
            {
                MoveNextInDialogue();
            }
            else if (currentResponseSelected != null && CanRespond)
            {
                ClearOldResponse();
                currentDialogueIndex = 0;
                StartDialogue(currentResponseSelected.SelectedResponse());
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartDialogue(dialogue.RootNode);
        }
        if (InputManager.Instance.DialogueDown)
        {
            if (playerDialogueResponses.Count <= 0) return;
            currentResponseSelected.SetSelected(false);
            currentResponseSelected = Helper.Down<PlayerDialogueResponse>(playerDialogueResponses, currentResponseSelected);
            currentResponseSelected.SetSelected(true);
        }
        if (InputManager.Instance.DialogueUp)
        {
            if (playerDialogueResponses.Count <= 0) return;
            currentResponseSelected.SetSelected(false);
            currentResponseSelected = Helper.Next<PlayerDialogueResponse>(playerDialogueResponses, currentResponseSelected);
            currentResponseSelected.SetSelected(true);
        }
    }
    public void StartDialogue(DialogueNode dialogueNode)
    {
        CanRespond = false;
        dialogueCanvas.SetActive(true);
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
        if (currentDialogueNode.IsLastNode() && currentDialogueIndex >= dialogueLength)
        {
            ClearOldDialogue();
            ClearOldResponse();
            currentDialogueNode = null;
            InputManager.Instance.DisableDialogue();
            dialogueCanvas.SetActive(false);
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
    private string Write(char letter)
    {
        actualText += letter;
        return actualText;
    }
    private void ReproduceText(string response, int index, DialogueNode node, TextMeshProUGUI textBody)
    {
        //if (skipped == true) return;
        //if not readied all letters
        if (index < response.Length)
        {
            //get one letter
            char letter = response[index];

            //Actualize on screen
            textBody.text = Write(letter);
            AudioManager.Instance.PlaySound();
            //set to go to the next
            index += 1;
            produceTextCoroutine = StartCoroutine(PauseBetweenChars(letter, response, index, node, textBody));
        }
        else
        {
            AudioManager.Instance.StopSound();
            //DialogueGoing = false;
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
    public static T Next<T>(this IList<T> list, T item)
    {
        var nextIndex = list.IndexOf(item) + 1;
        if (list.Count == 2 && nextIndex == 1)
        {
            //Debug.Log("Only two left in list");
            return list[1];
        }
        if (nextIndex == list.Count)
        {
            return list[0];
        }

        return list[nextIndex];
    }
    public static T Down<T>(this IList<T> list, T item)
    {
        var prevIndex = list.IndexOf(item) - 1;

        if (list.Count == 2 && prevIndex == 0)
        {
            // Only two left in list
            return list[0];
        }

        if (prevIndex < 0)
        {
            return list[list.Count - 1]; // Wrap around to the last item
        }

        return list[prevIndex];
    }
}
