using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;

namespace BattleInArea.Game
{

    public partial class CharacterController : Photon.PunBehaviour
    {
        public class CharacterState : Core.State
        {
            protected CharacterController ctn { private set; get; }

            public CharacterState(CharacterController ctn, string newKey, params string[] newTransition) : base(newKey, newTransition)
            {
                this.ctn = ctn;
            }
        }

        public GameObject Mesh;
        public GameObject Body;
        public GameObject selectedEffect;
        public GameObject myTurn;
        public GameObject targetTurn;

        [SerializeField]
        protected Text damageText;
        public SkeletonAnimation skeletonAnimation;
        protected Core.StateMachine stateMachine = new Core.StateMachine();
        protected bool isPlayAnimation;


        public Event.GameResult evt2;
        public Event.OnEvent et;
        private Event.Selected evt;
        public int AP = 0;
        public int HP = 0;

        public int x;
        public int z;

        public int MoveTileX, MoveTileZ;
        public int SkillTileX, SkillTileZ;

        public int selectedSkillIndex;

        protected bool isPlayEffect;

        public PhotonPlayer MyPhotonPlayer
        {
            private set;
            get;
        }

        public bool isSelected;
        public bool IsSelected
        {
            get { return selectedEffect.activeSelf; }
        }

        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        string[] listsName = new string[3];

        public List<Tile> movableTiles = new List<Tile>();
        public List<List<Tile>> skillLists = new List<List<Tile>>();
        public List<Tile> skill01Tiles = new List<Tile>();
        public List<Tile> skill02Tiles = new List<Tile>();
        public List<Tile> skill03Tiles = new List<Tile>();
        public float animationtime = 0;
        public bool animationend;
        public bool usedShield;

        public GameObject prefabs_Floating_text;
        public GameObject Target;
        public GameObject parent;

        public GameObject EffectTarget;

        public bool IsMyCharacter
        {
            get
            {
                if (PhotonNetwork.player.ID == PlayerID) // 포톤 네트워킹의 플레이어 아이디랑 클라이언트의 플레이어의 아이디랑 다르다는 뜻?
                {
                    return true;
                }

                return false;
            }
        }

        [SerializeField]
        private Types.Character characterType;
        public Types.Character CharacterType
        {
            get { return characterType; }
        }

        public int PlayerID
        {
            get { return MyPhotonPlayer.ID; }
        }

        private void Awake()
        {
            evt2 = new Event.GameResult();
            evt = new Event.Selected();
            et = new Event.OnEvent();
            skillLists.Add(skill01Tiles);
            skillLists.Add(skill02Tiles);
            skillLists.Add(skill03Tiles);
            SetListName();
            OnSkillTrigger();
            skeletonAnimation = gameObject.GetComponentInChildren<SkeletonAnimation>();
            stateMachine.Add(new Idle(this));
            stateMachine.Add(new Nomal(this));
            stateMachine.Add(new Move(this, Idle.MyKey));
            stateMachine.Add(new UsingSkill(this, Idle.MyKey));
            stateMachine.Add(new Hit(this, Idle.MyKey));
            stateMachine.Add(new UsingMoveSkill(this, Idle.MyKey));
            stateMachine.Add(new Miss(this, Idle.MyKey));
            stateMachine.Add(new MoveSkillMiss(this, Idle.MyKey));
            stateMachine.Add(new Death(this, Nomal.MyKey));
        }

        private void Start()
        {
            Debug.Log(this.characterType);
            SetStatus(this.characterType);
        }

        /// <summary>
        /// Called on all scripts on a GameObject (and children) that have been Instantiated using PhotonNetwork.Instantiate.
        /// </summary>
        /// <remarks>
        /// PhotonMessageInfo parameter provides info about who created the object and when (based off PhotonNetworking.time).
        /// </remarks>
        public override void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            MyPhotonPlayer = info.sender;

            Character.Manager.Add(this);
            Prepare();

            stateMachine.Change(Idle.MyKey);
        }
        //#if UNITY_EDITOR

        public Types.Character debugCharacterType;
        public int debugPlayerID;

        protected void Update()
        {
            stateMachine.Update();

            debugCharacterType = characterType;
            debugPlayerID = PlayerID;

            //animationtime += Time.deltaTime;
            //animationend = animation.timeScale <= animationtime;
            //if(animationend )
            //{ 
            //    SetAnimation("idle", true, 2.0f);
            //}
        }
        //#endif

        public void OnMyturn()
        {
            myTurn.SetActive(true);
        }
        public void OntargetTurn()
        {
            targetTurn.SetActive(true);
        }
        public void OffMyturn()
        {
            myTurn.SetActive(false);
        }
        public void OfftargetTurn()
        {
            targetTurn.SetActive(false);
        }

