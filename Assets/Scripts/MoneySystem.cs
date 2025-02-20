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
    public TMP_Text autoClickerText; // UI �ʴ��ӹǹ Auto Clicker
    public TMP_Text autoClickCostText; // UI �ʴ��ҤҢͧ Auto Clicker
    public ShopItem[] items; // ��¡���Թ���
    public Transform[] spawnPoints; // �ش Spawn ����Ѻ���������

    public int autoClickerCount = 0; // �ӹǹ Auto Clicker
    public int autoClickerCost = 100; // �Ҥҫ��� Auto Clicker
    public float clickInterval = 1f; // ��������㹡�ä�ԡ�ѵ��ѵ�

    public AudioSource audioSource; // ���������§
    public AudioClip buySuccessSound; // ���§�͹���������
    public AudioClip buyFailSound; // ���§�͹�Թ����

    private int currency = 0;  // �Թ�������
    private int multiplier = 1;  // ������ͤ�ԡ
    private int upgradeCost = 10; // ��������㹡���ѻ�ô
    private float nextClickTime = 0f;


    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        if (autoClickerCount > 0 && Time.time >= nextClickTime)
        {
            nextClickTime = Time.time + clickInterval; // ������� Auto Clicker
            EarnMoney(autoClickerCount * 10); // Auto Clicker ���Թ�ѵ��ѵ�
        }
    }

    public void EarnMoney()
    {
        currency += multiplier;  // ���Թ�����Ҥٳ
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
            currency -= upgradeCost;  // �ѡ�Թ
            multiplier *= 2;          // �ٳ���������
            upgradeCost *= 2;         // �Ҥ��ѻ�ô�������
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
        if (index < 0 || index >= items.Length) return; // ��ͧ�ѹ index �Դ��Ҵ
        if (index < 0 || index >= spawnPoints.Length) return; // ��ͧ�ѹ index �ͧ spawnPoint �Դ��Ҵ

        ShopItem selectedItem = items[index]; // �֧�����������������͡
        Transform spawnPoint = spawnPoints[index]; // �� SpawnPoint ����ӴѺ�����

        if (currency >= selectedItem.price)
        {
            currency -= selectedItem.price; // �ѡ�Թ

            // ��Ѻ��� Z Position �� 0
            Vector3 spawnPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, 0);

            // ���ҧ��������ش Spawn ����˹� Z �� 0
            GameObject newItem = Instantiate(selectedItem.prefab, spawnPosition, Quaternion.identity);

            UpdateUI();
            audioSource.PlayOneShot(buySuccessSound); // ������§���������
            Debug.Log("Purchased: " + selectedItem.itemName);
            Debug.Log("Spawned: " + selectedItem.itemName + " at " + spawnPosition);
        }
        else
        {
            audioSource.PlayOneShot(buyFailSound); // ������§�Թ����
            Debug.Log("Not enough money!");
        }
    }

    public void BuyAutoClicker()
    {
        if (currency >= autoClickerCost)
        {
            currency -= autoClickerCost; // �ѡ�Թ
            autoClickerCount++; // �����ӹǹ Auto Clicker
            autoClickerCost = Mathf.RoundToInt(autoClickerCost * 1.2f); // �ҤҢ�鹷��� 20%
            UpdateUI();
            Debug.Log("Bought Auto Clicker! Total: " + autoClickerCount);
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }
}



