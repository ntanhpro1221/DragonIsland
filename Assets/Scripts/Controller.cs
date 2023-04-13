using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    //////////////////////////// MAIN ////////////////////////////
    ////Initialzize
    private string curScene;
    private Camera cam;
    private PointerEventData PointerEventData;

    ////Start
    private void Start()
    {
        Start_BaseScene();
    }

    ////Update
    private void Update()
    {
        curScene = SceneManager.GetActiveScene().name;
        switch (curScene)
        {
            case "BaseScene":
                Update_BaseScene();
                break;
        }
    }

    ////Module



    //////////////////////////// BASE SCENE ////////////////////////////
    ////Initialize
    List<RaycastResult> raycastResults = new List<RaycastResult>();
    const short maxOrthographicSize = 6;
    const short minOrthographicSize = 2;

    ////Start
    private void Start_BaseScene()
    {
        cam = Camera.main;
        PointerEventData = new PointerEventData(EventSystem.current);
    }

    ////Update
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
                cam.orthographicSize *= Vector2.Distance(Input.touches[0].position - Input.touches[0].deltaPosition, Input.touches[1].position - Input.touches[1].deltaPosition) / Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
                if (cam.orthographicSize < minOrthographicSize)
                    cam.orthographicSize = minOrthographicSize;
                else if (cam.orthographicSize > maxOrthographicSize)
                    cam.orthographicSize = maxOrthographicSize;
            }
        }
    }

    ////Module
    private bool IsInteractBackground(Vector2 coord)
    {
        PointerEventData.position = coord;
        EventSystem.current.RaycastAll(PointerEventData, raycastResults);
        return raycastResults.Count <= 0;
    }
}
