using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MyGrabber3D
{
    public enum GrabType { Velocity, Force, Transform };

    [System.Serializable]
    public class ForceGrab
    {
        public float grabForce;
        public float throwForce;
        public bool useObjectMassOnThrow;
    }

    [System.Serializable]
    public class VelocityGrab
    {
        public float grabVelocity;
        public float throwVelocity;
        public bool useObjectMassOnThrow;
        public bool parentObject;
    }

    public class MyGrabber : MonoBehaviour
    {
        private bool holdingObject;
        private Rigidbody objectRb;
        private float objectDrag;
        private Transform oldParent;
        private RigidbodyInterpolation objectInterpolation;
        private IMyGrabberInputHandler inputHandler;
        public GrabType grabType = GrabType.Velocity;
        public ForceGrab forceGrab;
        public VelocityGrab velocityGrab;
        public float objectRotationSpeed = 2f;

        public event UnityAction<GameObject> onObjectGrabbed;
        public event UnityAction<GameObject> onObjectDropped;
        public event UnityAction<GameObject> onObjectThrown;
        public event UnityAction onObjectDestroyed;

        public Transform grabPos;
        public bool maintainObjectOffset = true;
        public bool returnToParent = true;
        public Vector3 grabPosOffset = new Vector3(0f, 0f, 2.5f);
        public float breakDistance;

        public GameObject grabbedObj
        {
            get;
            private set;
        }

        void Start()
        {
            // Add default input handler
            inputHandler = GetComponentInParent<IMyGrabberInputHandler>();
            if (inputHandler == null)
                inputHandler = gameObject.AddComponent<Interaction>();

            CheckForDragPos();
        }

        void OnDisable()
        {
            ReleaseObject();
        }

        public void GrabObject(GameObject obj)
        {
            if (grabbedObj != null)
                ReleaseObject();

            if (obj != null && Vector3.Distance(obj.transform.position, grabPos.position) < breakDistance) //Ensure its under break distance
            {

                grabbedObj = obj;

                if (maintainObjectOffset)
                    grabPos.position = grabbedObj.transform.position;

                if (obj.transform.parent != null && returnToParent)
                    oldParent = obj.transform.parent;

                if (grabbedObj.GetComponent<Rigidbody>())
                {
                    objectRb = grabbedObj.GetComponent<Rigidbody>();
                    objectDrag = objectRb.drag;//Saves object drag
                    objectInterpolation = objectRb.interpolation;//Saves object rigidbody interpolation mode
                    objectRb.angularDrag = 5f;
                }

                switch (grabType)
                {
                    case GrabType.Velocity:
                        if (!objectRb)
                            return;
                        if (velocityGrab.parentObject)
                        {
                            objectRb.interpolation = RigidbodyInterpolation.None;
                            grabbedObj.transform.SetParent(this.transform, true);
                        }
                        else
                            objectRb.interpolation = RigidbodyInterpolation.Interpolate;
                        break;
                    case GrabType.Force:
                        if (!objectRb)
                            return;
                        objectRb.interpolation = RigidbodyInterpolation.Interpolate;
                        break;
                    case GrabType.Transform:
                        grabbedObj.transform.position = grabPos.position;
                        grabbedObj.transform.SetParent(this.transform, true);
                        if (objectRb)
                        {
                            objectRb.useGravity = false;
                            objectRb.detectCollisions = false;
                        }
                        break;
                }

                onObjectGrabbed?.Invoke(grabbedObj);

                holdingObject = true;
            }
            else
                holdingObject = false;
        }

        public void ReleaseObject()
        {
            if (grabbedObj)
            {
                onObjectDropped?.Invoke(grabbedObj);

                DisconnectFromObject();
            }
        }

        public void ThrowObject()
        {
            if (grabbedObj)
            {

                Vector3 direction = this.transform.forward;
                float force = 0f;
                bool useMassOnThrow = false;

                switch (grabType)
                {
                    case GrabType.Force:
                        force = forceGrab.throwForce;
                        useMassOnThrow = forceGrab.useObjectMassOnThrow;
                        break;
                    case GrabType.Velocity:
                        force = velocityGrab.throwVelocity;
                        useMassOnThrow = velocityGrab.useObjectMassOnThrow;
                        break;
                    case GrabType.Transform:
                        force = velocityGrab.throwVelocity;
                        useMassOnThrow = velocityGrab.useObjectMassOnThrow;
                        break;
                }
                grabbedObj.GetComponent<Rigidbody>().AddForce(direction * force, useMassOnThrow ? ForceMode.Impulse : ForceMode.VelocityChange);

                onObjectThrown?.Invoke(grabbedObj);

                DisconnectFromObject();
            }
        }

        void DisconnectFromObject()
        {
            inputHandler.cameraInputs = true;

            if(returnToParent && oldParent != null)
                grabbedObj.transform.parent = oldParent;
            else
                grabbedObj.transform.parent = null;

            if (objectRb)
            {
                objectRb.useGravity = true;
                objectRb.detectCollisions = true;
                objectRb.angularDrag = 0.05f;
                objectRb.drag = objectDrag;
                objectRb.interpolation = objectInterpolation;
                objectRb = null;
            }

            grabbedObj = null;
            holdingObject = false;
            oldParent = null;
        }

        public void RotateObject(float x, float y)
        {
            if (grabbedObj)
            {
                grabbedObj.transform.Rotate(grabPos.up, -x * objectRotationSpeed, Space.World);
                grabbedObj.transform.Rotate(grabPos.right, y * objectRotationSpeed, Space.World);
            }
        }

        void FixedUpdate()
        {
            if (grabbedObj) //This check are made only for dragTypes that works with physics
            {
                // Check for break distance
                float grabbedObjDistance = Vector3.Distance(grabbedObj.transform.position, grabPos.position);
                if (grabbedObjDistance > breakDistance)
                    ReleaseObject();
                else
                {
                    switch (grabType)
                    {
                        case GrabType.Velocity:
                            if (!objectRb)
                                return;

                            objectRb.velocity = (grabPos.position - grabbedObj.transform.position) * velocityGrab.grabVelocity;
                            break;
                        case GrabType.Force:
                            if (!objectRb)
                                return;

                            objectRb.AddForce((grabPos.position - grabbedObj.transform.position) * forceGrab.grabForce, ForceMode.Force);
                            objectRb.drag = 5f / (grabPos.position - grabbedObj.transform.position).magnitude;
                            break;
                    }
                }
            }
            else
            {
                // Check if object was destroyed while holding
                if (holdingObject)
                {
                    holdingObject = false;
                    inputHandler.cameraInputs = true;

                    onObjectDestroyed?.Invoke();
                }
            }
        }

        void CheckForDragPos()
        {
            if (grabPos == null)
            {
                GameObject gPos = new GameObject("Grab Pos");
                gPos.transform.SetParent(this.transform);
                gPos.transform.localPosition = grabPosOffset;
                grabPos = gPos.transform;
            }
        }
    }
}