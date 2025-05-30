using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Inventory
{
    List<ItemTail> items = new List<ItemTail>();

    public int Count { get { return items.Count; } }

    public Inventory() { }
    public Inventory(string csv)
    {
        string[] ar = csv.Split('#', options: System.StringSplitOptions.RemoveEmptyEntries);
        foreach(string s in ar)
        {
            items.Add(new ItemTail(s));
        }
    }

    public void Add(int id, int number = 1)
    {
        foreach (ItemTail item in items)
        {
            if (item.ItemID == id)
            {
                item.ChangeCount(number);
                return;
            }
        }
        items.Add(new ItemTail(id, number));
        //Debug.Log($"id={id} num={number}");
    }

    public void Add(ItemTail newItem)
    {
        //Debug.Log($"ItemTail id={newItem.ItemID} num={newItem.Count}");
        foreach (ItemTail item in items)
        {
            if (item.ItemID == newItem.ItemID)
            {
                item.ChangeCount(item.Count);
                return;
            }
        }
        items.Add(new ItemTail(newItem.ItemID, newItem.Count));
    }

    public bool CheckItem(int id)
    {
        foreach(ItemTail item in items)
        {
            if (item.ItemID == id) return true;
        }
        return false;
    }

    public ItemTail GetItem(int index)
    {
        ItemTail item = ((index >= 0 && index < items.Count) ? items[index] : null);
        /*if (item != null)
        {
            if (item.Count > 1) items[index].ChangeCount(-1);
            else items.RemoveAt(index);
        }*/
        return item;
    }

    public void UpdateItems(Inventory current)
    {
        for(int i = 0; i < current.Count; i++)
        {
            Add(current.GetItem(i));
            //Debug.Log($"update item[{i}]={current.GetItem(i).GetCsvString()}");
        }
        //Debug.Log($"Update {ToCsvString()}");
    }

    public string ToCsvString(char sep = '#')
    {
        StringBuilder sb = new StringBuilder();
        foreach(ItemTail item in items)
        {
            sb.Append($"{item.GetCsvString()}{sep}");
        }
        return sb.ToString();
    }
}

public class ItemTail
{
    private int itemID;
    private Sprite sprite;
    private int count;

    public ItemTail() { }
    public ItemTail(int id, int number = 1)
    {
        itemID = id;
        count = number;
        if (id < 20) sprite = TailPrefabPak.Instance.GetStoneSprite(itemID);
        else sprite = TailPrefabPak.Instance.GetTileSprite(itemID - 20);
        //Debug.Log($"ItemTail {GetCsvString()}");
    }

    public ItemTail(string csvStr)
    {
        string[] ar = csvStr.Split('=');
        if (ar.Length == 2)
        {
            if (int.TryParse(ar[0], out int id) && int.TryParse(ar[1], out int number))
            {
                itemID = id;
                count = number;
                if (id < 20) sprite = TailPrefabPak.Instance.GetStoneSprite(itemID);
                else sprite = TailPrefabPak.Instance.GetTileSprite(itemID - 20);
            }
        }
    }

    public string GetCsvString(char sep = '=')
    {
        return $"{itemID}{sep}{count}";
    }

    public Sprite ItemSprite { get { return sprite; } }
    public int Count { get { return count; } }
    public int ItemID { get { return itemID; } }

    public bool ChangeCount(int number)
    {
        if (number < 0)
        {
            if (count + number < 0) return false;
            else count += number;
        }
        else count += number;
        return true;
    }
}
