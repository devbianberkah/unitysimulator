using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveOncamera : MonoBehaviour
{
    public Camera mainCamera;
    public int globalState;
    public float moveSpeed = 2f;
    public float moveToPartSpeed = 2f;

    public enum InterpolationType { Lerp,Slerp}
    public InterpolationType interpolationType;
    public Button resetButton;
    public float moveForwardstopDistance =1f;
    public float moveBackStopDistance =1f;

    public float distance = 0.0f;
    private Transform targetObject;
    public bool isMoving = false;
    public bool isMovingToPart = false;
    public bool isMovingToPreviousPos = false;
    public float backwardDistance = 1f;    // Distance to move backward
    public float waitTime = 0.5f;          // Time to wait after moving backward

    public Vector3[] originPos;
    // Start is called before the first frame update
    void Start()
    {
       // originPos = new Vector3[99];
        originPos[0] = mainCamera.transform.position;// state awal camera
        resetButton.onClick.AddListener(BackToPreviousPosition);  
    }

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetMouseButtonDown(0) && !isMovingToPreviousPos && !isMoving && !isMovingToPart){
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit)){
                // can only move to machine when global state 0
                if(hit.transform.CompareTag("Machine") && globalState == 0){
                    targetObject = hit.transform;
                    isMoving = true;
                }
                // can only move to part when global state 1
                else if(hit.transform.CompareTag("Part")  && globalState == 1){
                    targetObject = hit.transform;
                    isMovingToPart = true;
                  //  StartCoroutine(MoveBackAndForthRoutine(targetObject.position));
                }
            }
        }

        if(isMoving && targetObject != null && !isMovingToPart && !isMovingToPreviousPos){
            Vector3 direction = targetObject.position- mainCamera.transform.position;
            distance = direction.magnitude;
            if(distance > moveForwardstopDistance) {
                mainCamera.transform.position = 
                interpolationType == InterpolationType.Lerp ?
                 Vector3.Lerp(mainCamera.transform.position,targetObject.position,moveSpeed*Time.deltaTime)
                : Vector3.Slerp(mainCamera.transform.position,targetObject.position,moveSpeed*Time.deltaTime);
            }
            else{
                isMoving = false;
                globalState++;
                originPos[globalState] = mainCamera.transform.position;
            }
        }
        if(isMovingToPart && !isMovingToPreviousPos && !isMoving){
            // Create a new position, only changing X and Y, keep the Z value the same as the current position
                Vector3 newPosition = new Vector3(
                    Mathf.Lerp(mainCamera.transform.position.x, targetObject.position.x, Time.deltaTime * moveToPartSpeed),
                    Mathf.Lerp(mainCamera.transform.position.y, targetObject.position.y, Time.deltaTime * moveToPartSpeed),
                    mainCamera.transform.position.z // Keep the current Z value unchanged
                );
                // Apply the new position to the object
                mainCamera.transform.position = newPosition;
                Vector2 direction = targetObject.position - mainCamera.transform.position;
                distance = direction.magnitude;
                if(distance < 0.1f) {
                    isMovingToPart = false;
                }
        }
        if(isMovingToPreviousPos && !isMovingToPart && !isMoving ){
            Vector3 direction = originPos[globalState] - mainCamera.transform.position;
            distance = direction.magnitude;
            if(distance > moveBackStopDistance) {
                mainCamera.transform.position = 
                interpolationType == InterpolationType.Lerp ?
                 Vector3.Lerp(mainCamera.transform.position,originPos[globalState],moveSpeed*Time.deltaTime)
                : Vector3.Slerp(mainCamera.transform.position,originPos[globalState],moveSpeed*Time.deltaTime);
            }
            else{
                isMovingToPreviousPos = false;
           }
        }
    }
    private IEnumerator MoveBackAndForthRoutine(Vector3 targetPosition)
    {
        // Calculate the backward position by moving the object away a little
        Vector3 backwardPosition = mainCamera.transform.position - transform.forward * backwardDistance;

        // Move back smoothly
        while (Vector3.Distance(mainCamera.transform.position, backwardPosition) > 0.1f)
        {
            mainCamera.transform.position = Vector3.Slerp(mainCamera.transform.position, backwardPosition, Time.deltaTime * moveSpeed);
            yield return null;
        }

        // Snap to the exact backward position to prevent small inaccuracies
        // mainCamera.transform.position = backwardPosition;

        // Wait for a moment
        yield return new WaitForSeconds(waitTime);

        // Move forward to the target position
        while (Vector3.Distance(mainCamera.transform.position, targetPosition) > moveForwardstopDistance)
        {
            mainCamera.transform.position = Vector3.Slerp(mainCamera.transform.position, targetPosition, Time.deltaTime * moveSpeed);
            yield return null;
        }

        // Snap to the exact target position
       // mainCamera.transform.position = targetPosition;

        // Movement complete
        isMovingToPart = false;
    }

    public void BackToPreviousPosition(){
        isMoving = false;
        isMovingToPreviousPos = true;
        globalState--;
        if(globalState <= 0) globalState = 0;
     //   mainCamera.transform.position = originPos[globalState];
    }
}
