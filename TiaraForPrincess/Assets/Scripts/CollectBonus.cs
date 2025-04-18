using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectBonus : MonoBehaviour
{
    [SerializeField] private UI_Control ui_Control;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bonus") || other.CompareTag("stone"))
        {
            if (other.CompareTag("stone"))
            {
                int stoneID = other.gameObject.GetComponent<StoneInfo>().StoneID;
                ui_Control.ViewBonus(TailPrefabPak.Instance.GetStoneSprite(stoneID));
                GameManager.Instance.currentPlayer.currentInventory.Add(stoneID);
            }
            if (other.CompareTag("bonus"))
            {
                int tailID = other.gameObject.GetComponent<TailControl>().TailID;
                ui_Control.ViewBonus(TailPrefabPak.Instance.GetTileSprite(tailID));
                GameManager.Instance.currentPlayer.currentInventory.Add(tailID + 20);
            }
            Destroy(other.gameObject);
        }
    }
}
