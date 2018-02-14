using UnityEngine;

public class MobileNetwork : Photon.PunBehaviour
{
 
    void Start()
    {
	    PhotonNetwork.ConnectUsingSettings("0.1");
    }
    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
    public override void OnJoinedLobby()
    {
	    PhotonNetwork.JoinRandomRoom();
    }
    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
	    base.OnPhotonJoinRoomFailed(codeAndMsg);
    }
    public override void OnJoinedRoom()
    {
	    Debug.Log("Joined Random Room Success");
	    GameObject obj = PhotonNetwork.Instantiate("PhoneCube", new Vector3(0,0,0), 
			    Quaternion.identity, 0);
	   

        GetComponent<GyroController>().ControlledObject = obj;

	  

            //PhoneCube cube = GetComponent<GyroController>().ControlledObject = cube;
	    base.OnJoinedRoom();
    }

    // TODO-2.a: the same as 1.b
    //   and join a room


    //public override void OnJoinedRoom()
    //{
    //    GetComponent<MobileShooter>().Activate();
    //}


}
