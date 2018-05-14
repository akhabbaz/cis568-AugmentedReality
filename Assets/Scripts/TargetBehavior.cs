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

   // This function should set the state to searching for the trackable
    void ResumeTracking()
    {
        Tracker imageTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        imageTracker.Start();
	GyroOff();
	tracked = false;
	OnTrackingLost();
    }

    void PauseTracking()
    {
        Tracker imageTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
        imageTracker.Stop();
        Debug.Log("Pause Tracking");
        GyroOn();
    }
    // should reproduce the tracking Extended State if possible otherwise it goes to
    // Tracking Lost State.
    void GyroOn()
    {
        CameraGyro.Paused = false;
	Debug.Log("Gyro On");
	if (tracked) 
	{ 
	   CameraGyro.ResetOrientation();
	}
	else {
	    ResumeTracking();
	}
	    
    }
    void GyroOff()
    {
    	CameraGyro.Paused = true;
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
	Gyro.onClick.AddListener(PauseTracking);
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
		if (tracked) {
		     CameraGyro.UpdateOrientation(1.0f);
		     Debug.Log("Orientation updated");
		}
		else{
			CameraGyro.ResetOrientation();
			Debug.Log("Orientation Reset");
		}
                tracked = true;
                CameraGyro.Paused = true;
		if (!tracked) {
		        OnTrackingFound();
		}
                TrackButton.image.color = new Color(0.4f, 1, 0.7f, 0.5f);
                break;
            case TrackableBehaviour.Status.EXTENDED_TRACKED:
                // Target not in camera, but Vuforia can still calculate position and orientation
                //   and update ARCamera.
                // TODO-2.b
                CameraGyro.Paused = false;
               // PauseTracking();
                Debug.Log("Extended Tracking");
                TrackButton.image.color = new Color(0.7f, 0.5f, 0.1f, 0.5f);
                break;
	    case TrackableBehaviour.Status.NOT_FOUND:
	    case TrackableBehaviour.Status.UNKNOWN:
	    case TrackableBehaviour.Status.DETECTED:
        default:
			OnTrackingLost();
			CameraGyro.Paused = true;
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
