using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable
namespace Scenes.Base.UI {
    //[Serializable]
    //public class Test : KeyValuePair<string, string>{
    //    [SerializeField]
    //    private int Int;
    //    public string String;
    //}

    public class UIManager : MonoBehaviour {
        public static UIManager Instance { get; private set; }
#if UNITY_EDITOR
        private void OnEnable() {
#else
        private void Awake() {
#endif
            if (Instance == null) Instance = this;
            else Destroy(this);
        }
    }
}
