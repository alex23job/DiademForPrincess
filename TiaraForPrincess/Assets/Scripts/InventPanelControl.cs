using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventPanelControl : MonoBehaviour
{
    [SerializeField] private Button btnUp;
    [SerializeField] private Button btnDown;
    [SerializeField] private Button[] arrBtnItems;

    private int currentItem = 0;
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
        if (currentItem + 6 < GameManager.Instance.currentPlayer.inventory.Count) currentItem += 2;
        ViewInventory();
    }

    public void OnDownClick()
    {
        if (currentItem > 1) currentItem -= 2;
        ViewInventory();
    }

    public void OnBtnItemClick(int num)
    {
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
                ItemTail item = GameManager.Instance.currentPlayer.inventory.GetItem(currentItem + i);
                Text txtItem = arrBtnItems[i].transform.GetChild(0).gameObject.GetComponent<Text>();
                Image img = arrBtnItems[i].transform.GetChild(1).gameObject.GetComponent<Image>();
                img.sprite = item.ItemSprite;
                txtItem.text = $"{item.ItemID} {item.Count}";
            }
            else
            {
                arrBtnItems[i].gameObject.SetActive(false);
            }
        }
    }
}
