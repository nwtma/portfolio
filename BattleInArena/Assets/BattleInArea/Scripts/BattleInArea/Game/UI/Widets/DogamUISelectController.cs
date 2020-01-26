using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace BattleInArea.Game
{
    public class DogamUISelectController : Core.UI.Controller
    {
        Event.Selected evt;
        private int charCheck;
        [SerializeField] public GameObject ClickUI;

        //스토리 설명 페이지 이미지
        [SerializeField] public GameObject storyImage;
        [SerializeField] public GameObject[] storyImages;
        [SerializeField] public GameObject[] characterImages;
        [SerializeField] public GameObject[] nameTags;

        //스킬 설명 페이지 이미지
        [SerializeField] public GameObject skillImage;
        [SerializeField] public GameObject[] skillpageImages;


        [SerializeField] public GameObject[] AllSkillImages;

        [SerializeField] public GameObject[] musaSkillImages;
        [SerializeField] public GameObject[] ThiefSkillImages;
        [SerializeField] public GameObject[] ArcherSkillImages;
        [SerializeField] public GameObject[] MagicianSkillImages;

        [SerializeField] private GameObject[] AllSkillList;

        public int CheckCount = 0;
        private bool IsSkillBase;
        private bool IsStoryBase;




        public enum CharacterCheck
        {
            Musacheck,
            Thiefcheck,
            Archercheck,
            Magiciancheck,
        }

        public override int ID
        {
            get { return (int)UI.ID.BookUIController; }
        }

        private void Start()
        {
            evt = Core.Event.Getter.Get<Event.Selected>();
        }

        public virtual void OnClickMusaStory()
        {
            SetStoryUI(Types.Character.Musa);
            charCheck = 0;
            ContentChange();
            IsStoryBase = true;
        }

        public virtual void OnClickThiefStory()
        {

            SetStoryUI(Types.Character.Thief);
            charCheck = 1;
            ContentChange();
            IsStoryBase = true;
        }

        public virtual void OnClickArcherStory()
        {
            SetStoryUI(Types.Character.Archer);
            charCheck = 2;
            ContentChange();
            IsStoryBase = true;
        }

        public virtual void OnClickMagicianStory()
        {
            SetStoryUI(Types.Character.Magician);
            charCheck = 3;
            ContentChange();
            IsStoryBase = true;
        }

        public void SetStoryUI(Types.Character c) // 처음 스토리페이지가 켜진다.!
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            switch (c)
            {
                case Types.Character.Musa:
                case Types.Character.Thief:
                case Types.Character.Archer:
                case Types.Character.Magician:
                    {
                        OffImage();
                        ClickUI.SetActive(true);
                        skillImage.SetActive(false);
                        storyImage.SetActive(true);

                        for (int i = 0, ii = storyImages.Length; ii > i; ++i)
                        {
                            characterImages[i].SetActive(false);
                            storyImages[i].SetActive(false);
                            nameTags[i].SetActive(false);
                        }

                        characterImages[(int)c].SetActive(true);
                        storyImages[(int)c].SetActive(true);
                        nameTags[(int)c].SetActive(true);


                    }
                    break;
            }
        }

        public void OnStoryImage() //스토리를 꺼줌
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            switch (charCheck)
            {
                case (int)Types.Character.Musa:
                case (int)Types.Character.Thief:
                case (int)Types.Character.Archer:
                case (int)Types.Character.Magician:
                    {
                        OffImage();
                        storyImage.SetActive(true);
                        skillImage.SetActive(false);

                        for (int i = 0, ii = skillpageImages.Length; ii > i; ++i)
                        {
                            storyImages[i].SetActive(false);
                        }

                        storyImages[charCheck].SetActive(true); // 스토리 페이지를 켜준다.
                        IsSkillBase = false;
                        IsStoryBase = true;


                    }
                    break;
            }
        }

        public void SetSkillUI() // 스킬을 켜줌
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            switch (charCheck)
            {
                case (int)Types.Character.Musa:
                case (int)Types.Character.Thief:
                case (int)Types.Character.Archer:
                case (int)Types.Character.Magician:
                    {
                        OffImage();
                        storyImage.SetActive(false);
                        skillImage.SetActive(true);

                        for (int i = 0, ii = skillpageImages.Length; ii > i; ++i)
                        {
                            skillpageImages[i].SetActive(false);
                        }

                        skillpageImages[charCheck].SetActive(true); // 스킬페이지를 켜준다.
                        IsStoryBase = false;
                        IsSkillBase = true;

                    }
                    break;
            }
        }

        public void checkcheck()
        {
            switch (charCheck)
            {
                case (int)Types.Character.Musa:
                case (int)Types.Character.Thief:
                case (int)Types.Character.Archer:
                case (int)Types.Character.Magician:
                    {

                        skillpageImages[charCheck].SetActive(true); // 스킬페이지를 켜준다.

                    }
                    break;
            }
        }

        public void OffImage() // 페이지를 꺼준다.
        {
            for (int i = 0, ii = storyImages.Length; i < ii; ++i)
            {
                storyImages[i].SetActive(false);
                skillpageImages[i].SetActive(false);
            }
        }

        public void SkillContentImageChange(GameObject[] skillImageList) // 캐릭터의 스킬 바꾸기
        {
            for (int i = 0; i < skillImageList.Length; i++)
            {
                AllSkillList[i] = skillImageList[i];
            }
        }
        public void ContentChange()
        {
            switch (charCheck)
            {
                case 0:
                    {
                        SkillContentImageChange(musaSkillImages);
                    }
                    break;
                case 1:
                    {
                        SkillContentImageChange(ThiefSkillImages);
                    }
                    break;
                case 2:
                    {
                        SkillContentImageChange(ArcherSkillImages);
                    }
                    break;
                case 3:
                    {
                        SkillContentImageChange(MagicianSkillImages);
                    }
                    break;
            }
        }

        public void dogamCharacterLeftClick()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            ContentChange();

            CharacterImageHide();
            CharacterImageLeftShow();
        }

        public void dogamCharacterRightClick()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            ContentChange();

            CharacterImageHide();
            CharacterImageRightShow();
        }

        public void CharacterImageHide() // 캐릭터 이미지 끄기
        {
            for (int i = 0; i < characterImages.Length; i++)
            {
                characterImages[i].SetActive(false);
                nameTags[i].SetActive(false);
                storyImages[i].SetActive(false);
                skillpageImages[i].SetActive(false);
            }
        }
        public void CharRightCheck()
        {
            if (charCheck < 3)
            {
                charCheck += 1;
            }
            else if (charCheck >= 3)
            {
                charCheck = 0;
            }
        }
        public void CharLeftCheck()
        {

            if (charCheck > 0)
            {
                charCheck -= 1;
            }
            else if (charCheck <= 0)
            {
                charCheck = 3;
            }
        }
        public int GetcharCheck()
        {
            return charCheck;
        }
        public void CharacterImageLeftShow()
        {
            CharLeftCheck();
            ContentChange();

            if (IsStoryBase)
            {
                storyImages[charCheck].SetActive(true);
            }
            else if (IsSkillBase)
            {
                skillpageImages[charCheck].SetActive(true);
            }
            characterImages[charCheck].SetActive(true);
            nameTags[charCheck].SetActive(true);

            Debug.Log(charCheck);
        }

        public void CharacterImageRightShow()
        {
            CharRightCheck();
            ContentChange();

            if (IsStoryBase)
            {
                storyImages[charCheck].SetActive(true);
            }
            else if (IsSkillBase)
            {
                skillpageImages[charCheck].SetActive(true);
            }
            characterImages[charCheck].SetActive(true);
            nameTags[charCheck].SetActive(true);

        }
        //-----------------------------------------------스킬 화살표 부분----------------------------------------------------

        public void dogamSkillLeftClick() // 왼쪽 화살표 클릭시
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            SkillContentImageHide(); // 이미지 끄기
            SkillImageLeftShow(); // 왼쪽 버튼을 눌렀을시 스킬이미지 키기
        }

        public void dogamSkillRightClick() // 오른쪽 화살표 클릭시
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            SkillContentImageHide(); // 이미지 끄기
            SkillImageRightShow(); // 오른쪽 버튼을 눌렀을시 스킬이미지 키기
        }

        public void SkillContentImageHide() // 스킬이미지 끄기
        {
            for (int i = 0; i < AllSkillList.Length; i++)
            {
                AllSkillList[i].SetActive(false);
            }
        }

        public void SkillImageLeftShow()  // 스킬 왼쪽 화살표 클릭시 스킬설명을 보여줌
        {
            if (CheckCount > 0)
                CheckCount--;

            else
                CheckCount = 2;

            AllSkillList[CheckCount].SetActive(true);

        }

        public void SkillImageRightShow() // 스킬 오른쪽 화살표 클릭시 스킬설명을 보여줌
        {
            if (CheckCount < 2)
                CheckCount++;
            else
                CheckCount = 0;

            AllSkillList[CheckCount].SetActive(true);

        }

        public void Back()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.CreditButton;
            Core.Event.Dispatcher.Dispatch(evt);
        }

        public void DogamBack()
        {
            // 클릭시 뒤로가기 버튼을 만들어준다.(도감 설명참을 나가는 걸로)
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.CreditButton;
            Core.Event.Dispatcher.Dispatch(evt);

        }

        public void DogamContentUIBack()
        {
            ClickUI.SetActive(false);
        }

    }
}