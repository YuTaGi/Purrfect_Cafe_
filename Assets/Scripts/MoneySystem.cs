using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

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

    public AudioSource audioSource; // ตัวเล่นเสียง
    public AudioClip buySuccessSound; // เสียงตอนซื้อสำเร็จ
    public AudioClip buyFailSound; // เสียงตอนเงินไม่พอ

    private int currency = 0;  // เงินเริ่มต้น
    private int multiplier = 1;  // รายได้ต่อคลิก
    private int upgradeCost = 10; // ค่าใช้จ่ายในการอัปเกรด
    private float nextClickTime = 0f;


    void Start()
    {
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
        }
        else
        {
            Debug.Log("Not enough money!");
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

            // ปรับค่า Z Position เป็น 0
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0);

            // สร้างไอเท็มที่จุด Spawn ที่กำหนด Z เป็น 0
            GameObject newItem = Instantiate(selectedItem.prefab, spawnPosition, Quaternion.identity);

            UpdateUI();
            audioSource.PlayOneShot(buySuccessSound); // เล่นเสียงซื้อสำเร็จ
            Debug.Log("Purchased: " + selectedItem.itemName);
            Debug.Log("Spawned: " + selectedItem.itemName + " at " + spawnPosition);
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
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }
}



