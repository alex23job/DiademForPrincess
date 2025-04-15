using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Control : MonoBehaviour
{
    [SerializeField] private Button[] arrColorBtn;
    [SerializeField] private Image[] arrImgBonus;
    [SerializeField] private Sprite question; 

    private int indexBonus = 0;

    // Start is called before the first frame update
    void Start()
    {
        ViewBtnState(new int[arrColorBtn.Length]);
        indexBonus = 0;
        ClearImgBonus();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void ViewBtnState(int[] arrCounts)
    {
        for (int i = 0; i < arrColorBtn.Length; i++)
        {
            arrColorBtn[i].interactable = arrCounts[i] > 0;
            arrColorBtn[i].transform.GetChild(1).GetComponent<Text>().text = arrCounts[i].ToString();
        }
    }

    public void ViewBonus(Sprite spr)
    {
        arrImgBonus[indexBonus].color = Color.white;
        arrImgBonus[indexBonus++].sprite = spr;
    }

    public void ClearImgBonus()
    {
        arrImgBonus[0].color = Color.red;
        arrImgBonus[1].color = Color.red;
        arrImgBonus[0].sprite = question;
        arrImgBonus[1].sprite = question;
    }
}