        public Vector3 CharPos()
        {
            if(this == null) { return Vector3.zero; }
            return this.transform.position;
        }
        public void SetStatus(Types.Character type)
        {
            switch (type)
            {
                case Types.Character.Musa:
                    {
                        HP = CharacterData.instance.GetData("Musa").hp;
                        AP = CharacterData.instance.GetData("Musa").ap;
                    }
                    break;
                case Types.Character.Archer:
                    {
                        HP = CharacterData.instance.GetData("Archer").hp;
                        AP = CharacterData.instance.GetData("Archer").ap;
                    }
                    break;
                case Types.Character.Thief:
                    {
                        HP = CharacterData.instance.GetData("Thief").hp;
                        AP = CharacterData.instance.GetData("Thief").ap;
                    }
                    break;
                case Types.Character.Magician:
                    {
                        HP = CharacterData.instance.GetData("Magician").hp;
                        AP = CharacterData.instance.GetData("Magician").ap;
                    }
                    break;
            }
        }
        public void Add(Tile t)
        {
            movableTiles.Add(t);
        }

        public void Remove(Tile t)
        {
            movableTiles.Remove(t);
        }

        public List<Tile> GetMovableTiles()
        {
            return movableTiles;
        }

        public bool IsMovableTile(int x, int z)
        {
            for (int i = 0, ii = movableTiles.Count; ii > i; ++i)
            {
                if (movableTiles[i].X == x && movableTiles[i].Z == z)
                {
                    return true;
                }
            }

            return false;
        }

        public void SkillAdd(string name, Tile t)
        {
            for (int i = 0; i < skillLists.Count; ++i)
            {
                if (name == listsName[i])
                {
                    skillLists[i].Add(t);
                }
            }
        }

