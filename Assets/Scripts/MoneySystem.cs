using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.IO;

[System.Serializable]
public class SaveData
{
    public int currency;
    public int multiplier;
    public int autoClickerCount;
    public int autoClickerCost;
    public int upgradeCost;
    public List<int> autoClickerCosts; // เก็บราคาของ Auto Clicker ตามลำดับ
    public List<ItemData> purchasedItems = new List<ItemData>();
}

[System.Serializable]
public class ItemData
{
    public string itemName;
    public float x, y, z; // ตำแหน่งของไอเท็ม
}

public class MoneySystem : MonoBehaviour
{
    public TMP_Text currencyText;
    public TMP_Text multiplierText;
    public TMP_Text upgradeCostText;
    public TMP_Text autoClickerText; // UI แสดงจำนวน Auto Clicker
    public TMP_Text autoClickCostText; // UI แสดงราคาของ Auto Clicker
    public ShopItem[] items; // รายการสินค้า
    public Transform[] spawnPoints; // จุด Spawn สำหรับแต่ละไอเท็ม

    public int autoClickerCount = 0; // จำนวน Auto Clicker
    public int autoClickerCost = 100; // ราคาซื้อ Auto Clicker
    public float clickInterval = 1f; // ความเร็วในการคลิกอัตโนมัติ

    [SerializeField] AudioSource audioSource; // ตัวเล่นเสียง
    [SerializeField] AudioClip buySuccessSound; // เสียงตอนซื้อสำเร็จ
    [SerializeField] AudioClip buyFailSound; // เสียงตอนเงินไม่พอ
    [SerializeField] AudioClip ClickSound;

    private int currency = 0;  // เงินเริ่มต้น
    private int multiplier = 1;  // รายได้ต่อคลิก
    private int upgradeCost = 10; // ค่าใช้จ่ายในการอัปเกรด
    private float nextClickTime = 0f;


    private List<GameObject> spawnedItems = new List<GameObject>();

    private string saveFilePath;


    void Start()
    {
        saveFilePath = Application.persistentDataPath + "/savegame.json";
        LoadGame();
        UpdateUI();
    }

    void Update()
    {
        if (autoClickerCount > 0 && Time.time >= nextClickTime)
        {
            nextClickTime = Time.time + clickInterval; // ตั้งเวลา Auto Clicker
            EarnMoney(autoClickerCount * 10); // Auto Clicker ทำเงินอัตโนมัติ
        }
    }

    public void EarnMoney()
    {
        currency += multiplier;  // ได้เงินตามค่าคูณ
        UpdateUI();
        audioSource.PlayOneShot(ClickSound);
    }
    void EarnMoney(int amount)
    {
        currency += amount;
        UpdateUI();
    }

    public void UpgradeMultiplier()
    {
        if (currency >= upgradeCost)
        {
            currency -= upgradeCost;  // หักเงิน
            multiplier *= 2;          // คูณรายได้เพิ่ม
            upgradeCost *= 2;         // ราคาอัปเกรดเพิ่มขึ้น
            Debug.Log("Upgraded! New Multiplier: x" + multiplier);
            audioSource.PlayOneShot(buySuccessSound);
        }
        else
        {
            Debug.Log("Not enough money!");
            audioSource.PlayOneShot(buyFailSound);
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        currencyText.text = "Money: $" + currency;
        multiplierText.text = "Multiplier: x" + multiplier;
        upgradeCostText.text = "Upgrade Cost: $" + upgradeCost;
        autoClickerText.text = "Auto Clickers: " + autoClickerCount;
        autoClickCostText.text = "Auto Clicker Cost: $" + autoClickerCost;
    }

    public void BuyItem(int index)
    {
        if (index < 0 || index >= items.Length) return; // ป้องกัน index ผิดพลาด
        if (index < 0 || index >= spawnPoints.Length) return; // ป้องกัน index ของ spawnPoint ผิดพลาด

        ShopItem selectedItem = items[index]; // ดึงข้อมูลไอเท็มที่เลือก
        Transform spawnPoint = spawnPoints[index]; // ใช้ SpawnPoint ตามลำดับไอเท็ม

        if (currency >= selectedItem.price)
        {
            currency -= selectedItem.price; // หักเงิน

            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0);
            GameObject newItem = Instantiate(selectedItem.prefab, spawnPosition, Quaternion.identity);
            spawnedItems.Add(newItem); // เก็บอ้างอิงไอเท็มที่ Spawn

            SaveGame();
            UpdateUI();
            audioSource.PlayOneShot(buySuccessSound); // เล่นเสียงซื้อสำเร็จ
            //Debug.Log("Purchased: " + selectedItem.itemName);
            //Debug.Log("Spawned: " + selectedItem.itemName + " at " + spawnPosition);
        }
        else
        {
            audioSource.PlayOneShot(buyFailSound); // เล่นเสียงเงินไม่พอ
            Debug.Log("Not enough money!");
        }
    }

