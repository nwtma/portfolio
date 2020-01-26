using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heal : MonoBehaviour
{
    private float cooltime = 15;    //힐팩 사용 후 잴 쿨타임
    private float currentCoolTime;  //0으로 초기화
    private bool isOnCoolTime;      //쿨타임 여부 확인

    [SerializeField] public float healPoint; //치유량
    [SerializeField] public GameObject particle; //조개 반짝이 이펙트

    public GameObject healEffect;
    
    void Update()
    {
        if (isOnCoolTime)
        {
            currentCoolTime += Time.deltaTime;  //0에서 시간만큼 더해줌
            if(currentCoolTime >= cooltime)     //cooltime보다 수가 더 크면
            {
                currentCoolTime = 0;            //0으로 초기화
                isOnCoolTime = false;           //쿨타임 끝
                Heal_enable();                  //조개 활성화 시키는 함수로 이동
            }
        }
    }

    void Heal_enable()
    {
        this.GetComponent<Collider>().enabled = true;                     //조개 콜리더 활성화
        this.GetComponentsInChildren<MeshRenderer>()[0].enabled = true;   //조개 메쉬 활성화
        this.GetComponentsInChildren<MeshRenderer>()[1].enabled = true;   //조개 메쉬 활성화
        particle.SetActive(true);

        return;
    }

    void Heal_disable()
    {
        this.GetComponent<Collider>().enabled = false;                     //조개 콜리더 비활성화
        this.GetComponentsInChildren<MeshRenderer>()[0].enabled = false;   //조개 메쉬 비활성화
        this.GetComponentsInChildren<MeshRenderer>()[1].enabled = false;   //조개 메쉬 비활성화
        particle.SetActive(false);

        return;
    }

    void AfterHeal(Health health)
    {
        this.GetComponent<AudioSource>().Play();
        this.GetComponent<AudioSource>().volume = GameObject.Find("Option_Canvas").transform.Find("Window").transform.Find("Option_Window").transform.Find("SE_Slider").GetComponent<Slider>().value;
        Heal_disable();

        isOnCoolTime = true;    //쿨타임 시작

        health.HealEffect.SetActive(true);
        health.healAnimation.Play();
        Instantiate(healEffect, this.transform.position, Quaternion.identity);
    }

    bool IsPlayer(Collider col)
    {
        return col.transform.parent.tag == "Player";
    }

    bool IsFullHealth(Health health)
    {
        return health.HitPointsRemaining == health.fHitpoints;
    }

    bool IsOverHeal(Health health)
    {
        return health.HitPointsRemaining + healPoint > health.fHitpoints;
    }

    void OnTriggerEnter(Collider col)
    {
        if (!IsPlayer(col)) return; //플레이어가 아니라면 리턴

        Health col_health = col.GetComponentInChildren<Health>();   //
        if (IsFullHealth(col_health)) return;

        if (IsOverHeal(col_health))   //접촉한 콜리더의 남은 체력에 치유했을 때 최대 체력보다 클 때
        {
            col_health.fDamageTaken = 0;    //받은 데미지 0으로
        }

        else
        {
            col_health.fDamageTaken -= healPoint;  //받은 데미지 -healPoint
        }

        AfterHeal(col_health);
    }
    
    

}
    