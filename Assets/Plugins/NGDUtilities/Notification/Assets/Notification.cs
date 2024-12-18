using UnityEngine;

namespace NGDS {
    public enum Type {
        Notification,
        Warning,
        Error,
        Congratulation
    }
    public class Notification : MonoBehaviour {
        public static Notification Instance { get; private set; }
        private GameObject m_toggle;
        private TMPro.TextMeshProUGUI m_content;
        private TMPro.TextMeshProUGUI m_header;

        [Header("Message's color")]
        [SerializeField]
        private Color m_notification = new(.0f / 255, .0f / 255, .0f / 255);
        [SerializeField]
        private Color m_warning = new(150.0f / 255, 150.0f / 255, .0f / 255);
        [SerializeField]
        private Color m_error = new(255.0f / 255, .0f / 255, .0f / 255);
        [SerializeField]
        private Color m_congratulation = new(.0f / 255, 150.0f / 255, .0f / 255);
#if UNITY_EDITOR
        private void OnEnable() {
#else
        private void Start() {
#endif
            m_toggle = transform.Find("Toggle").gameObject;
            m_content = m_toggle.transform.Find("ContentTxt").GetComponent<TMPro.TextMeshProUGUI>();
            m_header = m_toggle.transform.Find("HeaderTxt").GetComponent<TMPro.TextMeshProUGUI>();
            if (Instance != null) Destroy(Instance);
            Instance = this;
        }        

        public void OnClick() {
            m_toggle.SetActive(false);
        }

        public void Notify(string content, Type type = default(Type)) {
            m_toggle.SetActive(true);
            m_content.text = content;
            m_header.text = type + ":";
            m_header.color = m_content.color = SuitColor(type);
        }

        private Color SuitColor(Type type) {
            switch (type) {
                case Type.Notification:
                    return m_notification;
                case Type.Warning:
                    return m_warning;
                case Type.Error:
                    return m_error;
                case Type.Congratulation:
                    return m_congratulation;
                default:
                    return m_notification;
            }
        }
    }

}
