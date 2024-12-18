using QFSW.QC;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.Base.UI {
    public class BR_Bag : MonoBehaviour {
#if UNITY_EDITOR
        private void OnEnable() {
#else
        private void Start() {
#endif
            for (int i = 0; i < compartments.Count; ++i) {
                int j = i;
                compartments[j].Find("Selector").GetComponent<Button>().onClick.AddListener(() => OnClickCompartment(j));
            }
            exitBtn.GetComponent<Button>().onClick.AddListener(() => OnClickExit());
        }

        /// <summary>
        /// Backpack compartment's button
        /// </summary>
        [SerializeField]
        private List<Transform> compartments;   

        [SerializeField]
        private GameObject toggle;

        /// <summary>
        /// Backpack's exit button
        /// </summary>
        [SerializeField]
        private Button exitBtn;

        #region Handle backpack compartment selection
        /// <summary>
        /// Sprite of compartment's selection button when in inactive state. 
        /// </summary>
        [SerializeField]
        private Sprite inactiveSptire;

        /// <summary>
        /// Sprite of compartment's selection button when in active state. 
        /// </summary>
        [SerializeField]
        private Sprite activeSptire;

        /// <summary>
        /// Drawing order of compartment's selection button when in inactive state. 
        /// </summary>
        private readonly DrawingOrder inactiveOrder = DrawingOrder.Back;

        /// <summary>
        /// Drawing order of compartment's selection button when in active state. 
        /// </summary>
        private readonly DrawingOrder activeOrder = DrawingOrder.Fore;


        /// <summary>
        /// The index of button (also and it's icon) which is being actived. 
        /// By default, it is the first button with the index of 0. 
        /// </summary>
        private int curActiveBtn = 0;

        /// <summary>
        /// The index of button (also and it's icon) which will be actived next. 
        /// It is used as the temporary variable. 
        /// </summary>
        private int newActiveBtn;

        /// <summary>
        /// Handle something after any backpack compartnents's selection button is clicked. 
        /// </summary>
        private void OnClickCompartment(int BtnId) {
            newActiveBtn = BtnId;
            if (curActiveBtn == newActiveBtn) //do nothing if it is being actived
                return;

            //change sprite
            compartments[newActiveBtn].Find("Selector").GetComponent<Image>().sprite = activeSptire;
            compartments[curActiveBtn].Find("Selector").GetComponent<Image>().sprite = inactiveSptire;

            //change drawing order
            compartments[newActiveBtn].Find("Selector").GetComponent<Canvas>().sortingOrder = (int)activeOrder;
            compartments[curActiveBtn].Find("Selector").GetComponent<Canvas>().sortingOrder = (int)inactiveOrder;

            //Change item list
            compartments[newActiveBtn].Find("ItemList").gameObject.SetActive(true);
            compartments[curActiveBtn].Find("ItemList").gameObject.SetActive(false);

            curActiveBtn = newActiveBtn;
        }

        #endregion

        /// <summary>
        /// When open backpack
        /// </summary>
        public void OnClickIcon() {
            toggle.SetActive(true);
        }

        /// <summary>
        /// When close backpack
        /// </summary>
        public async void OnClickExit() {
            await Task.Run(() => {
                while (exitBtn.image.color == exitBtn.colors.selectedColor) ;
            });
            toggle.SetActive(false);
        }
    }
}