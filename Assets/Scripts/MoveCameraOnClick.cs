using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveOncamera : MonoBehaviour
{
    public Camera mainCamera;
    public float moveSpeed = 2f;

    public enum InterpolationType { Lerp,Slerp}
    public InterpolationType interpolationType;
    public Button resetButton;
    public float stopDistance =1f;
    private Transform targetObject;
    private bool isMoving = false;

    private Vector3 originPos;
    // Start is called before the first frame update
    void Start()
    {
        originPos = mainCamera.transform.position;
        resetButton.onClick.AddListener(ResetCamera);  
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetMouseButtonDown(0)){
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit)){
                if(hit.transform.CompareTag("Clickable")){
                    targetObject = hit.transform;
                    isMoving = true;
                }
            }
        }

        if(isMoving && targetObject != null){
            Vector3 direction = targetObject.position- mainCamera.transform.position;
            float distance = direction.magnitude;
            if(distance > stopDistance) {
                mainCamera.transform.position = 
                interpolationType == InterpolationType.Lerp ?
                 Vector3.Lerp(mainCamera.transform.position,targetObject.position,moveSpeed*Time.deltaTime)
                : Vector3.Slerp(mainCamera.transform.position,targetObject.position,moveSpeed*Time.deltaTime);
            }
            else{
                isMoving = false;
            }
        }
    }

    public void ResetCamera(){
        isMoving = false;
        mainCamera.transform.position = originPos;
    }
}
