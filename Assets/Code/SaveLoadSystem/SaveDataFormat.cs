using System.Collections.Generic;

public class SaveDataFormat
{
    public int totalCoin;
    public List<LevelProperties> levelProperties;
    public string saveTime;
}

public struct LevelProperties
{
    public string LevelName;
    public string bestTime;
    public bool levelPassed;
    public bool levelPurchased;
}
