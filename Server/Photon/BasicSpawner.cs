﻿using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UltimaQuestSystem.Example;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Collections.Unicode;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    public NetworkPrefabRef _playerPrefab;
    public NetworkPrefabRef _EnemiesPrefab;
    private NetworkRunner _networkRunner;

    public EnemySpawn enemySpawn;

    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();
    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("OnConnectedToServer");
        if (_networkRunner.IsServer)
        {
            Vector3 spawnPosition = new Vector3(0, 0, 0);
            NetworkObject networkObject = runner.Spawn(_EnemiesPrefab, spawnPosition, Quaternion.identity);
        }

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("OnConnectedToServer");

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (PhotonPlayer.local)
        {
            PhotonPlayerInput photonPlayerInput = PhotonPlayer.local.GetComponent<PhotonPlayerInput>();
            if (photonPlayerInput != null)
            {
                input.Set(photonPlayerInput.GetNetworkInput());
            }
        }
        //var data = new NetworkInputData();
        //if (Input.GetKey(KeyCode.W))
        //{
        //    data.direction += Vector3.forward;
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    data.direction += Vector3.back;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    data.direction += Vector3.left;
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    data.direction += Vector3.right;
        //}
        //input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        Debug.Log("có thằng join phòng");
        if (_networkRunner.IsServer)
        {
            Vector3 spawnPosition = new Vector3(0, 0, 0);
            NetworkObject networkObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player.PlayerId);
            _spawnedCharacters.Add(player, networkObject);
        }


    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    async void StartGame(GameMode mode)
    {
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
        Debug.Log("loading nè");
        _networkRunner = gameObject.AddComponent<NetworkRunner>();
        _networkRunner.ProvideInput = true;
        Debug.Log("loading nè2");

        await _networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "testRoom",
            Scene = SceneManager.GetActiveScene().buildIndex,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
        });
        Debug.Log("loading nè3");

        if (_networkRunner.IsServer)
        {
            enemySpawn.SpawnEnemyStart(_networkRunner);
        }
        SceneManager.UnloadScene("LoadingScene");
        Debug.Log("loading xong nè");

    }

    private void OnGUI()
    {
        if (_networkRunner == null)
        {
            if (UserDataManager.Instance.Username == "lucpham566")
            {
                Debug.Log("run server nè");
                StartGame(GameMode.Host);
            }
            else
            {
                StartGame(GameMode.Client);
            }

            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }

            if (GUI.Button(new Rect(0, 60, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }

    private void LoadScenePlay()
    {
        SceneManager.LoadScene("MapScene");
    }
}
