using System;
using UnityEngine;

public class ActionNotifier : MonoBehaviour
{
    #region Instance
    public static ActionNotifier Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    #endregion

    public Action JapMaster;
    public Action<Enums.Items> Item;
    public Action<Enums.Puzzles> Puzzle;
    public Action<WorldColor> WorldColor;
}
