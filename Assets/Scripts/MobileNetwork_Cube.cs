using UnityEngine;

public class MobileNetwork_Cube : Photon.PunBehaviour
{
    //  using PCNetwork_Cube.  write any functions needed to establish connection
    //   and join a room. Joining a random room will do for now if you are testing
    //   it yourself. But you can also list the rooms or require player to enter
    //   the room name in case there are more people playing
    //   your game - though it is not required for the assignment
    void Start()
    {
	    PhotonNetwork.ConnectUsingSettings("0.1");
    }



    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	//GUILayout.Label("Room name: " + roomName);
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

          var cube = GetComponent<GyroController>().ControlledObject = obj;

            //PhoneCube cube = GetComponent<GyroController>().ControlledObject = cube;
	    base.OnJoinedRoom();
    }

    //public override void OnJoinedRoom()
    //{
    //    //TODO-1.c: use PhotonNetwork.Instantiate to create a "PhoneCube" across the network
    //}


}
