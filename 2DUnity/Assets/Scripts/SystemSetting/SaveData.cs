using System;
using System.Collections.Generic;

[Serializable]
public class FishSaveSlot
{
    public string fishType;
    public int count;
}

[Serializable]
public class SaveData
{
    public List<FishSaveSlot> fishList = new List<FishSaveSlot>();
}
