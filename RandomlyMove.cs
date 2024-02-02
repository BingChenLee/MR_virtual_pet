using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class RandomlyMove : MonoBehaviour
{
    public float movementSpeed = 50f;
    public float rotationSpeed = 100f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;

    private bool holdingball = false;
    private bool approaching = false;
    public bool petting = false;

    Rigidbody rb;
    public GameObject Target;
    public GameObject origin;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // if ball isn't exist, randomly move; else, find the ball
        if(Target.GetComponent<Throwable>().getBall == false && holdingball == false){

            if (isWandering == false){
                StartCoroutine(Wander());
            }

            if (isRotatingRight == true){
                transform.Rotate(transform.up * Time.deltaTime * rotationSpeed);
            }

            if (isRotatingLeft == true){
                transform.Rotate(transform.up * Time.deltaTime * -rotationSpeed);
            }

            if (isWalking == true){
                rb.AddForce(transform.forward * movementSpeed);
            }
        }

        else if(Target.GetComponent<Throwable>().getBall == true){
            transform.LookAt(Target.transform.position);
            transform.position = Vector3.Lerp(transform.position, Target.transform.position, 0.05f);
            // rb.AddForce(transform.forward * movementSpeed);
            // rb.velocity = transform.forward * movementSpeed;
        }

        else if(Target.GetComponent<Throwable>().getBall == false && holdingball == true){
            transform.LookAt(origin.transform.position);
            if(Vector3.Distance(origin.transform.position, transform.position) > 0.5f && approaching == true){
                transform.position = Vector3.Lerp(transform.position, origin.transform.position, 0.05f);
            }
            else{
                approaching = false;
            }
            // rb.AddForce(transform.forward * movementSpeed);
            // rb.velocity = transform.forward * movementSpeed;
        }

        if(petting == true){
            // StartCoroutine(Timer());
            Debug.Log("I'm happy");
            petting = false;
            holdingball = false;
        }

        // press botton A to reset
        if (OVRInput.Get(OVRInput.Button.One)){
            Debug.Log("Reset animal");
            petting = false;
            holdingball = false;
            transform.position = new Vector3(-1f, 0.117f, 1f);
        }
    }

    IEnumerator Wander()
    {
        int rotationTime = Random.Range(1, 3);
        int rotationWait = Random.Range(1 ,3);
        int rotationDirection = Random.Range(1, 3);
        int walkWait = Random.Range(1 ,3);
        int walkTime = Random.Range(1 ,3);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);

        isWalking = true;

        yield return new WaitForSeconds(walkTime);

        isWalking = false;

        yield return new WaitForSeconds(rotationWait);

        if (rotationDirection == 1){
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingLeft = false;
        }

        if (rotationDirection == 2){
            isRotatingRight = true;
            yield return new WaitForSeconds(rotationTime);
            isRotatingRight = false;
        }

        isWandering = false;
    }

    private void OnCollisionEnter(Collision other) {

        // Collision with ball, then stop to find ball, and hold ball
        if(other.gameObject.tag == "ball"){
            holdingball = true;
            Target.GetComponent<Throwable>().getBall = false;
            approaching = true;
        }
    }

    // IEnumerator Timer(){
    //     yield return new WaitForSeconds(3);
    // }
}
