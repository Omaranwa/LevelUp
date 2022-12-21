using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProgressDataManager
{
    private static string FileLocation = $"{Application.persistentDataPath}/Progress.dat";

    public static void Load()
    {
        if (File.Exists(FileLocation))
        {
            string fileData = File.ReadAllText(FileLocation);
            Progress progress = JsonUtility.FromJson<Progress>(fileData);

            GameManager.Instance.ApplyProgress(progress);
        }
        else
        {
            Debug.LogError("No save file exists for Progress");
        }
    }

    public static void Save()
    {
        Progress progress = GameManager.Instance.GetProgress();
        
        string fileContent = JsonUtility.ToJson(progress);
        File.WriteAllText(FileLocation, fileContent);
    }
}

public class Progress
{
    public float Currency;
}

public class UpgradeDataManager
{
    private static string FileLocation = $"{Application.persistentDataPath}/Upgrades.dat";

    public static void Load()
    {
        UpgradeModelContainer container = null;
        if (File.Exists(FileLocation))
        {
            string fileData = File.ReadAllText(FileLocation);
            container = JsonUtility.FromJson<UpgradeModelContainer>(fileData);
        }
        
        // update settings
        GameManager.UpgradeManager.LoadSerializedUpgrades(container);
    }

    public static void Save()
    {
        // grab values from settings
        List<Upgrade> upgrades = GameManager.UpgradeManager.GetAllUpgrades();
        UpgradeModelContainer container = new UpgradeModelContainer(upgrades);
        
        // write to file
        string fileContent = JsonUtility.ToJson(container);
        File.WriteAllText(FileLocation, fileContent);
    }
}

[Serializable]
public class UpgradeModelContainer
{
    public List<UpgradeModel> upgrades;

    public UpgradeModelContainer(List<Upgrade> upgrades)
    {
        foreach (var upgrade in upgrades)
        {
            AddUpgrade(upgrade);
        }
    }
    
    public void AddUpgrade(Upgrade upgrade)
    {
        if (upgrades == null)
        {
            upgrades = new List<UpgradeModel>();
        }

        UpgradeModel newUpgrade = null;
        switch (upgrade)
        {
            case PlayerUpgrade playerUpgrade:
                var typedUpgrade = new PlayerUpgradeModel();
                typedUpgrade.numberPurchased = playerUpgrade.numberPurchased;
                typedUpgrade.upgradeType = playerUpgrade.upgradeType;
                newUpgrade = typedUpgrade; 
                break;
            case WeaponUpgrade weaponUpgrade:
                var typedUpgrade2 = new WeaponUpgradeModel();
                typedUpgrade2.numberPurchased = weaponUpgrade.numberPurchased;
                typedUpgrade2.upgradeType = weaponUpgrade.upgradeType;
                newUpgrade = typedUpgrade2; 
                break;
        }
        
        upgrades.Add(newUpgrade);
    }
}

[Serializable]
public class UpgradeModel
{
    public int numberPurchased;
}

[Serializable]
public class PlayerUpgradeModel : UpgradeModel
{
    public PlayerUpgradeType upgradeType;
}

[Serializable]
public class WeaponUpgradeModel : UpgradeModel
{
    public WeaponUpgradeType upgradeType;
}
