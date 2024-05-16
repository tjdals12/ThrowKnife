using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    public enum UIType {
        Lobby = 0,
        Inventory,
        InGame,
        GameOver,
        KnifePopup,
        SettingsPopup,
        Loading,
    }

    public enum UIAnimationType {
        None = 0,
        Fade,
        Slide,
        Scale
    }

    public class Data : MonoBehaviour
    {
        public UIType prevUI = UIType.Lobby;
        public UIType currentUI = UIType.Lobby;

        public event Action<UIType, UIAnimationType, Action> OnMove;
        public event Action<UIType, UIAnimationType, Action> OnOpen;
        public event Action<UIType, UIAnimationType, Action> OnClose;

        public void Move(UIType uiType, UIAnimationType uiAnimationType = UIAnimationType.None, Action callback = null) {
            this.OnMove?.Invoke(uiType, uiAnimationType, callback);
        }

        public void Open(UIType uiType, UIAnimationType uiAnimationType = UIAnimationType.None, Action callback = null) {
            this.OnOpen?.Invoke(uiType, uiAnimationType, callback);
        }

        public void Close(UIType uiType, UIAnimationType uiAnimationType = UIAnimationType.None, Action callback = null) {
            this.OnClose?.Invoke(uiType, uiAnimationType, callback);
        }
    }
}
