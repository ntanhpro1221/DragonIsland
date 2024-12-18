using UnityEngine;

namespace Scenes.Login {
    public class UIManager : MonoBehaviour {
        [SerializeField]
        private GameObject LoadBarObj;
        [SerializeField]
        private GameObject registerObj;
        [SerializeField]
        private GameObject loginObj;
        [SerializeField]
        private GameObject menuObj;

        public static UIManager Instance { get; private set; }
#if UNITY_EDITOR
        private void OnEnable() {
#else
        private void Awake() {
#endif
            if (Instance == null) Instance = this;
            else Destroy(this);
        }

        public void LoadScreen() {
            LoadBarObj.SetActive(true);
            registerObj.SetActive(false);
            loginObj.SetActive(false);
            menuObj.SetActive(false);
        }

        public void RegisterScreen() {
            LoadBarObj.SetActive(false);
            registerObj.SetActive(true);
            loginObj.SetActive(false);
            menuObj.SetActive(false);
        }

        public void LoginScreen() {
            LoadBarObj.SetActive(false);
            registerObj.SetActive(false);
            loginObj.SetActive(true);
            menuObj.SetActive(false);
        }

        public void ProfileMenuScreen() {
            LoadBarObj.SetActive(false);
            registerObj.SetActive(false);
            loginObj.SetActive(false);
            menuObj.SetActive(true);
        }

        public void VoidScreen() {
            LoadBarObj.SetActive(false);
            registerObj.SetActive(false);
            loginObj.SetActive(false);
            menuObj.SetActive(false);
        }

    }
}
