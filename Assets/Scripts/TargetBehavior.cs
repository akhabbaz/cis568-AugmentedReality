using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Vuforia;
using System;

public class TargetBehavior : MonoBehaviour, ITrackableEventHandler
{

    public Button TrackButton;
    public Button ShotTopButton;
    public Button Gyro;
    public GyroController CameraGyro;
    private TrackableBehaviour mTrackableBehaviour;
    bool tracked = false;


    void ResumeTracking()
    {
        Tracker imageTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        imageTracker.Start();
        Debug.Log("Resume Tracking");
    }

    void PauseTracking()
    {
        Tracker imageTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        imageTracker.Stop();
        Debug.Log("Pause Tracking");
    }
    void GyroOn()
    {
        CameraGyro.Paused = false;
    }

    // Use this for initialization
    void Start () {
        CameraGyro = GetComponent<GyroController>();
        if (CameraGyro == null)
        {
            Debug.Log("Camera Gyro is null");
        }
        else
        {
            Debug.Log("Camera Gyro is found");

        }
        CameraGyro.Paused = true;
        CameraGyro.ControlledObject = GameObject.FindWithTag("MainCamera");
       
        //Debug.Assert(CameraGyro.ControlledObject != null); 
        if (CameraGyro.ControlledObject != null)
        {
            Debug.Log("AR Camera found");
            Debug.Log(CameraGyro.ControlledObject.transform);
        }
        else
        {
            Debug.Log("Ar Camera not attached.");
        }

        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
        
        TrackButton.onClick.AddListener(ResumeTracking);
    }

    //public void OnTrackableStateChanged(
    //    TrackableBehaviour.Status previousStatus,
    //    TrackableBehaviour.Status newStatus)
    //{
    //    Debug.Log("Switch State prior: " + previousStatus + 
    //             " new : " + newStatus);
    //    if (newStatus == TrackableBehaviour.Status.DETECTED ||
    //        newStatus == TrackableBehaviour.Status.TRACKED ||
    //        newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
    //    {
    //        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
    //        OnTrackingFound();
    //            TrackButton.image.color = new Color(0.4f, 1, 0.7f, 0.5f);
    //    }
    //    else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
    //             newStatus == TrackableBehaviour.Status.NOT_FOUND)
    //    {
    //        Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
    //        OnTrackingLost();
    //    }
    //    else
    //    {
    //        // For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
    //        // Vuforia is starting, but tracking has not been lost or found yet
    //        // Call OnTrackingLost() to hide the augmentations
    //        OnTrackingLost();
    //            TrackButton.image.color = new Color(1, 0.1f, 0.1f, 0.5f);
    //    }
    //}
    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus)
    {
        Debug.Log("Switch State prior: " + previousStatus + 
                 " new : " + newStatus);
        switch (newStatus)
        {
            case TrackableBehaviour.Status.TRACKED:
                // Target in camera
                // TODO-2.b 
                // Recalibrate reference quaternions at GyroController
                //   and switch the Control of the camera between Vuforia and GyroController.
                // You may want to toggle GyroController.Paused .
		if (tracked == false){
		        CameraGyro.ResetOrientation();
		}
                tracked = true;
		Debug.Log("Tracked");
                CameraGyro.Paused = true;
                TrackButton.image.color = new Color(0.4f, 1, 0.7f, 0.5f);
                break;
            case TrackableBehaviour.Status.EXTENDED_TRACKED:
                // Target not in camera, but Vuforia can still calculate position and orientation
                //   and update ARCamera.
                // TODO-2.b
		Debug.Log("Extended");
                CameraGyro.Paused = false;
                tracked = true;
                PauseTracking();
                TrackButton.image.color = new Color(0.7f, 0.5f, 0.1f, 0.5f);
                break;
            default:
                tracked = false;
		Debug.Log("Not Tracking");
		CameraGyro.Paused = true;
                // TODO-2.b
                ResumeTracking(); 
                TrackButton.image.color = new Color(1, 0.1f, 0.1f, 0.5f);
                break;
        }
        
    }
    protected virtual void OnTrackingFound()
    {
        Debug.Log("Tracking Found");
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
    }


    protected virtual void OnTrackingLost()
    {
        Debug.Log("Tracking Lost");
        var rendererComponents = GetComponentsInChildren<Renderer>(true);
        var colliderComponents = GetComponentsInChildren<Collider>(true);
        var canvasComponents = GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }
}
