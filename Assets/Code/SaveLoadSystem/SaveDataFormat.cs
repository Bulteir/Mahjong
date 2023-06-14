using System.Collections.Generic;

public class SaveDataFormat
{
    public int totalCoin;
    public List<LevelProperties> levelProperties;
    public string saveTime;
    public bool saveFileIsSyncEver;
    public int totalEnergy;
    public string lastEnergyGainTime;
    public int shuffleJokerQuantity;
    public int undoJokerQuantity;
    public bool noAdsJokerActive;
}

public struct LevelProperties
{
    public string LevelName;
    public string bestTime;
    public bool levelPassed;
    public bool levelPurchased;
    public int earnedStarQuantity;
}
