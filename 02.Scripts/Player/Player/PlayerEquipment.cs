
using System;
using System.Collections.Generic;

public class PlayerEquipment
{
    public event Action OnEquipItem;
    public Dictionary<int, ItemData> EquipItems { get; private set; } = new Dictionary<int, ItemData>();
    
    public void EquipItem(ItemData item)
    {
        
    }
    

}

