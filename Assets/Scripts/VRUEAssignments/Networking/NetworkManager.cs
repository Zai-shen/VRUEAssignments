using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields
    
    [SerializeField] 
    private string role = "host";

    [Tooltip("The maximum number of players per room")] [SerializeField]
    private byte maxPlayersPerRoom = 4;

    [Tooltip("The UI Loader Anime")] [SerializeField]
    private LoaderAnimation loaderAnimation;

    #endregion

    #region Private Fields

    private const string ROLE_HOST = "host";
    private const string ROLE_MEMBER = "member";
    private const string ROLE_OBSERVER = "observer";
    
    
    [Tooltip("The prefab to use for representing the player")]
    [SerializeField]
    private GameObject playerPrefab;
    
    /// <summary>
    /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon, 
    /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
    /// Typically this is used for the OnConnectedToMaster() callback.
    /// </summary>
    private bool isConnecting;

    /// <summary>
    /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
    /// </summary>
    private string gameVersion = "1";

    #endregion

    #region MonoBehaviour CallBacks
    
    private void Awake()
    {
        if (loaderAnimation == null)
        {
            Debug.LogError("<Color=red><b>Missing</b></Color> loaderAnimation Reference.", this);
        }

        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.NickName = role;

        Connect();
    }
    
    #endregion


    #region Public Methods

    /// <summary>
    /// Start the connection process. 
    /// - If already connected, we attempt joining a random room
    /// - if not yet connected, Connect this application instance to Photon Cloud Network
    /// </summary>
    public void Connect()
    {
        // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
        isConnecting = true;

        // start the loader animation for visual effect.
        if (loaderAnimation != null)
        {
            loaderAnimation.StartLoaderAnimation();
        }

        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.IsConnected)
        {
            LogFeedback("Joining Room...");
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            LogFeedback("Connecting...");

            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = this.gameVersion;

            role = "client";
            PhotonNetwork.NickName = role;
        }
    }
    
    private static void LogFeedback(string message)
    {
        if (UIManager.Instance == null)
        {
            return;
        }

        UIManager.Instance.DisplayUIMessage(System.Environment.NewLine + message);
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null) {
            Debug.LogError("<color=red><b>Missing</b></color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        } else {
            if (PhotonNetwork.InRoom && PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                PhotonNetwork.Instantiate("Prefabs/Core/" + this.playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
            }else{
                Debug.LogFormat("Ignoring instantiation in {0} as we already have a player!", SceneManagerHelper.ActiveSceneName);
            }
        }
    }
    
    #endregion


    #region MonoBehaviourPunCallbacks CallBacks

    /// <summary>
    /// Called after the connection to the master is established and authenticated
    /// </summary>
    public override void OnConnectedToMaster()
    {
        // we don't want to do anything if we are not attempting to join a room. 
        // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
        // we don't want to do anything.
        if (isConnecting)
        {
            LogFeedback("OnConnectedToMaster: Next -> try to Join Random Room");
            Debug.Log(
                "OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");

            // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
            PhotonNetwork.JoinRandomRoom();
        }
    }

    /// <summary>
    /// Called when a JoinRandom() call failed. The parameter provides ErrorCode and message.
    /// </summary>
    /// <remarks>
    /// Most likely all rooms are full or no rooms are available. <br/>
    /// </remarks>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        LogFeedback("<color=red>OnJoinRandomFailed</color>: Next -> Create a new Room");
        Debug.Log(
            "OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = this.maxPlayersPerRoom});
    }


    /// <summary>
    /// Called after disconnecting from the Photon server.
    /// </summary>
    public override void OnDisconnected(DisconnectCause cause)
    {
        LogFeedback("<color=red>OnDisconnected</color> " + cause);
        Debug.LogError("Disconnected");

        // #Critical: we failed to connect or got disconnected. There is not much we can do. Typically, a UI system should be in place to let the user attemp to connect again.
        loaderAnimation.StopLoaderAnimation();

        isConnecting = false;
    }

    /// <summary>
    /// Called when entering a room (by creating or joining it). Called on all clients (including the Master Client).
    /// </summary>
    /// <remarks>
    /// This method is commonly used to instantiate player characters.
    /// If a match has to be started "actively", you can call an [PunRPC](@ref PhotonView.RPC) triggered by a user's button-press or a timer.
    ///
    /// When this is called, you can usually already access the existing players in the room via PhotonNetwork.PlayerList.
    /// Also, all custom properties should be already available as Room.customProperties. Check Room..PlayerCount to find out if
    /// enough players are in the room to start playing.
    /// </remarks>
    public override void OnJoinedRoom()
    {
        LogFeedback("<color=green>OnJoinedRoom</color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " Player(s)");
        Debug.Log(
            "OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");

        // #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.AutomaticallySyncScene to sync our instance scene.
        int users = PhotonNetwork.CurrentRoom.PlayerCount;
        if (users == 1)
        {
        }else if (users == 2)
        {
            role = ROLE_MEMBER;
        }
        else
        {
            role = ROLE_OBSERVER;
        }

        Debug.Log($"We are {role}");

        SpawnPlayer();
        
        loaderAnimation.StopLoaderAnimation();
    }

    #endregion
}