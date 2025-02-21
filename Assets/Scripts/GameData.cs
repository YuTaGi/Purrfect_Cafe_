using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData : MonoBehaviour
{

    public int currency; // เงินในเกม
    public int multiplier; // ค่าคูณเงิน
    public int autoClickerCount; // จำนวน Auto Clicker
    public int autoClickerCost; // ราคาของ Auto Clicker
    public List<SavedItem> purchasedItems = new List<SavedItem>(); // รายการไอเท็มที่ซื้อ
}
    

[Serializable]
public class SavedItem
{
    public string itemName;
    public Vector3 position; // ตำแหน่งของไอเท็มในฉาก
}

