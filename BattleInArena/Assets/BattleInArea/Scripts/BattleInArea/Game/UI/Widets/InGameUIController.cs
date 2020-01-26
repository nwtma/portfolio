using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
namespace BattleInArea.Game
{
    public class InGameUIController : Core.UI.Controller
    {
        public override int ID
        {
            get { return (int)UI.ID.IngameUIController; }
        }
        [SerializeField]
        public GameObject[] firstCharacterImage;

        [SerializeField]
        public GameObject[] firstCharacterSkill;

        [SerializeField]
        public GameObject[] secondCharacterImage;

        [SerializeField]
        public GameObject[] secondCharacterSkill;

        [SerializeField]
        public GameObject firstResultImage; //첫번째 플레이어 UI

        [SerializeField]
        public GameObject secondResultImage; //두번째 플레이어 UI

        [SerializeField]
        protected GameObject Escape;

        [SerializeField]
        protected Button[] firstSkillButton = new Button[3];
        [SerializeField]
        protected Button[] secondSkillButton = new Button[3];

        [SerializeField]
        protected Slider playerHpSlider;
        [SerializeField]
        protected Slider playerApSlider;
        [SerializeField]
        protected Slider enemyHpSlider;
        [SerializeField]
        protected Slider enemyApSlider;
        [SerializeField]
        public GameObject hpFull;
        [SerializeField]
        public GameObject apFull;
        [SerializeField]
        protected GameObject[] firstPlayerSlidersImages;
        [SerializeField]
        protected GameObject[] secondPlayerSlidersImages;

        protected bool isPlayerMax = false;
        protected bool isEnemyMax = false;
        protected Color myTurnColor = new Color32(255, 255, 255, 255);
        protected Color targetTurnColor = new Color32(90, 90, 90, 255);
        public GameObject changeTurnButton;
        public List<GameObject> itemList = new List<GameObject>();
        public List<Image> firstPlayerImage = new List<Image>();
        public List<Image> secondPlayerImage = new List<Image>();
        public GameObject firstPlayerItem;
        public GameObject secondPlayerItem;
        public GameObject APChecker;

        public Text firstPlayerHP;
        public Text secondPlayerHP;
        public Text firstPlayerAP;
        public Text secondPlayerAP;






        protected override void Awake()
        {
            base.Awake();
        }

        public void SetWinResultUI()
        {

            firstResultImage.SetActive(true);

        }

        public void SetDefeatResultUI()
        {

            secondResultImage.SetActive(true);

        }

        public void SetFirstCharacter(Types.Character c)
        {
            switch (c)
            {
                case Types.Character.Musa:
                case Types.Character.Magician:
                case Types.Character.Thief:
                case Types.Character.Archer:
                    {
                        for (int i = 0, ii = firstCharacterImage.Length; ii > i; ++i)
                        {
                            firstCharacterImage[i].SetActive(false);
                        }
                        firstCharacterImage[(int)c].SetActive(true);
                        SetFirstCharacterUIImage((int)c, firstCharacterImage);
                        SetFirstCharacterSliderImage();
                    }
                    break;
            }
        }

        public void SetFirstCharacterSKill(Types.Character c)
        {
            switch (c)
            {
                case Types.Character.Musa:
                case Types.Character.Magician:
                case Types.Character.Thief:
                case Types.Character.Archer:
                    {
                        for (int i = 0, ii = firstCharacterSkill.Length; ii > i; ++i)
                        {
                            firstCharacterSkill[i].SetActive(false);
                        }
                        firstCharacterSkill[(int)c].SetActive(true);
                        SetFirstCharacterUIImage((int)c, firstCharacterSkill);
                    }
                    break;
            }
        }

