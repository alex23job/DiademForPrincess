using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailPrefabPak : MonoBehaviour
{
    public static TailPrefabPak Instance;

    [SerializeField] private GameObject[] arrStones;
    [SerializeField] private GameObject[] arrGoldTiles;
    [SerializeField] private Material[] arrTailMat;

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
}
