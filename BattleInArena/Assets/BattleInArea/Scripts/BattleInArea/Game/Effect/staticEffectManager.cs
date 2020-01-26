using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

namespace BattleInArea.Game
{
    public class staticEffectManager : MonoBehaviour
    {
        static public staticEffectManager instance;


        [SerializeField]
        protected GameObject[] StayEffects;
        [SerializeField]
        protected GameObject[] LaunchEffects;
        [SerializeField]
        protected GameObject[] MoveEffects;


        [SerializeField]
        public GameObject clones;

        [SerializeField]
        protected GameObject clones2;

        [SerializeField]
        protected CharacterController ctn;
        protected Vector3 ctnPosition;

        protected Vector3 TilePosition;

        [SerializeField]
        protected Quaternion PrefabsRotation;


        [SerializeField]
        protected GameObject AniImgeObject;

        protected List<string> AniArray = new List<string>();


        Animation anim;

        protected int moveTileX;
        protected int moveTileZ;
        protected UnityEngine.Vector3 goal;
        protected UnityEngine.Vector3 pos;

        enum AniColor
        {
            AniBlue,
            AniGreen,
            AniRed,
            AniYellow,
        }


        void Awake()    
        {
            ctn = gameObject.GetComponent<CharacterController>();
            if (instance == null)
            {
                instance = this;
            }
        }

        private void Start()
        {
            anim = AniImgeObject.GetComponent<Animation>();
            AnimationArray();
        }

        public void Update()
        {
            
            
        }

        public void SettingEffectOnItem(string effectName)
        {
            for (int i = 0; i < StayEffects.Length; i++)
            {
                if (StayEffects[i].name == effectName)
                {
                    clones2 = Instantiate(StayEffects[i], ctnPosition, PrefabsRotation);
                   // PhotonNetwork.Instantiate("Characters/BIA_Effect/" + StayEffects[i].name, ctnPosition, PrefabsRotation, 0);
                    break;
                }
            }
            Destroy(this.clones, 3.0f);
        }

        
        public void SettingTarget(CharacterController character)
        {
            ctnPosition = character.transform.position;
        }

        public void StayEffectOn(string effectName)
        {
            for (int i = 0; i < StayEffects.Length; i++)
            {
                if (StayEffects[i].name == effectName)
                {
                    clones = Instantiate(StayEffects[i], ctnPosition, PrefabsRotation);
                    //PhotonNetwork.Instantiate("Characters/BIA_Effect/" + StayEffects[i].name, ctnPosition, PrefabsRotation, 0);
                    break;
                }
            }
            Destroy(this.clones, 3.0f);
        }
        public void PrefabsSetting1(CharacterController character)
        {
            if (character.SkillTileZ > character.Position.z)
            {
                PrefabsRotation = Quaternion.Euler(0, 0, 0);
            }
            else if (character.SkillTileZ < character.Position.z)
            {
                PrefabsRotation = Quaternion.Euler(0, -180, 0);
            }
            else
            {
                PrefabsRotation = Quaternion.Euler(0, 0, 0);
            }
        }
 
        public void LaunchEffectOn1(string effectName) // 적 캐릭터의 포지션을 기준으로 이펙트가 생성
        {
            for (int i = 0; i < LaunchEffects.Length; i++)
            {
                if (LaunchEffects[i].name == effectName)
                {                                       // 상대 캐릭터 좌표
                    clones = Instantiate(LaunchEffects[i], ctnPosition, PrefabsRotation); 
                   // PhotonNetwork.Instantiate("Characters/BIA_Effect/" + LaunchEffects[i].name, ctnPosition, PrefabsRotation,0);
                    //PhotonNetwork.Destroy()
                    break;
                }
            }
            Destroy(this.clones, 3.0f);
        }

        public void LaunchEffectOn2(string effectName) // 위에서 아래로 내려오는 스킬
        {
            for (int i = 0; i < LaunchEffects.Length; i++)
            {
                if (LaunchEffects[i].name == effectName)
                {                                       
                    clones = Instantiate(LaunchEffects[i], ctnPosition, Quaternion.Euler(90,0,0));
                   // PhotonNetwork.Instantiate("Characters/BIA_Effect/" + LaunchEffects[i].name, ctnPosition, Quaternion.Euler(90, 0, 0), 0);
   
                    break;
                }
            }
            Destroy(this.clones, 3.0f);
        }
        public IEnumerator PrefabsDestroy()
        {
           
           yield return new WaitForSeconds(1.0f);

        }



        public void SetItemEffect(string itemName, CharacterController mrCharacter, CharacterController targetCharacter)
        {
            switch(itemName)
            {
                case "HpItem":
                    {
                        SettingTarget(mrCharacter);
                        StayEffectOn("Heal Effect");
                    }
                    break;
                case "ApItem":
                    {
                        SettingTarget(mrCharacter);
                        StayEffectOn("Ap Effect");
                    }
                    break;
                case "ShieldItem" : 
                    {
                        SettingTarget(mrCharacter);
                        StayEffectOn("Shield Effect3");
                        ctnPosition.y += 1.5f;
                        //SettingEffectOnItem("Shield Image Effect");
                    }
                    break;
            }
        }

