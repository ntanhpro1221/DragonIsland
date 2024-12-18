using Mono.CSharp;
using QFSW.QC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class Controller : MonoBehaviour {
    #region MAIN

    #region Initialize
    private string curSceneName;
    private Camera cam;
    private PointerEventData pointerEventData;
    List<RaycastResult> raycastResults = new();
    const float maxOrthographicSize = 6;
    const float minOrthographicSize = 2;
    #endregion

    #region Events

    //Fix this when add interacting with server feature, use OnSpawnNetwork and something anyway :v
#if UNITY_EDITOR
    private void OnEnable() {
#else
    private void Start() {
#endif
        DontDestroyOnLoad(gameObject);
        //can be delete in release because in debug, it is added direct to basescene
        OnSceneLoaded(SceneManager.GetActiveScene());

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode = default) {
        switch (scene.name) {
            case "BaseScene":
#if UNITY_STANDALONE || UNITY_EDITOR
                Start_Debug();
#elif UNITY_ANDROID
                Start_BaseScene();
#endif
                break;
        }
    }

    private void Update() {
        curSceneName = SceneManager.GetActiveScene().name;
        switch (curSceneName) {
            case "BaseScene":
#if UNITY_STANDALONE || UNITY_EDITOR
                Update_Debug();
#elif UNITY_ANDROID
                Update_BaseScene();
#endif
                break;
        }
    }
#endregion

    #region Modules
    private bool IsInteractBackground(Vector2 coord) {
        pointerEventData.position = coord;
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);
        return raycastResults.Count <= 0;
    }

    #endregion

    #endregion

#if UNITY_STANDALONE || UNITY_EDITOR
    //#if false
    #region DEBUG

    #region Initialize
    Vector3 oldMouseCoord;
    float scaleSpeed = 0.1f;
    bool dragable = false;
    #endregion

    #region Events

    private void Start_Debug() {
        cam = Camera.main;
        pointerEventData = new PointerEventData(EventSystem.current);
    }

    private void Update_Debug() {
        //Controller
        if (Input.GetMouseButtonUp(0))
            dragable = false;
        if (Input.GetMouseButton(0) && dragable && Input.mousePosition != oldMouseCoord) {
            //Move camera
            cam.transform.Translate((cam.ScreenToWorldPoint(Vector3.zero) - cam.ScreenToWorldPoint(Input.mousePosition - oldMouseCoord)));
            oldMouseCoord = Input.mousePosition;
        }
        if (IsInteractBackground(Input.mousePosition)) {
            if (Input.mouseScrollDelta != Vector2.zero) {
                //Scale camera
                cam.orthographicSize *= 1 - Input.mouseScrollDelta.y * scaleSpeed;
                if (cam.orthographicSize < minOrthographicSize)
                    cam.orthographicSize = minOrthographicSize;
                else if (cam.orthographicSize > maxOrthographicSize)
                    cam.orthographicSize = maxOrthographicSize;
            }
            if (Input.GetMouseButtonDown(0)) {
                oldMouseCoord = Input.mousePosition;
                dragable = true;
            }
        }
    }
    #endregion

    #region Modules
    #endregion

    #endregion
#elif UNITY_ANDROID
    #region BASE_SCENE

    #region Initialize
    #endregion

    #region Events
    private void Start_BaseScene()
    {
        cam = Camera.main;
        pointerEventData = new PointerEventData(EventSystem.current);
    }

    private void Update_BaseScene()
    {
        if (Input.touches.Length == 1)
        {
            //Move camera
            if (Input.touches[0].phase == TouchPhase.Moved)
            {
                if (IsInteractBackground(Input.touches[0].position))
                {
                    cam.transform.Translate(cam.ScreenToWorldPoint(Vector3.zero) - cam.ScreenToWorldPoint(Input.touches[0].deltaPosition));
                }
            }
        }
        else if (Input.touches.Length > 1)
        {
            //Scale camera
            if (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved)
            {
                cam.orthographicSize *= 
                    Vector2.Distance(
                        Input.touches[0].position - Input.touches[0].deltaPosition, 
                        Input.touches[1].position - Input.touches[1].deltaPosition) / 
                    Vector2.Distance(
                        Input.touches[0].position, 
                        Input.touches[1].position);
                if (cam.orthographicSize < minOrthographicSize)
                    cam.orthographicSize = minOrthographicSize;
                else if (cam.orthographicSize > maxOrthographicSize)
                    cam.orthographicSize = maxOrthographicSize;
            }
        }
    }

    #endregion

    #region Modules

    #endregion

    #endregion
#endif
}