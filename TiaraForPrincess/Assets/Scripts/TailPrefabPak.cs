using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailPrefabPak : MonoBehaviour
{
    public static TailPrefabPak Instance;

    [SerializeField] private GameObject[] arrStones;
    [SerializeField] private GameObject[] arrGoldTiles;
    [SerializeField] private Material[] arrTailMat;
    [SerializeField] private Sprite[] arrStonesSprites;
    [SerializeField] private Sprite[] arrTilesSprites;
    [SerializeField] private string[] nameStonesRu;
    [SerializeField] private string[] nameStonesEn;
    [SerializeField] private string[] nameTailesRu;
    [SerializeField] private string[] nameTailesEn;

    public int CountStones { get { return arrStones.Length; } }
    public int CountTiles {  get { return arrGoldTiles.Length; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetStone(int index)
    {
        if (index >= 0 && index < arrStones.Length) return arrStones[index];
        return null;
    }

    public GameObject GetTail(int index)
    {
        if (index >= 0 && index < arrGoldTiles.Length) return arrGoldTiles[index];
        return null;
    }

    public Material GetMaterial(int index)
    {
        return ((index >= 0 && index < arrTailMat.Length) ? arrTailMat[index] : arrTailMat[0]);
    }

    public Sprite GetStoneSprite(int index)
    {
        return ((index >= 0 && index < arrStonesSprites.Length) ? arrStonesSprites[index] : arrStonesSprites[0]);
    }

    public Sprite GetTileSprite(int index)
    {
        return ((index >= 0 && index < arrTilesSprites.Length) ? arrTilesSprites[index] : arrTilesSprites[0]);
    }

    public string GetNameStone(int index)
    {
        string nm = "Stone";
        if (Language.Instance.CurrentLanguage == "ru")
        {
            if (index >= 0 && index < nameStonesRu.Length)
            {
                nm = nameStonesRu[index];
            }
            else nm = "Камень";
        }
        else
        {
            if (index >= 0 && index < nameStonesEn.Length)
            {
                nm = nameStonesEn[index];
            }

        }
        return nm;
    }
    public string GetNameTail(int index)
    {
        string nm = "Tail";
        if (Language.Instance.CurrentLanguage == "ru")
        {
            if (index >= 0 && index < nameTailesRu.Length)
            {
                nm = nameTailesRu[index];
            }
            else nm = "Деталь";
        }
        else
        {
            if (index >= 0 && index < nameTailesEn.Length)
            {
                nm = nameTailesEn[index];
            }

        }
        return nm;
    }
}

public interface IIsChild
{    
    bool IsChild { get; set; }
}
