using UnityEngine;

public class Items : MonoBehaviour
{
    #region Instance
    public static Items Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    #endregion
    public bool HasWeapon = false;
    
}
