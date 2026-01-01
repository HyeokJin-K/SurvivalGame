using UnityEngine;


public enum ItemType
{
    Resrouce,   
    Equipable,  
    Consumable 
}

public enum ConsumableType
{
    Health,
    Stamina
}

[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type; 
    public float value; 
}

[CreateAssetMenu(fileName = "item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string itemName; // 아이템명
    public string description; // 설명
    public Sprite icon; // 아이콘
    public GameObject dropPrefab; // 드랍 아이템

    [Header("Stack")]
    public bool canStack;
    public int maxStackAmount; // 최대 수

    [Header("Consumalbe")]
    public ItemDataConsumable[] consumables;
}
