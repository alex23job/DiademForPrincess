using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventPanelControl : MonoBehaviour
{
    [SerializeField] private Button btnUp;
    [SerializeField] private Button btnDown;
    [SerializeField] private Button[] arrBtnItems;

    private int currentItemsPos = 0;
    private int currentItemsNum = 0;
    private ItemTail currentItemTail = null;
    private GameObject tail = null;
    // Start is called before the first frame update
    void Start()
    {
        print($"Count inventory {GameManager.Instance.currentPlayer.inventory.Count} {GameManager.Instance.currentPlayer.inventory.ToCsvString()}");
        ViewInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnUpClick()
    {
        if (currentItemsPos > 1) currentItemsPos -= 2;
        ViewInventory();
    }

    public void OnDownClick()
    {
        if (currentItemsPos + 6 < GameManager.Instance.currentPlayer.inventory.Count) currentItemsPos += 2;
        ViewInventory();
    }

    public void OnBtnItemClick(int num)
    {
        if (tail != null)
        {
            IIsChild isChild = tail.GetComponent<StoneControl>() as IIsChild;
            if (isChild != null && isChild.IsChild == false) return;
            isChild = tail.GetComponent<TailControl>() as IIsChild;
            if (isChild != null && isChild.IsChild == false) return;
        }
        currentItemsNum = currentItemsPos + num;
        currentItemTail = GameManager.Instance.currentPlayer.inventory.GetItem(currentItemsNum);
        
        if (currentItemTail.ItemID < 20)
        {
            tail = Instantiate(TailPrefabPak.Instance.GetStone(currentItemTail.ItemID));
            tail.AddComponent<StoneControl>();
            tail.AddComponent<StoneInfo>();
            tail.GetComponent<StoneInfo>().SetStoneID(currentItemTail.ItemID);
        }
        if (currentItemTail.ItemID >= 20)
        {
            tail = Instantiate(TailPrefabPak.Instance.GetTail(currentItemTail.ItemID - 20));
            tail.GetComponent<TailControl>().SetID(currentItemTail.ItemID);
            for (int i = 0; i < tail.transform.childCount; i++)
            {
                tail.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        Vector3 pos = new Vector3(-1.8f, 2f, 0);
        if (tail != null) tail.transform.position = pos;
        ViewInventory();
    }

    private void ViewInventory()
    {
        int countItem = GameManager.Instance.currentPlayer.inventory.Count >= 6 ? 6 : GameManager.Instance.currentPlayer.inventory.Count;
        btnDown.interactable = GameManager.Instance.currentPlayer.inventory.Count > 6;
        btnUp.interactable = GameManager.Instance.currentPlayer.inventory.Count > 6;
        for (int i = 0; i < 6; i++)
        {
            if (i < countItem)
            {
                arrBtnItems[i].gameObject.SetActive(true);
                ItemTail item = GameManager.Instance.currentPlayer.inventory.GetItem(currentItemsPos + i);
                if (item != null)
                {
                    Text txtItem = arrBtnItems[i].transform.GetChild(0).gameObject.GetComponent<Text>();
                    Image img = arrBtnItems[i].transform.GetChild(1).gameObject.GetComponent<Image>();
                    Text txtCount = arrBtnItems[i].transform.GetChild(2).gameObject.GetComponent<Text>();
                    img.sprite = item.ItemSprite;
                    string nameItem = (item.ItemID < 20) ? TailPrefabPak.Instance.GetNameStone(item.ItemID) : TailPrefabPak.Instance.GetNameTail(item.ItemID - 20);
                    txtItem.text = $"{nameItem}";
                    txtCount.text = $"{item.Count}";
                }
                else
                {
                    arrBtnItems[i].gameObject.SetActive(false);
                }
            }
            else
            {
                arrBtnItems[i].gameObject.SetActive(false);
            }
        }
    }
}