        public void SetSecondCharacter(Types.Character c)
        {
            switch (c)
            {
                case Types.Character.Musa:
                case Types.Character.Magician:
                case Types.Character.Thief:
                case Types.Character.Archer:
                    {
                        for (int i = 0, ii = secondCharacterImage.Length; ii > i; ++i)
                        {
                            secondCharacterImage[i].SetActive(false);
                        }
                        secondCharacterImage[(int)c].SetActive(true);
                        SetSecondCharacterUIImage((int)c, secondCharacterImage);
                        SetSecondCharacterSliderImage();
                    }
                    break;
            }
        }

        public void SetSecondCharacterSkill(Types.Character c)
        {
            switch (c)
            {
                case Types.Character.Musa:
                case Types.Character.Magician:
                case Types.Character.Thief:
                case Types.Character.Archer:
                    {
                        for (int i = 0, ii = secondCharacterSkill.Length; ii > i; ++i)
                        {
                            secondCharacterSkill[i].SetActive(false);
                        }
                        secondCharacterSkill[(int)c].SetActive(true);
                        SetSecondCharacterUIImage((int)c, secondCharacterSkill);
                    }
                    break;
            }
        }

        public void EscapeCheck()
        {
            if (Escape.activeSelf == true)
            {
                Escape.SetActive(false);
            }
            else
            {
                Escape.SetActive(true);
            }

        }
        public void EscapeNo()
        {
            Escape.SetActive(false);
        }

