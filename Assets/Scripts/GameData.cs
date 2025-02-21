using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData : MonoBehaviour
{

    public int currency; // �Թ���
    public int multiplier; // ��Ҥٳ�Թ
    public int autoClickerCount; // �ӹǹ Auto Clicker
    public int autoClickerCost; // �ҤҢͧ Auto Clicker
    public List<SavedItem> purchasedItems = new List<SavedItem>(); // ��¡�������������
}
    

[Serializable]
public class SavedItem
{
    public string itemName;
    public Vector3 position; // ���˹觢ͧ�����㹩ҡ
}

