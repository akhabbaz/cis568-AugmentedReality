using UnityEngine;
using System.Collections;

public class GyroController : MonoBehaviour
{
    public GameObject ControlledObject
    {
        get { return controlledObject; }
        set
        {
            controlledObject = value;
            ResetOrientation();
        }
    }

    public bool Paused { get; set; }

    Quaternion qRefObject = Quaternion.identity;
    Quaternion qRefGyro = Quaternion.identity;
    Gyroscope gyro;
    // velocity of the camera
    Vector3 vel;
    // change position Dot of the camera
    Vector3 posDot;
    // velocity dot of the camera
    Vector3 velDot;
    // last interval's acceleration
    Vector3 priorAccel;

    GameObject controlledObject;

    void Awake()
    {
        Paused = false;
    }

    // Use this for initialization
    void Start()
    {
        gyro = Input.gyro;
        gyro.enabled = true;
        gyro.updateInterval = 0.01f;
        Debug.Log("Gyro initialized update interval = " + gyro.updateInterval);
	vel = new Vector3(0.0f);
	posDot = new Vector3(0.0f);
	velDot = new Vector3(0.0f);
	priorAccel = new Vector3(0.0f);
    }
    // updates the dynamics
    void upDateDynamics()
    {
    	posDot = vel;
	velDot = gyro.userAcceleration;
    }
    //updates the position and the velocity
    void updatePositionVelocity()
    {
    	Vector3 vnext = vel + velDot * Time.deltaTime;
	// R2K for position
	controlledObject.transform.position += 0.5f * (vnext + posDot) *
	Time.deltaTime;
	// Runge Kutta 2 for acceleration too 
	vel += 0.5f * (priorAccel + velDot) * Time.deltaTime;
    }

    void OnGUI()
    {
        GUILayout.Label("");
        GUILayout.Label("Gyroscope attitude : " + gyro.attitude);
        GUILayout.Label("Gyroscope gravity : " + gyro.gravity);
        GUILayout.Label("Gyroscope rotationRate : " + gyro.rotationRate);
        GUILayout.Label("Gyroscope rotationRateUnbiased : " + gyro.rotationRateUnbiased);
        GUILayout.Label("Gyroscope updateInterval : " + gyro.updateInterval);
        GUILayout.Label("Gyroscope userAcceleration : " + gyro.userAcceleration);
        GUILayout.Label("Ref camera rotation:" + qRefObject);
        GUILayout.Label("Ref gyro attitude:" + qRefGyro);
    }

    // LOOK-1.d:
    // Converts the data returned from gyro from right-handed base to left-handed base.
    // Your device may require a different conversion
    private static Quaternion ConvertRotation(Quaternion q)
    {
        return new Quaternion(q.x, q.y,  -q.z, -q.w);
    }
    private static Vector3 ConvertRotation(Vector3 input)
    {
        return new Vector3(input.x, input.y, -input.z);
    }

    // Update is called once per frame
    void Update()
    {
    }
    void FixedUpdate()
    {
        if (controlledObject != null && !Paused)
        {
            // TODO-1.d & TODO-2.a:
            //   rotate the camera or cube based on qRefObject, qRefGyro and current 
            //   data from gyroscope
            Quaternion deltaR = Quaternion.Inverse(qRefGyro) * ConvertRotation(gyro.attitude);
            Vector3 angularRate = ConvertRotation(gyro.rotationRateUnbiased);
    	    Quaternion update = Quaternion.AngleAxis(-Time.deltaTime *
	    angularRate.magnitude * Mathf.Rad2Deg , angularRate);
	    // for part1 and 2 uncomment out this line and the controlled object gets
	    // updated using gyro.attitude.  As it stands the update function uses
	    // the angularRateUnbiased function to get an improved measure of the
	    // rotation rate. 
             //controlledObject.transform.rotation = qRefObject * deltaR;
            controlledObject.transform.rotation =  controlledObject.transform.rotation * update;
	    updateDynamics();
	    updatePositionVelocity();
        }
    }


    public void ResetOrientation()
    {
        if (controlledObject == null)
        {
            return;
        }
        qRefObject = controlledObject.transform.rotation;
        qRefGyro = ConvertRotation(Input.gyro.attitude);
    }

    //// Possible helper function to smooth between gyro and Vuforia
    public void UpdateOrientation(float deltatime)
    {
            float smooth = 1f;
            qRefObject = Quaternion.Slerp(qRefObject, transform.rotation, smooth * deltatime);
            qRefGyro = Quaternion.Slerp(qRefGyro, ConvertRotation(gyro.attitude), smooth * deltatime);
            
    }

}
