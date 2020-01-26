using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleInArea.Game
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField]
        public float FloatingmoveSpeed;                                    //플로팅 텍스트 이동 스피드

        [SerializeField]
        public float FloatingdestroyTime;                                  //플로팅 텍스트 삭제시간

        [SerializeField]
        public Text Floatingtext;                                          //플로팅 텍스트 프리팹

        private Vector3 Floatingvector;                                    //플로팅 텍스트 이동거리


        CharacterController character;


        // Update is called once per frame
        void Update() { 
            FloatingTextShow();
        }

        public void FloatingTextShow()                                                    // 플로팅 텍스트
        {
            Floatingvector.Set(Floatingtext.transform.position.x, Floatingtext.transform.position.y + (FloatingmoveSpeed * Time.deltaTime), Floatingtext.transform.position.z);
            Floatingtext.transform.position = Floatingvector;

            FloatingdestroyTime -= Time.deltaTime;

            if (FloatingdestroyTime <= 0)
                Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
}