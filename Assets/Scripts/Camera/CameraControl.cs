using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    [SerializeField]
    private GameObject player; //The Camera Itself
    private GameObject heldObj; //Object Which We Pick Up
    public Transform holdPos;
    public Transform Orientation; // to keep track of where the player is looking at
    public float BrushZAxis;
    public float throwForce = 500f; //Force At Which The Object Is Thrown At
    public float pickUpRange = 5f; //how far the player can pickup the object from
    private float rotationSensitivity = 1f; //how fast/slow the object is rotated in relation to mouse movement
    private Rigidbody heldObjRb; //rigidbody of object we pick up
    private bool canDrop = true; //this is needed so we don't throw/drop object when rotating the object
    private int LayerNumber; //layer index

    [SerializeField]
    private float _sensitivityX = 1f, _sensitivityY = 1f; //Sensitivity Modifier
    
    void Start(){

        //Cursor.lockState = CursorLockMode.Locked; //Locks Mouse In The Middle

        Cursor.visible = false;
    }
    void Update()
    {
        HandleLookX();
	    HandleLookY();
        HoldHandler();
    }

    private void HandleLookX()
    {
        float mouseX = Input.GetAxis("Mouse X"); //Gets The X Movement Of the Mouse

        Vector3 rotation = player.transform.localEulerAngles; //Gets The Current Camera Axis
        rotation.y += mouseX * _sensitivityX; // Modifies The Current Camera Axis
        player.transform.localEulerAngles = rotation; // Modifies The Current Camera 
        Orientation.transform.localEulerAngles = rotation;
    }

    private void HandleLookY()
    {
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 rotation = transform.localEulerAngles;
        rotation.x += mouseY * _sensitivityY;
        rotation.x = (rotation.x > 180) ? rotation.x - 360 : rotation.x;
        rotation.x = Mathf.Clamp(rotation.x, -70, 60);  
        transform.localEulerAngles = rotation;
    }
    private void HoldHandler()
    {
        if (Input.GetKeyDown(KeyCode.E)) //Checks Whether E is Pressed Down
        {
            Debug.Log("E Pressed");
            if (heldObj == null) //if currently not holding anything
            {
                //perform raycast to check if player is looking at object within pickuprange
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                {
                    Debug.Log("Object Detected");
                    //make sure pickup tag is attached
                    if (hit.transform.gameObject.tag == "canPickUp")
                    {
                        Debug.Log("Object Piackable");
                        //pass in object hit into the PickUpObject function
                        PickUpObject(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                if(canDrop == true)
                {
                    //StopClipping(); //prevents object from clipping through walls
                    DropObject();
                }
            }
        }
        if (heldObj != null) //if player is holding object
        {
            MoveObject(); //keep object position at holdPos
        }
    }
    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            Debug.Log("Started Picking Up");
            heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            GameObject ClosestCanvas = FindClosestCanvas();
            Vector3 HoldPos = getAxis(ClosestCanvas);
            BrushZAxis = HoldPos.z;
            Debug.Log(BrushZAxis);
            Debug.Log("Picking Up Done");
        }
    }
     void DropObject()
    {
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null; //unparent object
        heldObj = null; //undefine game object
    }
        void MoveObject()
    {
        float ratio = BrushZAxis/holdPos.transform.position.z;
        //keep object position the same as the holdPosition position
        Vector3 HoldPosZ = new Vector3(holdPos.transform.position.x*ratio, holdPos.transform.position.y*ratio, BrushZAxis-0.2f);
        //heldObj.transform.position = holdPos.transform.position;
        heldObj.transform.position = HoldPosZ;
        heldObj.transform.rotation = new Quaternion(1,0,0,1);
    }
    public GameObject FindClosestCanvas()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("BrushInteraction");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
    public Vector3 getAxis(GameObject GO){
        return GO.transform.position;
    }
}