        public void SetFirstCharacterMax(CharacterController character)
        {
            switch (character.CharacterType)
            {
                case Types.Character.Musa:
                    {
                        playerHpSlider.maxValue = CharacterData.instance.GetData("Musa").hp;
                        playerApSlider.maxValue = CharacterData.instance.GetData("Musa").maxap;
                        isPlayerMax = true;
                    }
                    break;
                case Types.Character.Thief:
                    {
                        playerHpSlider.maxValue = CharacterData.instance.GetData("Thief").hp;
                        playerApSlider.maxValue = CharacterData.instance.GetData("Thief").maxap;
                        isPlayerMax = true;
                    }
                    break;
                case Types.Character.Archer:
                    {
                        playerHpSlider.maxValue = CharacterData.instance.GetData("Archer").hp;
                        playerApSlider.maxValue = CharacterData.instance.GetData("Archer").maxap;
                        isPlayerMax = true;
                    }
                    break;
                case Types.Character.Magician:
                    {
                        playerHpSlider.maxValue = CharacterData.instance.GetData("Magician").hp;
                        playerApSlider.maxValue = CharacterData.instance.GetData("Magician").maxap;
                        isPlayerMax = true;
                    }
                    break;
            }
        }
        public void SetSecondCharacterMax(CharacterController character)
        {
            while (true)
            {
                if (character != null)
                {
                    break;
                }
            }
            switch (character.CharacterType)
            {
                case Types.Character.Musa:
                    {
                        enemyHpSlider.maxValue = CharacterData.instance.GetData("Musa").hp;
                        enemyApSlider.maxValue = CharacterData.instance.GetData("Musa").maxap;
                        isEnemyMax = true;
                    }
                    break;
                case Types.Character.Thief:
                    {
                        enemyHpSlider.maxValue = CharacterData.instance.GetData("Thief").hp;
                        enemyApSlider.maxValue = CharacterData.instance.GetData("Thief").maxap;
                        isEnemyMax = true;
                    }
                    break;
                case Types.Character.Archer:
                    {
                        enemyHpSlider.maxValue = CharacterData.instance.GetData("Archer").hp;
                        enemyApSlider.maxValue = CharacterData.instance.GetData("Archer").maxap;
                        isEnemyMax = true;
                    }
                    break;
                case Types.Character.Magician:
                    {
                        enemyHpSlider.maxValue = CharacterData.instance.GetData("Magician").hp;
                        enemyApSlider.maxValue = CharacterData.instance.GetData("Magician").maxap;
                        isEnemyMax = true;
                    }
                    break;
            }
        }
        public void FirstSkillUpdate(CharacterController player)
        {
            for (int j = 0; j < firstCharacterSkill.Length; ++j)
            {
                if (firstCharacterSkill[j] != null)
                {
                    firstSkillButton = firstCharacterSkill[j].gameObject.GetComponentsInChildren<Button>();

                    for (int i = 0; i < firstSkillButton.Length; ++i)
                    {
                        if (player.AP >= CharacterData.instance.GetData(player.CharacterType.ToString()).skill01ap)
                        {
                            firstSkillButton[0].image.color = myTurnColor;
                        }
                        else
                        {
                            firstSkillButton[0].image.color = targetTurnColor;
                            firstSkillButton[1].image.color = targetTurnColor;
                            firstSkillButton[2].image.color = targetTurnColor;
                        }
                        if (player.AP >= CharacterData.instance.GetData(player.CharacterType.ToString()).skill02ap)
                        {
                            firstSkillButton[0].image.color = myTurnColor;
                            firstSkillButton[1].image.color = myTurnColor;
                        }
                        else
                        {
                            firstSkillButton[1].image.color = targetTurnColor;
                            firstSkillButton[2].image.color = targetTurnColor;
                        }
                        if (player.AP >= CharacterData.instance.GetData(player.CharacterType.ToString()).skill03ap)
                        {
                            firstSkillButton[0].image.color = myTurnColor;
                            firstSkillButton[1].image.color = myTurnColor;
                            firstSkillButton[2].image.color = myTurnColor;
                        }
                        else
                        {
                            firstSkillButton[2].image.color = targetTurnColor;
                        }
                    }
                }
            }
        }
        public void SecondSkillUpdate(CharacterController player)
        {
            for (int j = 0; j < secondCharacterSkill.Length; ++j)
            {
                if (secondCharacterSkill[j] != null)
                {
                    secondSkillButton = secondCharacterSkill[j].gameObject.GetComponentsInChildren<Button>();

                    for (int i = 0; i < secondSkillButton.Length; ++i)
                    {
                        if (player.AP >= CharacterData.instance.GetData(player.CharacterType.ToString()).skill01ap)
                        {
                            secondSkillButton[0].image.color = myTurnColor;
                        }
                        else
                        {
                            secondSkillButton[0].image.color = targetTurnColor;
                            secondSkillButton[1].image.color = targetTurnColor;
                            secondSkillButton[2].image.color = targetTurnColor;
                        }
                        if (player.AP >= CharacterData.instance.GetData(player.CharacterType.ToString()).skill02ap)
                        {
                            secondSkillButton[0].image.color = myTurnColor;
                            secondSkillButton[1].image.color = myTurnColor;
                        }
                        else
                        {
                            secondSkillButton[1].image.color = targetTurnColor;
                            secondSkillButton[2].image.color = targetTurnColor;
                        }
                        if (player.AP >= CharacterData.instance.GetData(player.CharacterType.ToString()).skill03ap)
                        {
                            secondSkillButton[0].image.color = myTurnColor;
                            secondSkillButton[1].image.color = myTurnColor;
                            secondSkillButton[2].image.color = myTurnColor;
                        }
                        else
                        {
                            secondSkillButton[2].image.color = targetTurnColor;
                        }
                    }
                }
            }
        }
        public void SecondSkillEnable(string value)
        {
            for (int j = 0; j < secondCharacterSkill.Length; ++j)
            {
                if (secondCharacterSkill[j] != null)
                {
                    secondSkillButton = secondCharacterSkill[j].gameObject.GetComponentsInChildren<Button>();

                    for (int i = 0; i < secondSkillButton.Length; ++i)
                    {
                        switch (value)
                        {
                            case "true":
                                {
                                    secondSkillButton[i].enabled = true;
                                }
                                break;
                            case "false":
                                {
                                    secondSkillButton[i].enabled = false;
                                }
                                break;
                        }
                    }
                }
            }
        }