    public void BuyAutoClicker()
    {
        if (currency >= autoClickerCost)
        {
            currency -= autoClickerCost; // หักเงิน
            autoClickerCount++; // เพิ่มจำนวน Auto Clicker
            autoClickerCost = Mathf.RoundToInt(autoClickerCost * 1.2f); // ราคาขึ้นทีละ 20%
            UpdateUI();
            Debug.Log("Bought Auto Clicker! Total: " + autoClickerCount);
            audioSource.PlayOneShot(buySuccessSound);
        }
        else
        {
            Debug.Log("Not enough money!");
            audioSource.PlayOneShot(buyFailSound);
        }
    }

    public void SaveGame()
    {
        SaveData data = new SaveData
        {
            currency = currency,
            multiplier = multiplier,
            autoClickerCount = autoClickerCount,
            autoClickerCost = autoClickerCost,
            upgradeCost = upgradeCost
        };

        // บันทึกไอเท็มที่ Spawn
        foreach (GameObject item in spawnedItems)
        {
            ItemData itemData = new ItemData
            {
                itemName = item.name.Replace("(Clone)", "").Trim(),
                x = item.transform.position.x,
                y = item.transform.position.y,
                z = item.transform.position.z
            };
            data.purchasedItems.Add(itemData);
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game Saved!");
        audioSource.PlayOneShot(ClickSound);
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            currency = data.currency;
            multiplier = data.multiplier;
            autoClickerCount = data.autoClickerCount;
            autoClickerCost = data.autoClickerCost;
            upgradeCost = data.upgradeCost != 0 ? data.upgradeCost : 10;  // ถ้า upgradeCost = 0 ให้ตั้งเป็น 10

            // ลบไอเท็มเก่าที่เคย Spawn
            foreach (GameObject item in spawnedItems)
            {
                Destroy(item);
            }
            spawnedItems.Clear();

            // โหลดไอเท็มที่ซื้อไว้
            foreach (ItemData itemData in data.purchasedItems)
            {
                foreach (ShopItem shopItem in items)
                {
                    string shopItemName = shopItem.itemName.Trim();
                    string savedItemName = itemData.itemName.Trim();

                    // ตรวจสอบว่าชื่อที่เซฟตรงกับชื่อ Prefab หรือไม่
                    if (shopItemName == savedItemName)
                    {
                        Vector3 position = new Vector3(itemData.x, itemData.y, itemData.z);
                        GameObject newItem = Instantiate(shopItem.prefab, position, Quaternion.identity);

                        spawnedItems.Add(newItem); // บันทึกไอเท็มที่ Spawn ใหม่
                        Debug.Log("Loaded item: " + savedItemName + " at " + position);
                        break;
                    }
                }
            }

            Debug.Log("Game Loaded!");
        }
        else
        {
            Debug.Log("No Save Data Found");
        }
        audioSource.PlayOneShot(ClickSound);
        UpdateUI();
    }

    // 🔹 **รีเซ็ตเกม**
    public void ResetGame()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }

        currency = 0;
        multiplier = 1;
        autoClickerCount = 0;
        autoClickerCost = 100;
        upgradeCost = 10;

        foreach (GameObject item in spawnedItems)
        {
            Destroy(item);
        }
        spawnedItems.Clear();
        audioSource.PlayOneShot(ClickSound);
        SaveGame();
        UpdateUI();
        Debug.Log("Game Reset!");
    }

}




