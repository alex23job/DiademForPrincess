using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Control : MonoBehaviour
{
    [SerializeField] private Button[] arrColorBtn;

    // Start is called before the first frame update
    void Start()
    {
        ViewBtnState(new int[arrColorBtn.Length]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ViewBtnState(int[] arrCounts)
    {
        for (int i = 0; i < arrColorBtn.Length; i++)
        {
            arrColorBtn[i].interactable = arrCounts[i] > 0;
            arrColorBtn[i].transform.GetChild(1).GetComponent<Text>().text = arrCounts[i].ToString();
        }
    }
}
