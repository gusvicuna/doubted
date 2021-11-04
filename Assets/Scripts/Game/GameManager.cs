using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public UserData currentUserData;
    public GameData currentGameData;
    public PlayerData currentPlayerData;

    public GameInfo gameInfo;
    public Dropdown numberDropdown;
    public Dropdown quantityDropdown;
    public PlayerDice currentPlayerDice;
    public List<PlayerDice> playersDice;
    public GameObject turnPanel;

    public bool updating = true;
    public int updateCooldown = 5;

    private OnlineManager _onlineManager;

    // Start is called before the first frame update
    void Start()
    {
        _onlineManager = OnlineManager.singleton;

        currentUserData = new UserData();
        currentPlayerData = new PlayerData();
        currentGameData = new GameData();

        currentGameData.id = PlayerPrefs.GetInt("currentGameId");
        currentUserData.id = PlayerPrefs.GetInt("currentPlayerId");

        UpdateDatas();

    }
    public void UpdateDatas() {
        StartCoroutine(UpdateData());
    }
    public IEnumerator UpdateData() {
        while (updating)
        {

            StartCoroutine(_onlineManager.GetUser(PlayerPrefs.GetString("username"), result =>
            {
                currentUserData.id = result["id"];
                currentUserData.username = result["username"];
            }));

            yield return new WaitForSeconds(1);

            StartCoroutine(_onlineManager.GetGame(currentGameData.id.ToString(), result =>
            {
                Debug.Log(result);
                currentGameData.gameName = result["gameName"];
                currentGameData.password = result["passWord"];
                currentGameData.maxPlayers = result["maxPlayers"];
                currentGameData.round = result["round"];
                currentGameData.playerTurn = result["playerTurn"];
                currentGameData.gameStarted = result["gameStarted"];
                currentGameData.obligando = result["obligando"];
                currentGameData.newRound = result["newRound"];
                currentGameData.canCalzar = result["canCalzar"];
                currentGameData.canObligar = result["canObligar"];
            }));

            yield return new WaitForSeconds(1);

            StartCoroutine(_onlineManager.GetPlayers(currentGameData.id.ToString(), result => {
                foreach (SimpleJSON.JSONNode player in result) {
                    PlayerData player2 = new PlayerData();
                    player2.haObligado = player["haObligado"];
                    player2.acceptationState = player["acceptationState"];
                    player2.turnNumber = player["turnNumber"];
                    player2.name = player["name"];
                    foreach (SimpleJSON.JSONNode dado in player["dados"])
                    {
                        player2.dice.Add((pinta)(int)dado);
                    }

                    PredictionData newPredictionData = new PredictionData();
                    newPredictionData.type = player["lastPType"];
                    newPredictionData.quantity = player["lastPQuantity"];
                    newPredictionData.number = (pinta) (int)player["lastPPinta"];
                    player2.lastPrediction = newPredictionData;

                    currentGameData.players.Add(player2);
                }
            }));

            yield return new WaitForSeconds(1);

            currentPlayerData = currentGameData.players.Find(p => p.userId == currentUserData.id);

            if (!currentGameData.gameStarted)
            {
                NewRound();
                currentGameData.gameStarted = true;
            }

            SetGameInfo();
            SetPlayerDice();

            if (currentGameData.playerTurn == currentPlayerData.turnNumber) turnPanel.SetActive(true);
            if (currentGameData.newRound)
            {
                turnPanel.transform.Find("Doubt Button").gameObject.SetActive(false);
                turnPanel.transform.Find("Calzar Button").gameObject.SetActive(false);
            }
            if(!currentGameData.canCalzar) turnPanel.transform.Find("Calzar Button").gameObject.SetActive(false);

            yield return new WaitForSeconds(updateCooldown);
        }
    }
    private void SetGameInfo()
    {
        int playersLeft = 0;
        int diceLeft = 0;
        foreach (var playerData in currentGameData.players) {
            if (playerData.dice.Count > 0) playersLeft += 1;
            diceLeft += playerData.dice.Count;
        }

        gameInfo.SetInitialInfo(currentGameData.maxPlayers, playersLeft, currentGameData.maxPlayers * 5, diceLeft, currentGameData.round);
        gameInfo.SetPlayers(currentGameData.players,currentGameData.playerTurn);
    }

    private void SetPlayerDice()
    {
        for (int i = 0; i < currentGameData.players.Count; i++) {
            Debug.Log(currentGameData.players[i].dice.Count);
            playersDice[i].SetDice(currentGameData.players[i], false);
        }

        currentPlayerDice.SetDice(currentPlayerData, true);
    }

    public void ShowAllDice()
    {
        for (int i = 0; i < currentGameData.players.Count; i++) {
            playersDice[i].SetDice(currentGameData.players[i], true);
        }
    }


    #region Actions

    public void MakePrediction() {
        PredictionData predictionData = new PredictionData();
        predictionData.type = "prediction";
        predictionData.quantity = quantityDropdown.value + 1;
        predictionData.number = (pinta)(numberDropdown.value + 1);
        currentPlayerData.lastPrediction = predictionData;

        NextTurn();

        SetPlayerDice();
        SetGameInfo();
    }

    public void Doubt()
    {
        StartCoroutine(IDoubt());
    }
    public IEnumerator IDoubt() {
        PredictionData predictionData = new PredictionData();
        predictionData.type = "doubt";
        currentPlayerData.lastPrediction = predictionData;

        ShowAllDice();

        yield return new WaitForSeconds(8);

        int lastTurn = currentPlayerData.turnNumber - 1;
        PlayerData lastPlayerData;
        while (true) {
            if (lastTurn <= 0) lastTurn = currentGameData.maxPlayers;
            lastPlayerData = currentGameData.players.Find(p => p.turnNumber == lastTurn);
            if (lastPlayerData.dice.Count > 0) break;
        }

        int trueQuantity = 0;
        foreach (var playerData in currentGameData.players) {
            foreach (var playerDataDie in playerData.dice) {
                if (playerDataDie.Equals(lastPlayerData.lastPrediction.number) || playerDataDie.Equals(pinta.As))
                    trueQuantity += 1;
            }
        }
        Debug.Log(trueQuantity);

        LoseRound(trueQuantity >= lastPlayerData.lastPrediction.quantity ? currentPlayerData : lastPlayerData);

        SetPlayerDice();
        SetGameInfo();
    }

    public void Calzar() {
        PredictionData predictionData = new PredictionData();
        predictionData.type = "calzar";
        currentPlayerData.lastPrediction = predictionData;

        ShowAllDice();

        int lastTurn = currentPlayerData.turnNumber - 1;
        PlayerData lastPlayerData;
        while (true) {
            if (lastTurn <= 0) lastTurn = currentGameData.maxPlayers;
            lastPlayerData = currentGameData.players.Find(p => p.turnNumber == lastTurn);
            if (lastPlayerData.dice.Count > 0) break;
        }

        int trueQuantity = 0;
        foreach (var playerData in currentGameData.players) {
            foreach (var playerDataDie in playerData.dice) {
                if (playerDataDie.Equals(lastPlayerData.lastPrediction.number) || playerDataDie.Equals(pinta.As))
                    trueQuantity += 1;
            }
        }
        Debug.Log(trueQuantity);

        if (trueQuantity == lastPlayerData.lastPrediction.quantity) WinRound();
        else LoseRound(currentPlayerData);

        SetPlayerDice();
        SetGameInfo();
    }

    #endregion

    private void WinRound() {
        if (currentPlayerData.dice.Count < 5) currentPlayerData.dice.Add(pinta.As);

        currentGameData.playerTurn = currentPlayerData.turnNumber;

        NewRound();
    }

    private void LoseRound(PlayerData playerData) {
        playerData.dice.RemoveAt(0);
        Debug.Log(playerData.dice.Count);
        currentGameData.playerTurn = playerData.turnNumber;

        NewRound();
    }

    private void NewRound() {
        foreach (var playerData in currentGameData.players)
        {
            playerData.lastPrediction = null;
            int numberDice = playerData.dice.Count;
            playerData.dice = new List<pinta>();
            for (int i = 0; i < numberDice; i++) {
                pinta die = (pinta)Random.Range(1, 6);
                playerData.dice.Add(die);
            }
        }

        currentGameData.round++;
    }

    private void NextTurn()
    {
        currentGameData.playerTurn += 1;
        if(currentGameData.players[currentGameData.playerTurn].dice.Count == 0) NextTurn();
        if (currentGameData.playerTurn >= currentGameData.players.Count) currentGameData.playerTurn = 0;
    }

    

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
