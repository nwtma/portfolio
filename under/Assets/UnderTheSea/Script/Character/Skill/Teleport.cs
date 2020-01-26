using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Anemone[] ane;
    GameObject clown;

    void Start()
    {
        clown = GameObject.Find("fish");
    }

    public void SearchRandomTransform()
    {
        int tp = Random.Range(0, 8);

        for (int i = 0; i < ane.Length; ++i)
        {
            if (ane[i].isEnter && ane[tp].name == ane[i].name)
            {
                SearchRandomTransform();
            }

            else if (ane[i].isEnter)
            {
                Instantiate(TP_Effect, ane[i].transform.position, Quaternion.identity);
                MoveToPoint(ane[tp].transform);
                return;
            }
        }

    }

    public GameObject TP_Effect;    //텔레포트할 때 나타날 이펙트

    public void MoveToPoint(Transform NewT)
    {
        clown.transform.position = NewT.position;
        Instantiate(TP_Effect, NewT.position, Quaternion.identity);
    }
}