        public void FirstSkillEnable(string value)
        {
            for (int j = 0; j < firstCharacterSkill.Length; ++j)
            {
                if (firstCharacterSkill[j] != null)
                {
                    firstSkillButton = firstCharacterSkill[j].gameObject.GetComponentsInChildren<Button>();

                    for (int i = 0; i < firstSkillButton.Length; ++i)
                    {
                        switch (value)
                        {
                            case "true":
                                {
                                    firstSkillButton[i].enabled = true;
                                }
                                break;
                            case "false":
                                {
                                    firstSkillButton[i].enabled = false;
                                }
                                break;
                        }
                    }
                }
            }
        }

        public void FirstItemEnable(string value)
        {
            Button[] button = firstPlayerItem.GetComponentsInChildren<Button>();
            for (int i = 0; i < button.Length; ++i)
            {
                if (button[i] != null)
                {
                    switch (value)
                    {
                        case "true":
                            {
                                button[i].enabled = true;
                            }
                            break;
                        case "false":
                            {
                                button[i].enabled = false;
                            }
                            break;
                    }
                }
            }
        }
        public void SecondItemEnable(string value)
        {
            Button[] button = secondPlayerItem.GetComponentsInChildren<Button>();
            for (int i = 0; i < button.Length; ++i)
            {
                if (button[i] != null)
                {
                    switch (value)
                    {
                        case "true":
                            {
                                button[i].enabled = true;
                            }
                            break;
                        case "false":
                            {
                                button[i].enabled = false;
                            }
                            break;
                    }
                }
            }
        }
        public void Skill01()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            Event.Selected evt = Core.Event.Getter.Get<Event.Selected>();
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.SkillButton;
            evt.hashTable["select_skill_id"] = Types.SkillButton.Skill01;

            Core.Event.Dispatcher.Dispatch(evt);
        }
        public void Skill02()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            Event.Selected evt = Core.Event.Getter.Get<Event.Selected>();
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.SkillButton;
            evt.hashTable["select_skill_id"] = Types.SkillButton.Skill02;

