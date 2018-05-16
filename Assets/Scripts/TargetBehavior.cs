using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Vuforia;
using System.Collections.Generic;

public class TargetBehavior : MonoBehaviour, ITrackableEventHandler
{

    public Button TrackButton;
    public Button ShotTopButton;
    public Button Gyro;
    public GyroController CameraGyro;
    private TrackableBehaviour mTrackableBehaviour;
    private Tracker imageTracker;
    bool tracked = false;

   // This function should set the state to searching for the trackable
    void ResumeTracking()
    {
	tracked = false;
	OnTrackingLost();
        imageTracker.Start();
	GyroOff();
    }

    void PauseTracking()
    {
        imageTracker.Stop();
    //    Debug.Log("Pause Tracking");
    }
    // should reproduce the tracking Extended State if possible otherwise it goes to
    // Tracking Lost State.
    void GyroOn()
    {
        CameraGyro.Paused = false;
	Debug.Log("Gyro On Extended Pause Traking");
	PauseTracking();
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
        imageTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();

        // Query the StateManager to retrieve the list of
        // currently 'active' trackables 
        //(i.e. the ones currently being tracked by Vuforia)

        // Iterate through the list of active trackables
        Debug.Log("List of trackables currently active (tracked): ");
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
        Debug.Log("Image type " + mTrackableBehaviour.GetType());
        //extendedTrackable
        //extendedTrackable et = GetComponent<ExtendedTrackable>();
        //ObjectTarget et = GetComponent<ObjectTarget>();
        //et.StartExtendedTracking();
        if (mTrackableBehaviour != null)
        {
            Debug.Log("mTrackable not null");
        }
        else
        {
            Debug.Log("mTrackable is null");
        }
        if (mTrackableBehaviour)
        {
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
            Debug.Log("Trackable behaviour found");
        }
        
        TrackButton.onClick.AddListener(ResumeTracking);
	Gyro.onClick.AddListener(GyroOff);
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
            case TrackableBehaviour.Status.DETECTED:
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
	    case TrackableBehaviour.Status.NOT_FOUND:
	    case TrackableBehaviour.Status.UNKNOWN:
        	default:
                // Target not in camera, but Vuforia can still calculate position and orientation
                //   and update ARCamera.
                // TODO-2.b
		if ( tracked && CameraGyro.Paused) {
			GyroOn();
                	Debug.Log("Turn Gyro On");
                	TrackButton.image.color = new Color(0.7f, 0.5f, 0.1f, 0.5f);
		}
		else if (!tracked) {
			ResumeTracking();
            		TrackButton.image.color = new Color(1.0f, 0.1f, 0.1f, 0.5f);
		}
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
