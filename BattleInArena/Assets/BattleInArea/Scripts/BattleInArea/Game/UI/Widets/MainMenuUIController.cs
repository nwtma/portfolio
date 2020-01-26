using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace BattleInArea.Game
{
    public class MainMenuUIController : Core.UI.Controller
    {
        public Event.Selected evt;
        public override int ID
        {
            get { return (int)UI.ID.MainMenuUIController; }
        }

        [SerializeField]
        protected GameObject quitUi;
        [SerializeField]
        protected GameObject soundUi;
        [SerializeField]
        protected GameObject optionUi;
        [SerializeField]
        protected GameObject FogObject;
        [SerializeField]
        protected GameObject bgmMute;
        [SerializeField]
        protected GameObject bgmUnMute;
        [SerializeField]
        protected GameObject effectMute;
        [SerializeField]
        protected GameObject effectUnMute;
        [SerializeField]
        protected Slider bgm_Slider;
        [SerializeField]
        protected Slider effect_Slider;
        [SerializeField]
        protected GameObject[] Characters;

        protected bool IsOnQuit;
        protected bool IsSound;
        protected bool IsOption;
        protected Button Fog;
        public void Start()
        {
            evt = Core.Event.Getter.Get<Event.Selected>();
            RandomCharacter();
        }
        public void Update()
        {
            SetSlider();
        }
        public void OnClickStartButton()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.MainMenuStartButton;
            Core.Event.Dispatcher.Dispatch(evt);
        }
        public void OnClickBookButton()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.MainMenuBookButton;
            Core.Event.Dispatcher.Dispatch(evt);
        }
        public void OnClickCreditButton()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            evt.hashTable["select_type"] = Event.SelectedType.UI;
            evt.hashTable["select_ui_id"] = UI.ID.MainMenuCreditButton;
            Core.Event.Dispatcher.Dispatch(evt);
        }
        public void OnClickOptionButton()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            IsOption = true;
            optionUi.SetActive(true);
        }
        public void OnClickSoundButton()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            IsSound = true;
            IsOption = false;
            FogEnable();
            soundUi.SetActive(true);
        }
        public void OnClickQuitButton()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            IsOnQuit = true;
            IsOption = false;
            FogEnable();
            quitUi.SetActive(true);
        }
        public void OnClickBackButton()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            if (IsOption)
            {
                optionUi.SetActive(false);
                IsOption = false;
            }
            else if (IsOnQuit)
            {
                FogEnable();
                quitUi.SetActive(false);
                IsOnQuit = false;
                IsOption = true;
            }
            else if (IsSound)
            {
                FogEnable();
                soundUi.SetActive(false);
                IsSound = false;
                IsOption = true;
            }
        }
        public void ExitYes()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            Application.Quit();
            Debug.Log("게임종료");
        }

        public void FogEnable()
        {
            Fog = FogObject.GetComponentInChildren<Button>();
            if(Fog.enabled)
            {
                Fog.enabled = false;
            }
            else
            {
                Fog.enabled = true;
            } 
        }
        public void SetSlider()
        {
            SoundManager.instance.SetBgmSlider(bgm_Slider);
            SoundManager.instance.SetEffectSlider(effect_Slider);
        }
        public void OnClickEffectMute()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");

            SoundManager.instance.EffectMute(effectMute, effectUnMute);
        }
        public void OnClickBgmMute()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");

            SoundManager.instance.BgmMute(bgmMute, bgmUnMute);
        }

        public void RandomCharacter()
        {
            int a;
            a = Random.Range(0, 4);

            if(a == 0)
            {
                Characters[0].SetActive(true);
            }
            else if(a == 1)
            {
                Characters[1].SetActive(true);
            }
            else if(a == 2)
            {
                Characters[2].SetActive(true);
            }
            else if(a == 3)
            {
                Characters[3].SetActive(true);
            }
        }
        public void OnClickCharacter()
        {
            SoundManager.instance.PlayOneShotSE("Sound/Effect/Button");
            if (Characters[0].activeSelf == true)
            {
                Characters[0].SetActive(false);
                Characters[1].SetActive(true);
                Characters[2].SetActive(false);
                Characters[3].SetActive(false);

            }
            else if (Characters[1].activeSelf == true)
            {
                Characters[0].SetActive(false);
                Characters[1].SetActive(false);
                Characters[2].SetActive(true);
                Characters[3].SetActive(false);
            }
            else if(Characters[2].activeSelf == true)
            {
                Characters[0].SetActive(false);
                Characters[1].SetActive(false);
                Characters[2].SetActive(false);
                Characters[3].SetActive(true);
            }
            else if(Characters[3].activeSelf == true)
            {
                Characters[0].SetActive(true);
                Characters[1].SetActive(false);
                Characters[2].SetActive(false);
                Characters[3].SetActive(false);
            }
        }
    }
}