        //private IEnumerator EffectCoroutine(float time)
        //{
            
        //    yield return new WaitForSeconds(time);

        //}

        #region 필살기 UI변경 메소드(나중에 string 파라미터 만들어서 하나로 메소드 만들기)

        private void ImageAniColorChange(int index)
        {
            anim.Play(AniArray[index]);
        }

        public void AnimationArray()
        {
            foreach (AnimationState state in anim)
            {
                AniArray.Add(state.name);
            }
        }

        #endregion

        public void SetEffectRPC(Types.Character c, int skillIndex, string whenPlaySE, CharacterController mrCharacter, CharacterController targetCharacter)
        {
            switch (c)
            {
                case Types.Character.Musa:
                    {
                        #region 무사 스킬 사운드
                        switch (skillIndex)
                        {
                            case 1:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "MoveSkill":
                                            {
                                            }
                                            break;
                                        case "NomalSkill":
                                            {
                                                SettingTarget(mrCharacter);
                                                ctnPosition.y += 1f;
                                                PrefabsSetting1(mrCharacter);
                                                LaunchEffectOn1("Musa Skill 1");
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "MoveSkill":
                                            {
                                                SettingTarget(mrCharacter);
                                                ctnPosition.y += 1f;
                                                PrefabsSetting1(mrCharacter);
                                                LaunchEffectOn1("Musa Skill 2");
                                            }
                                            break;
                                        case "NomalSkill":
                                            {
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "MoveSkill":
                                            {
                                            }
                                            break;
                                        case "NomalSkill":
                                            {
                                                ImageAniColorChange((int)AniColor.AniRed);
                                                SettingTarget(mrCharacter);
                                                StayEffectOn("Musa Skill 3");
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    break;
                #endregion 무사 스킬 사운드 끝
                case Types.Character.Thief:
                    {
                        #region 도적 스킬 사운드
                        switch (skillIndex)
                        {
                            case 1:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "MoveSkill":
                                            {
                                            }
                                            break;
                                        case "NomalSkill":
                                            {
                                                SettingTarget(mrCharacter);
                                                StayEffectOn("Thief Skill 1");
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "MoveSkill":
                                            {
                                                SettingTarget(targetCharacter);
                                                // ctnPosition.y += 0f;
                                                LaunchEffectOn1("Thief Skill 2");
                                            }
                                            break;
                                        case "NomalSkill":
                                            {
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "MoveSkill":
                                            {
                                            }
                                            break;
                                        case "NomalSkill":
                                            {
                                                ImageAniColorChange((int)AniColor.AniYellow);
                                                SettingTarget(mrCharacter);
                                                ctnPosition.y += 0.1f;
                                                PrefabsSetting1(mrCharacter);
                                                LaunchEffectOn1("Thief Skill 3");
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    break;
                #endregion 도적 스킬 사운드 끝
                case Types.Character.Archer:
                    {
                        #region 궁수 스킬 사운드
                        switch (skillIndex)
                        {
                            case 1:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "MoveSkill":
                                            {
        
                                            }
                                            break;
                                        case "NomalSkill":
                                            {
                                                SettingTarget(targetCharacter);
                                                ctnPosition.y += 3f;
                                                LaunchEffectOn2("Archer Skill 1");
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "MoveSkill":
                                            {
                                            }
                                            break;
                                        case "NomalSkill":
                                            {
                                                SettingTarget(mrCharacter);
                                                ctnPosition.y += 0.6f;
                                                PrefabsSetting1(mrCharacter);
                                                LaunchEffectOn1("Archer Skill 2");
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "MoveSkill":
                                            {
                                            }
                                            break;
                                        case "NomalSkill":
                                            {
                                                ImageAniColorChange((int)AniColor.AniGreen);
                                                SettingTarget(mrCharacter);
                                                StayEffectOn("Archer Skill 3 test");
                                                ctnPosition.y += 0.6f;

                                                PrefabsSetting1(mrCharacter);
                                                LaunchEffectOn1("Archer Skill 3");
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    break;
                #endregion 궁수 스킬 사운드 끝
                case Types.Character.Magician:
                    {
                        #region 법사 스킬 사운드
                        switch (skillIndex)
                        {
                            case 1:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "MoveSkill":
                                            {
                                            }
                                            break;
                                        case "NomalSkill":
                                            {
                                                SettingTarget(targetCharacter);
                                                //ctnPosition.y += 3f;
                                                LaunchEffectOn1("Magician Skill 1");
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "MoveSkill":
                                            {
                                            }
                                            break;
                                        case "NomalSkill":
                                            {
                                                SettingTarget(mrCharacter);
                                                StayEffectOn("Magician Skill 2 Field2");

                                                SettingTarget(targetCharacter);
                                                LaunchEffectOn1("Magician Skill 2");
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "MoveSkill":
                                            {
                                            }
                                            break;
                                        case "NomalSkill":
                                            {

                                                ImageAniColorChange((int)AniColor.AniBlue);
                                                SettingTarget(mrCharacter);
                                                StayEffectOn("Magician Skill 3");
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    break;
                    #endregion 법사 스킬 사운드 끝
            }
        }
    }
}
