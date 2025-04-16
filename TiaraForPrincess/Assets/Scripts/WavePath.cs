using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class WavePath
{
    private int[] arQu;
    private int startNum = -1, endNum = -1;
    private List<int> path = new List<int>();

    public int[] GetPath()
    {
        return path.ToArray();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateBoard(GameObject[] arGo)
    {
        int i, maxZn = arGo.Length * arGo.Length;
        arQu = new int[arGo.Length];
        for(i = 0; i < arGo.Length; i++)
        {
            if (arGo[i] == null) arQu[i] = -1;
            else arQu[i] = maxZn;
        }
    }

    public void SetStartEnd(int s_num, int e_num)
    {
        startNum = s_num;
        endNum = e_num;
    }

    public bool FindPath()
    {
        int[] Wave = new int[arQu.Length];
        int i, step = 1, x, y, countMaxQu = 0, countQu, maxZn = arQu.Length * arQu.Length;
        for (i = 0; i < arQu.Length; i++)
        {
            Wave[i] = arQu[i];
            if (Wave[i] != -1) countMaxQu++;
        }
        countQu = countMaxQu;

        StringBuilder sb = new StringBuilder();
        for (i = 0; i < Wave.Length; i++)
        {
            sb.Append($"{Wave[i]} ");
        }
        sb.Append($"  maxQu={countQu}");
        Debug.Log(sb.ToString());
        //return false;
        if (startNum != -1 && endNum != -1)
        {
            Wave[startNum] = 0;
            while (step < 50)
            {
                for (i = 0; i < Wave.Length; i++)
                {
                    if (Wave[i] == -1 || Wave[i] >= step) continue;
                    //i = startNum;
                    x = i % 3; y = i / 3;
                    if ((x > 0) && (Wave[i - 1] != -1) && (Wave[i - 1] == maxZn)) Wave[i - 1] = step;
                    if ((x < 2) && (Wave[i + 1] != -1) && (Wave[i + 1] == maxZn)) Wave[i + 1] = step;
                    if ((y > 0) && (Wave[i - 3] != -1) && (Wave[i - 3] == maxZn)) Wave[i - 3] = step;
                    if ((y < 5) && (Wave[i + 3] != -1) && (Wave[i + 3] == maxZn)) Wave[i + 3] = step;
                }
                step++;
                for (i = 0, countQu = 0; i < arQu.Length; i++)
                {
                    if (Wave[i] != -1 && Wave[i] != maxZn) countQu++;
                }
                sb = new StringBuilder();
                for (i = 0; i < Wave.Length; i++)
                {
                    sb.Append($" {((i % 3 == 0) ?  '#' : ' ')} {Wave[i]}");
                }
                sb.Append($" step = {step}    countQu = {countQu}");
                Debug.Log(sb.ToString());
                if (countQu == countMaxQu) break;
            }
            //return false;
            if (Wave[endNum] != arQu.Length * arQu.Length)
            {   //  волна дошла до конечной точки - путь есть
                //  нужно его перенести в path
                step = Wave[endNum];
                path.Add(endNum);
                i = endNum;
                int ei = i;
                while(step > 0)
                {
                    x = i % 3; y = i / 3;
                    if ((x > 0) && (Wave[i - 1] != -1) && (Wave[i - 1] < Wave[i])) { step = Wave[i - 1]; path.Add(i - 1); ei = i - 1; }
                    if ((x < 2) && (Wave[i + 1] != -1) && (Wave[i + 1] < Wave[i])) { step = Wave[i + 1]; path.Add(i + 1); ei = i + 1; }
                    if ((y > 0) && (Wave[i - 3] != -1) && (Wave[i - 3] < Wave[i])) { step = Wave[i - 3]; path.Add(i - 3); ei = i - 3; }
                    if ((y < 5) && (Wave[i + 3] != -1) && (Wave[i + 3] < Wave[i])) { step = Wave[i + 3]; path.Add(i + 3); ei = i + 3; }
                    i = ei;
                }
                //path.Add(startNum);
                path.Reverse();
                return true;
            }
        }
        return false;
    }
}