        public void SkillRemove()
        {
            for (int i = 0; i < skillLists.Count; ++i)
            {
                skillLists[i].Clear();
            }
        }
        public List<Tile> GetSkillTiles(string name)
        {
            for (int i = 0; i < skillLists.Count; ++i)
            {
                if (name == listsName[i])
                {
                    return skillLists[i];
                }
            }
            return null;
        }
        public Tile GetRightSkillTiles(string name)
        {
            for (int i = 0; i < this.GetSkillTiles(name).Count; ++i)
            {
                if (CharPos().z < this.GetSkillTiles(name)[i].Z)
                {
                    return this.GetSkillTiles(name)[i];
                }
            }
            return null;
        }
        public Tile GetLeftSkillTiles(string name)
        {
            for (int i = 0; i < this.GetSkillTiles(name).Count; ++i)
            {
                if (CharPos().z > this.GetSkillTiles(name)[i].Z)
                {
                    return this.GetSkillTiles(name)[i];
                }
            }
            return null;
        }
        public bool IsSkillTile(int x, int z, string name)
        {
            for (int i = 0; i < GetSkillTiles(name).Count;++i)
            {
                if(GetSkillTiles(name)[i].X == x && GetSkillTiles(name)[i].Z == z)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsEnemyInTiles(string name, Vector3 enemyPos)
        {
            for (int i = 0; i < this.GetSkillTiles(name).Count; ++i)
            {
                if (enemyPos == this.GetSkillTiles(name)[i].transform.position)
                {
                    return true;
                }
            }

            return false;
        }

        public void OffSkillTrigger()
        {
            SkillTrigger[] skill = gameObject.GetComponentsInChildren<SkillTrigger>();
            for (int i = 0, ii = skill.Length; ii > i; ++i)
            {
                skill[i].Off();
            }
        }

        public void OnSkillTrigger()
        {
            SkillTrigger[] skill = gameObject.GetComponentsInChildren<SkillTrigger>();
            for (int i = 0, ii = skill.Length; ii > i; ++i)
            {
                skill[i].On();
            }
        }
        public void SetListName()
        {
            listsName[0] = "skill01Tiles";
            listsName[1] = "skill02Tiles";
            listsName[2] = "skill03Tiles";
        }

        public void Prepare()
        {
            Selectable selectable = gameObject.GetComponent<Selectable>();

            if (selectable == null) return;    //AI 선택 안 되게 SelectedAble 뺐음 -> selectedable null일 경우 리턴

            selectable.fun = OnSelected;
        }


        public void SetSelectedSkillIndex(int idx)
        {
            selectedSkillIndex = idx;
        }

        public void SetMoveTile(int x, int z)
        {
            this.MoveTileX = x;
            this.MoveTileZ = z;
        }

        public void SetSkillTile(int x, int z)
        {
            this.SkillTileX = x;
            this.SkillTileZ = z;
        }

        public void SetPosition(int x, int z)
        {
            transform.position = new Vector3(x, 0, z);

            this.x = x;
            this.z = z;
        }

        public void OnSelected()
        {
            isSelected = true;
            OffSkillTrigger();
            evt.hashTable["select_type"] = Event.SelectedType.Character;
            Core.Event.Dispatcher.Dispatch(evt);
        }

        public void OffCollider()
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        public void OnCollider()
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }

        public void ClearMovableTileList()
        {
            movableTiles.Clear();
        }

        public void OffMovableTrigger()
        {
            MovableTrigger[] triggers = gameObject.GetComponentsInChildren<MovableTrigger>();
            for (int i = 0, ii = triggers.Length; ii > i; ++i)
            {
                triggers[i].Off();
            }
        }

        public void OnMovableTrigger()
        {
            MovableTrigger[] triggers = gameObject.GetComponentsInChildren<MovableTrigger>();
            for (int i = 0, ii = triggers.Length; ii > i; ++i)
            {
                triggers[i].On();
            }
        }


        /// <summary>
        /// AP를 소모합니다
        /// </summary>
        /// <returns></returns>
        public void ConsumeAP(int consumeCount)
        {
            AP -= consumeCount;

            if (AP <= 0)
            {
                AP = 0;
                et.hashTable["event_type"] = Event.EventType.AllUsedAP;
                Core.Event.Dispatcher.Dispatch(et);

            }
        }
        public void NextTurnAP()
        {
            switch (this.CharacterType)
            {
                case Types.Character.Musa:
                    {
                        if(AP + CharacterData.instance.GetData("Musa").turnap > CharacterData.instance.GetData("Musa").maxap)
                        {
                            AP = CharacterData.instance.GetData("Musa").maxap;
                            return;
                        }
                        AP = AP + CharacterData.instance.GetData("Musa").turnap;
                    }
                    break;
                case Types.Character.Archer:
                    {
                        if (AP + CharacterData.instance.GetData("Archer").turnap > CharacterData.instance.GetData("Archer").maxap)
                        {
                            AP = CharacterData.instance.GetData("Archer").maxap;
                            return;
                        }
                        AP = AP + CharacterData.instance.GetData("Archer").turnap;
                    }
                    break;
                case Types.Character.Thief:
                    {
                        if (AP + CharacterData.instance.GetData("Thief").turnap > CharacterData.instance.GetData("Thief").maxap)
                        {
                            AP = CharacterData.instance.GetData("Thief").maxap;
                            return;
                        }
                        AP = AP + CharacterData.instance.GetData("Thief").turnap;
                    }
                    break;
                case Types.Character.Magician:
                    {
                        if (AP + CharacterData.instance.GetData("Magician").turnap > CharacterData.instance.GetData("Magician").maxap)
                        {
                            AP = CharacterData.instance.GetData("Magician").maxap;
                            return;
                        }
                        AP = AP + CharacterData.instance.GetData("Magician").turnap;
                    }
                    break;
            }
        }
        public void GiveDamage(int damage)
        {
            GiveDamage2(damage);

            if (!AliveCheck())
            {
                ChangeDeathState();
            }
        }

        public void GiveDamage2(int damage)
        {
           // damageText.text = damage.ToString();
            HP -= damage;
        }

        public bool AliveCheck()
        {
            if (HP <= 0)
            {
                HP = 0;
                return false;
            }
            return true;
        }
        public void GiveHeal(int heal)
        {
            switch (this.CharacterType)
            {
                case Types.Character.Musa:
                    {
                        if (HP + heal > CharacterData.instance.GetData("Musa").hp)
                        {
                            HP = CharacterData.instance.GetData("Musa").hp;
                            return;
                        }
                        HP = HP += heal;
                    }
                    break;
                case Types.Character.Archer:
                    {
                        if (HP + heal > CharacterData.instance.GetData("Archer").hp)
                        {
                            HP = CharacterData.instance.GetData("Archer").hp;
                            return;
                        }
                        HP = HP += heal;
                    }
                    break;
                case Types.Character.Thief:
                    {
                        if (HP + heal > CharacterData.instance.GetData("Thief").hp)
                        {
                            HP = CharacterData.instance.GetData("Thief").hp;
                            return;
                        }
                        HP = HP += heal;
                    }
                    break;
                case Types.Character.Magician:
                    {
                        if (HP + heal > CharacterData.instance.GetData("Magician").hp)
                        {
                            HP = CharacterData.instance.GetData("Magician").hp;
                            return;
                        }
                        HP = HP += heal;
                    }
                    break;
            }
        }
        public void GiveAP(int heal)
        {
            switch (this.CharacterType)
            {
                case Types.Character.Musa:
                    {
                        if (AP + heal > CharacterData.instance.GetData("Musa").maxap)
                        {
                            AP = CharacterData.instance.GetData("Musa").maxap;
                            return;
                        }
                        AP = AP += heal;
                    }
                    break;
                case Types.Character.Archer:
                    {
                        if (AP + heal > CharacterData.instance.GetData("Archer").maxap)
                        {
                            AP = CharacterData.instance.GetData("Archer").maxap;
                            return;
                        }
                        AP = AP += heal;
                    }
                    break;
                case Types.Character.Thief:
                    {
                        if (AP + heal > CharacterData.instance.GetData("Thief").maxap)
                        {
                            AP = CharacterData.instance.GetData("Thief").maxap;
                            return;
                        }
                        AP = AP += heal;
                    }
                    break;
                case Types.Character.Magician:
                    {
                        if (AP + heal > CharacterData.instance.GetData("Magician").maxap)
                        {
                            AP = CharacterData.instance.GetData("Magician").maxap;
                            return;
                        }
                        AP = AP += heal;
                    }
                    break;
            }
        }
        /// <summary>
        /// AP 가 남아 있습니가?
        /// </summary>
        /// <returns></returns>
        public bool IsRemainAP()
        {
            return AP > 0;
        }

        public int RemainAP()
        {
            return AP;
        }
        public int RemainHP()
        {
            return HP;
        }

        public void PlayAnimation(string name, bool loop, float timeScale = 1f)
        {
            isPlayAnimation = true;

            skeletonAnimation.AnimationState.Complete += OnCompleted;
            // skeletonAnimation.state.End = OnEnded;
            skeletonAnimation.state.SetAnimation(0, name, loop).TimeScale = timeScale;
        }

        public bool IsPlayAnimation()
        {
            return isPlayAnimation;
        }

        
        public void OnCompleted(TrackEntry trackEntry)
        {
            Debug.LogFormat("log ! : {0} OnEnded",  trackEntry.Animation.Name);

            skeletonAnimation.AnimationState.Complete -= OnCompleted;
            isPlayAnimation = false;
        }

        public void SetAnimation(string name, bool loop, float speed)
        {
            animationtime = 0f;
            animationend = false;
            skeletonAnimation.state.SetAnimation(0, name, loop).TimeScale = speed;
        }

        public void ChangeMoveState()
        {
            stateMachine.Change(Move.MyKey);
        }

        public void ChangeUsingSkillState()
        {
            stateMachine.Change(UsingSkill.MyKey);
        }
        
        public void ChangeUsingMoveSkillState()
        {
            stateMachine.Change(UsingMoveSkill.MyKey);
        }

        public void ChangeHitState()
        {
            stateMachine.Change(Hit.MyKey);
        }
        public void ChangeMissState()
        {
            stateMachine.Change(Miss.MyKey);
        }
        public void ChangeMoveSkillMissState()
        {
            stateMachine.Change(MoveSkillMiss.MyKey);
        }
        public void ChangeDeathState()
        {
            stateMachine.Change(Death.MyKey);
        }
        public void FloatingTextOnEnter(int damage)                     //데미지 뜨는 플로팅 텍스트
        {
            Vector3 vector = this.Target.transform.position;

            vector.x += 0;
            vector.y += 2;
            vector.z += (float)0.5;


            GameObject clone = Instantiate(prefabs_Floating_text, vector, Quaternion.Euler(0,-90,0));
            clone.GetComponentInChildren<FloatingText>().Floatingtext.text = "-" + damage.ToString();

            clone.GetComponentInChildren<FloatingText>().Floatingtext.color = Color.red;

            clone.GetComponentInChildren<FloatingText>().Floatingtext.fontSize = 40;
            //clone.transform.SetParent(parent.transform);
        }

        public void FloatingTextOnEnter2(int ApHeal)                     //AP 뜨는 플로팅 텍스트
        {
            Vector3 vector = this.Target.transform.position;

            vector.x += 0;
            vector.y += 2;
            vector.z += (float)0.5;


            GameObject clone = Instantiate(prefabs_Floating_text, vector, Quaternion.Euler(0, -90, 0));
            clone.GetComponentInChildren<FloatingText>().Floatingtext.text = "+" + ApHeal.ToString();

            clone.GetComponentInChildren<FloatingText>().Floatingtext.color = Color.cyan;

            clone.GetComponentInChildren<FloatingText>().Floatingtext.fontSize = 40;
            //clone.transform.SetParent(parent.transform);
        }

        public void FloatingTextOnEnter3(int Heal)                     //Heal 뜨는 플로팅 텍스트
        {
            Vector3 vector = this.Target.transform.position;

            vector.x += 0;
            vector.y += 2;
            vector.z += (float)0.5;


            GameObject clone = Instantiate(prefabs_Floating_text, vector, Quaternion.Euler(0, -90, 0));
            clone.GetComponentInChildren<FloatingText>().Floatingtext.text = "+" + Heal.ToString();

            clone.GetComponentInChildren<FloatingText>().Floatingtext.color = Color.green;

            clone.GetComponentInChildren<FloatingText>().Floatingtext.fontSize = 40;
            //clone.transform.SetParent(parent.transform);
           
        }

    }
}