            Core.Event.Dispatcher.Dispatch(evt);
        }
        public void Skill03()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            Event.Selected evt = Core.Event.Getter.Get<Event.Selected>();
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.SkillButton;
            evt.hashTable["select_skill_id"] = Types.SkillButton.Skill03;

            Core.Event.Dispatcher.Dispatch(evt);
        }

        public void SetSlider(CharacterController player, CharacterController enemy)
        {
            playerHpSlider.maxValue = player.RemainHP();
            playerApSlider.maxValue = player.RemainAP();
            enemyHpSlider.maxValue = enemy.RemainHP();
            enemyApSlider.maxValue = enemy.RemainAP();
        }

        public void UpdatePlayerHpSlider(CharacterController player)
        {
            if (isPlayerMax == true)
            {
                playerHpSlider.value = player.RemainHP();
                firstPlayerHP.text = "(" + playerHpSlider.value.ToString() + "/" + playerHpSlider.maxValue.ToString() + ")";
            }
        }

        public void UpdatePlayerApSlider(CharacterController player)
        {
            if (isPlayerMax == true)
            {
                playerApSlider.value = player.RemainAP();
                firstPlayerAP.text = "(" + playerApSlider.value.ToString() + "/" + playerApSlider.maxValue.ToString() + ")";
            }
        }
        public void UpdateTargetHpSlider(CharacterController player)
        {
            if (isEnemyMax == true)
            {
                enemyHpSlider.value = player.RemainHP();
                secondPlayerHP.text = "(" + enemyHpSlider.value.ToString() + "/" + enemyHpSlider.maxValue.ToString() + ")";
            }
        }

        public void UpdateTargetApSlider(CharacterController player)
        {
            if (isEnemyMax == true)
            {
                enemyApSlider.value = player.RemainAP();
                secondPlayerAP.text = "(" + enemyApSlider.value.ToString() + "/" + enemyApSlider.maxValue.ToString() + ")";
            }
        }

        public void ChanageTurn()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            Event.Selected evt = Core.Event.Getter.Get<Event.Selected>();
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.ChangeTurnButton;

            Core.Event.Dispatcher.Dispatch(evt);
        }
        public void SetFirstCharacterUIImage(int inx, GameObject[] images) // 캐릭터 스킬 UI에 들어가 있는 이미지를 ON
        {
            Image[] a = images[inx].GetComponentsInChildren<Image>();

            for (int i = 0; i < a.Length; ++i)
            {
                firstPlayerImage.Add(a[i]);
            }

        }
        public void SetSecondCharacterUIImage(int inx, GameObject[] images) // 캐릭터 스킬 UI에 들어가 있는 이미지를 ON
        {
            Image[] a = images[inx].GetComponentsInChildren<Image>();

            for (int i = 0; i < a.Length; ++i)
            {
                secondPlayerImage.Add(a[i]);
            }
        }
        public void SetFirstCharacterSliderImage()
        {
            for (int i = 0; i < firstPlayerSlidersImages.Length; ++i)
            {
                firstPlayerImage.Add(firstPlayerSlidersImages[i].GetComponentInChildren<Image>());
            }
            firstPlayerImage.Add(firstPlayerItem.GetComponentInChildren<Image>());
        }
        public void SetSecondCharacterSliderImage()
        {
            for (int i = 0; i < firstPlayerSlidersImages.Length; ++i)
            {
                secondPlayerImage.Add(secondPlayerSlidersImages[i].GetComponentInChildren<Image>());
            }
            secondPlayerImage.Add(secondPlayerItem.GetComponentInChildren<Image>());

        }

        public void SetColorDark(List<Image> image)
        {
            for (int i = 0; i < image.Count; ++i)
            {
                if (image[i] != null)
                {
                    image[i].color = targetTurnColor;
                }
            }
        }
        public void SetColorWhite(List<Image> image)
        {
            for (int i = 0; i < image.Count; ++i)
            {
                if (image[i] != null)
                {
                    image[i].color = myTurnColor;
                }
            }
        }
        public void ApTrue()
        {
            if (apFull.activeSelf == false)
            {
                SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
                apFull.SetActive(true);
                Invoke("ApFalse", 1.5f);
            }
        }
        public void ApFalse()
        {
            apFull.SetActive(false);
        }
        public void HpTrue()
        {
            if (hpFull.activeSelf == false)
            {
                SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
                hpFull.SetActive(true);
                Invoke("HpFalse", 1.5f);
            }
        }
        public void HpFalse()
        {
            hpFull.SetActive(false);
        }
        public void OnClickHpItem()
        {
            Event.Selected evt = Core.Event.Getter.Get<Event.Selected>();
            evt.hashTable["select_type"] = Event.SelectedType.Item;
            evt.hashTable["select_item_id"] = UI.ItemID.HpItem;

            Core.Event.Dispatcher.Dispatch(evt);
        }

        public void OnClickApItem()
        {
            Event.Selected evt = Core.Event.Getter.Get<Event.Selected>();
            evt.hashTable["select_type"] = Event.SelectedType.Item;
            evt.hashTable["select_item_id"] = UI.ItemID.ApItem;

            Core.Event.Dispatcher.Dispatch(evt);
        }

        public void OnClickShieldItem()
        {
            Event.Selected evt = Core.Event.Getter.Get<Event.Selected>();
            evt.hashTable["select_type"] = Event.SelectedType.Item;
            evt.hashTable["select_item_id"] = UI.ItemID.ShieldItem;

            Core.Event.Dispatcher.Dispatch(evt);
        }

        public void InGameExit()
        {
            Event.Selected evt = Core.Event.Getter.Get<Event.Selected>();
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.CreditButton;
            Core.Event.Dispatcher.Dispatch(evt);
        }

        public void APCheck()
        {
            if (APChecker.activeSelf == false)
            {
                SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
                APChecker.SetActive(true);
                Invoke("OffAPChecker", 1.5f);
            }
        }
        public void OffAPChecker()
        {
            APChecker.SetActive(false);
        }

        public void EndResult()
        {
            firstResultImage.SetActive(false);
            secondResultImage.SetActive(false);
        }
    }
}