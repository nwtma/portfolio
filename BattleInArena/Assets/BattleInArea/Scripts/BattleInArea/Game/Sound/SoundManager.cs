using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattleInArea.Game
{
    public class SoundManager : MonoBehaviour
    {
        static public SoundManager instance;

        [SerializeField]
        protected List<AudioSource> BgmBoxList = new List<AudioSource>();
        [SerializeField]
        protected List<AudioSource> EffectBoxList = new List<AudioSource>();
        protected AudioClip Sound_Name;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }
        void Update()
        {
        }

        public void PlayOneShotSE(string Sound_Name)
        {
            this.Sound_Name = Instantiate(Resources.Load<AudioClip>(Sound_Name));
            for (int i = 0; i < EffectBoxList.Count; i++)
            {
                if (!EffectBoxList[i].isPlaying)
                {
                    EffectBoxList[i].PlayOneShot(this.Sound_Name);
                    return;
                }
                Debug.Log("모든 AudioSource가 사용중입니다.");
            }
        }
        public void PlaySE(string Sound_Name)
        {
            this.Sound_Name = Instantiate(Resources.Load<AudioClip>(Sound_Name));
            for (int i = 0; i < BgmBoxList.Count; i++)
            {
                if (!BgmBoxList[i].isPlaying)
                {
                    BgmBoxList[i].clip = this.Sound_Name;
                    BgmBoxList[i].loop = true;
                    BgmBoxList[i].Play();

                    return;
                }
                Debug.Log("모든 AudioSource가 사용중입니다.");
            }
         }
        public void StopALLSE()
        {
            for (int i = 0; i < BgmBoxList.Count; i++)
            {
                BgmBoxList[i].Stop();
                BgmBoxList[i].loop = false;
                break;
            }
        }

        public void StopSE2(string sound_Name)
        {
            for (int i = 0; i < BgmBoxList.Count; i++)
            {
                if (BgmBoxList[i].clip.name == sound_Name)
                {
                    BgmBoxList[i].Stop();
                    BgmBoxList[i].loop = false;
                    break;
                }
                else
                {
                    return;
                }
            }
        }
        public void StopSE(string Sound_Name)
        {
            for (int i = 0; i < BgmBoxList.Count; i++)
            {
                if (BgmBoxList[i].clip.name == Sound_Name)
                {
                    BgmBoxList[i].Stop();
                    BgmBoxList[i].loop = false;
                    break;
                }
                else
                {
                    return;
                }
            }
            Debug.Log("재생 중인" + Sound_Name + "사운드가 없습니다.");
        }
        public void BgmMute(GameObject Mute, GameObject UnMute)
        {
            for (int i = 0; i < BgmBoxList.Count; ++i)
            {
                if(BgmBoxList[i].mute == false)
                {
                    UnMute.SetActive(false);
                    Mute.SetActive(true);
                    BgmBoxList[i].mute = true;
                }
                else
                {
                    Mute.SetActive(false);
                    UnMute.SetActive(true);
                    BgmBoxList[i].mute = false;
                }
            }
        }
        public void EffectMute(GameObject Mute, GameObject UnMute)
        {
            for (int i = 0; i < EffectBoxList.Count; ++i)
            {
                if (EffectBoxList[i].mute == false)
                {
                    UnMute.SetActive(false);
                    Mute.SetActive(true);
                    EffectBoxList[i].mute = true;
                }
                else
                {
                    Mute.SetActive(false);
                    UnMute.SetActive(true);
                    EffectBoxList[i].mute = false;
                }
            }
        }
        public void SetBgmSlider(Slider slider)
        {
            for (int i = 0; i < BgmBoxList.Count; ++i)
            {
                BgmBoxList[i].volume = slider.value;
            }
        }
        public void SetEffectSlider(Slider slider)
        {
            for (int i = 0; i < EffectBoxList.Count; ++i)
            {
                EffectBoxList[i].volume = slider.value;
            }
        }
        public void MovePlaySE()
        {
            Helper.RPC.Send("RpcPlaySE", PhotonTargets.AllBuffered, "Sound/Effect/State/Female/Move/MoveEffect");
        }
        public void MoveStopSE(string soundName)
        {
            Helper.RPC.Send("RpcStopSE", PhotonTargets.AllBuffered, soundName);
        }
        public void SetHitSound(Types.Character c)
        {
            switch(c)
            {
                case Types.Character.Musa:
                    {
                        Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/State/Female/Hit/HitEffect");
                    }
                    break;
                case Types.Character.Thief:
                    {
                        Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/State/Female/Hit/HitEffect");
                    }
                    break;
                case Types.Character.Archer:
                    {
                        Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/State/Male/Hit/HitEffect");
                    }
                    break;
                case Types.Character.Magician:
                    {
                        Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/State/Male/Hit/HitEffect");
                    }
                    break;

            }
        }
        public void SetSoundRPC(Types.Character c, int skillIndex, string whenPlaySE)
        {
            switch(c)
            {
                case Types.Character.Musa:
                    {
                        #region 무사 스킬 사운드
                        switch (skillIndex)
                        {
                            case 1:
                                {
                                    switch(whenPlaySE)
                                    {
                                        case "Enter":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Warrior/Skill01/Skill01Effect");
                                            }
                                            break;
                                        case "Exit":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Warrior/SkillHit/SkillHit");                               
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "Enter":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Warrior/Skill02/Skill02Effect");
                                            }
                                            break;
                                        case "Exit":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Warrior/SkillHit/SkillHit");
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "Enter":
                                            {
                                                //Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Warrior/Skill03/Skill03Effect");
                                            }
                                            break;
                                        case "Exit":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Warrior/SkillHit/SkillHit");
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
                                        case "Enter":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Thief/Skill01/Skill01Effect");
                                            }
                                            break;
                                        case "Exit":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Thief/SkillHit/SkillHit");
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "Enter":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Thief/Skill02/Skill02Effect");
                                            }
                                            break;
                                        case "Exit":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Thief/SkillHit/SkillHit");
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "Enter":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Thief/Skill03/Skill03Effect");
                                            }
                                            break;
                                        case "Exit":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Thief/SkillHit/SkillHit");
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
                                        case "Enter":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Archer/Skill01/ArrowUp");
                                            }
                                            break;
                                        case "Turm":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Archer/Skill01/ArrowDown");
                                            }
                                            break;
                                        case "Exit":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Archer/SkillHit/SkillHit");
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "Enter":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Archer/Skill02/Skill02Effect");
                                            }
                                            break;
                                        case "Exit":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Archer/SkillHit/SkillHit");
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "Enter":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Archer/Skill03/Skill03ShotEffect");
                                            }
                                            break;
                                        case "Exit":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Archer/SkillHit/SkillHit");
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
                                        case "Enter":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Magician/Skill01/Skill01Effect");
                                            }
                                            break;
                                        case "Exit":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Magician/SkillHit/SkillHit");
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "Enter":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Magician/Skill02/Skill02Effect");
                                            }
                                            break;
                                        case "Exit":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Magician/Skill02/Skill02Hit");
                                            }
                                            break;
                                    }
                                }
                                break;
                            case 3:
                                {
                                    switch (whenPlaySE)
                                    {
                                        case "Enter":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Magician/Skill03/Skill03Prepare");
                                            }
                                            break;
                                        case "Exit":
                                            {
                                                Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/Magician/Skill03/Skill03Effect");
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

        public void SetDeathSound(Types.Character c)
        {
            switch(c)
            {
                case Types.Character.Musa:
                    {
                        Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/State/Female/Death/DeathEffect");
                    }
                    break;
                case Types.Character.Thief:
                    {
                        Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/State/Female/Death/DeathEffect");
                    }
                    break;
                case Types.Character.Archer:
                    {
                        Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/State/Male/Death/DeathEffect");
                    }
                    break;
                case Types.Character.Magician:
                    {
                        Helper.RPC.Send("RpcPlayOneShotSE", PhotonTargets.AllBuffered, "Sound/Effect/State/Male/Death/DeathEffect");
                    }
                    break;
            }
        }
    }
}
