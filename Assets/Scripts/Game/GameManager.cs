using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Game
{
    public class GameManager : MonoBehaviour
    {
        #region Public Attributes

        [Header("GameObjects")]
        public GameInfo gameInfo;
        public Dropdown numberDropdown;
        public Dropdown quantityDropdown;
        public PlayerDice currentPlayerDice;
        public List<PlayerDice> playersDice;
        public GameObject turnPanel;
        public Text winText;
        public Text loseText;

        [Header("Update Settings")]
        public bool updating = true;
        public int updateCooldown = 5;

        #endregion


        #region Private Attributes

        private UserData _currentUserData;
        private GameData _currentGameData;
        private PlayerData _currentPlayerData;

        private OnlineManager _onlineManager;

        #endregion


        #region Monobehaviour Callbacks

        private void Start() {
            _onlineManager = OnlineManager.singleton;

            _currentUserData = new UserData();
            _currentPlayerData = new PlayerData();
            _currentGameData = new GameData();

            _currentGameData.id = PlayerPrefs.GetInt("currentGameId");
            _currentUserData.id = PlayerPrefs.GetInt("currentPlayerId");

            UpdateData();

        }

        #endregion


        #region Actions

        public void MakePrediction() {
            PredictionData predictionData = new PredictionData();
            predictionData.type = "prediction";
            predictionData.quantity = quantityDropdown.value + 1;
            predictionData.number = (pinta)(numberDropdown.value + 1);
            _currentPlayerData.lastPrediction = predictionData;

            NextTurn();

            UploadToDB();

            SetPlayerDice();
            SetGameInfo();
        }

        public void DoubtAction()
        {
            StartCoroutine(Doubt());
        }
        public IEnumerator Doubt() {
            PredictionData predictionData = new PredictionData();
            predictionData.type = "doubt";
            _currentPlayerData.lastPrediction = predictionData;

            ShowAllDice();

            yield return new WaitForSeconds(8);

            int lastTurn = _currentPlayerData.turnNumber - 1;
            PlayerData lastPlayerData;
            while (true) {
                if (lastTurn <= 0) lastTurn = _currentGameData.maxPlayers;
                lastPlayerData = _currentGameData.players.Find(p => p.turnNumber == lastTurn);
                if (lastPlayerData.dados.Count > 0) break;
            }

            int trueQuantity = 0;
            foreach (var playerData in _currentGameData.players) {
                foreach (var playerDataDie in playerData.dados) {
                    if (playerDataDie.Equals(lastPlayerData.lastPrediction.number) || playerDataDie.Equals(pinta.As))
                        trueQuantity += 1;
                }
            }
            Debug.Log(trueQuantity);

            if (trueQuantity >= lastPlayerData.lastPrediction.quantity)
            {
                _currentUserData.dudasIncorrectas += 1;
                LoseRound(_currentPlayerData);
            }
            else
            {
                _currentUserData.dudasCorrectas += 1;
                LoseRound(lastPlayerData);
            }
            LoseRound(trueQuantity >= lastPlayerData.lastPrediction.quantity ? _currentPlayerData : lastPlayerData);

            UploadToDB();

            SetPlayerDice();
            SetGameInfo();
        }

        public void Calzar() {
            PredictionData predictionData = new PredictionData();
            predictionData.type = "calzar";
            _currentPlayerData.lastPrediction = predictionData;

            ShowAllDice();

            int lastTurn = _currentPlayerData.turnNumber - 1;
            PlayerData lastPlayerData;
            while (true) {
                if (lastTurn <= 0) lastTurn = _currentGameData.maxPlayers;
                lastPlayerData = _currentGameData.players.Find(p => p.turnNumber == lastTurn);
                if (lastPlayerData.dados.Count > 0) break;
            }

            int trueQuantity = 0;
            foreach (var playerData in _currentGameData.players) {
                foreach (var playerDataDie in playerData.dados) {
                    if (playerDataDie.Equals(lastPlayerData.lastPrediction.number) || playerDataDie.Equals(pinta.As))
                        trueQuantity += 1;
                }
            }
            Debug.Log(trueQuantity);

            if (trueQuantity == lastPlayerData.lastPrediction.quantity)
            {
                _currentUserData.calzadasCorrectas += 1;
                WinRound();
            }
            else
            {
                _currentUserData.calzadasIncorrectas += 1;
                LoseRound(_currentPlayerData);
            }

            UploadToDB();

            SetPlayerDice();
            SetGameInfo();
        }

        #endregion


        #region Public Methods
        private void SetGameInfo() {
            int playersLeft = 0;
            int diceLeft = 0;
            foreach (var playerData in _currentGameData.players) {
                if (playerData.dados.Count > 0) playersLeft += 1;
                diceLeft += playerData.dados.Count;
            }

            gameInfo.SetInitialInfo(_currentGameData.maxPlayers, playersLeft, _currentGameData.maxPlayers * 5, diceLeft, _currentGameData.round);
            gameInfo.SetPlayersInfo(_currentGameData.players, _currentGameData.playerTurn);
        }

        private void SetPlayerDice() {
            for (int i = 0; i < _currentGameData.players.Count; i++) {
                playersDice[i].SetDice(_currentGameData.players[i], false);
            }
            currentPlayerDice.SetDice(_currentPlayerData, true);
        }

        public void ShowAllDice() {
            for (int i = 0; i < _currentGameData.players.Count; i++) {
                playersDice[i].SetDice(_currentGameData.players[i], true);
            }
        }

        #endregion
        private void WinRound() {
            if (_currentPlayerData.dados.Count < 5) _currentPlayerData.dados.Add(1);

            _currentGameData.playerTurn = _currentPlayerData.turnNumber;

            NewRound();
        }

        private void LoseRound(PlayerData playerData) {
            playerData.dados.RemoveAt(0);
            Debug.Log(playerData.dados.Count);
            _currentGameData.playerTurn = playerData.turnNumber;

            NewRound();
        }

        private void NewRound() {
            foreach (var playerData in _currentGameData.players)
            {
                playerData.lastPrediction = null;
                int numberDice = playerData.dados.Count;
                playerData.dados = new List<int>();
                for (int i = 0; i < numberDice; i++) {
                    int die = Random.Range(1, 6);
                    playerData.dados.Add(die);
                }
            }

            _currentGameData.round++;
        }

        private void NextTurn()
        {
            updating = true;
            _currentGameData.playerTurn += 1;
            if(_currentGameData.players[_currentGameData.playerTurn].dados.Count == 0) NextTurn();
            if (_currentGameData.playerTurn >= _currentGameData.players.Count) _currentGameData.playerTurn = 0;
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene");
        }

        #region Data Management

        public IEnumerator UpdateDataCoroutine() {
            while (updating)
            {
                _currentUserData = new UserData();
                _currentGameData = new GameData();
                _currentPlayerData = new PlayerData();
                StartCoroutine(_onlineManager.GetUser(PlayerPrefs.GetString("username"), result => {
                    _currentUserData.id = result["id"];
                    _currentUserData.username = result["username"];

                    Debug.Log("User: " + _currentUserData.Stringify());
                }));

                yield return new WaitForSeconds(1);

                StartCoroutine(_onlineManager.GetGame(_currentGameData.id.ToString(), result => {
                    _currentGameData.gameName = result["gameName"];
                    _currentGameData.password = result["passWord"];
                    _currentGameData.maxPlayers = result["maxPlayers"];
                    _currentGameData.round = result["round"];
                    _currentGameData.playerTurn = result["playerTurn"];
                    _currentGameData.gameStarted = result["gameStarted"];
                    _currentGameData.obligando = result["obligando"];
                    _currentGameData.newRound = result["newRound"];
                    _currentGameData.canCalzar = result["canCalzar"];
                    _currentGameData.canObligar = result["canObligar"];

                    Debug.Log("Game: " + _currentGameData.Stringify());
                }));

                yield return new WaitForSeconds(1);

                StartCoroutine(_onlineManager.GetPlayers(_currentGameData.id.ToString(), result => {
                    foreach (SimpleJSON.JSONNode playerJson in result) {
                        

                        PlayerData playerData = new PlayerData();
                        playerData.id = playerJson["id"];
                        playerData.gameId = _currentGameData.id;
                        playerData.userId = playerJson["userId"];
                        playerData.haObligado = playerJson["haObligado"];
                        playerData.acceptationState = playerJson["acceptationState"];
                        playerData.turnNumber = playerJson["turnNumber"];
                        playerData.name = playerJson["name"];
                        if (playerJson["dado1"] != -1) playerData.dados.Add(playerJson["dado1"]);
                        if (playerJson["dado2"] != -1) playerData.dados.Add(playerJson["dado2"]);
                        if (playerJson["dado3"] != -1) playerData.dados.Add(playerJson["dado3"]);
                        if (playerJson["dado4"] != -1) playerData.dados.Add(playerJson["dado4"]);
                        if (playerJson["dado5"] != -1) playerData.dados.Add(playerJson["dado5"]);

                        PredictionData newPredictionData = new PredictionData();
                        newPredictionData.type = playerJson["lastPType"];
                        newPredictionData.quantity = playerJson["lastPQuantity"];
                        newPredictionData.number = (pinta)(int)playerJson["lastPPinta"];
                        playerData.lastPrediction = newPredictionData;

                        if (playerJson["userId"] == _currentUserData.id)
                        {
                            _currentPlayerData = playerData;
                        }
                        Debug.Log("Player: " + playerData.Stringify());

                        _currentGameData.players.Add(playerData);
                    }
                }));

                yield return new WaitForSeconds(1);

                //Lógica
                if (!_currentGameData.gameStarted) {
                    NewRound();
                    _currentGameData.gameStarted = true;
                }

                SetGameInfo();
                SetPlayerDice();

                if (_currentGameData.gameFinished) {
                    if (_currentPlayerData.dados.Count > 0) {
                        winText.gameObject.SetActive(true);
                        updating = false;
                    }
                    else {
                        loseText.gameObject.SetActive(true);
                        updating = false;
                    }
                }
                if (_currentGameData.playerTurn == _currentPlayerData.turnNumber) {
                    updating = false;
                    turnPanel.SetActive(true);
                    if (_currentGameData.newRound) {
                        turnPanel.transform.Find("Doubt Button").gameObject.SetActive(false);
                        turnPanel.transform.Find("Calzar Button").gameObject.SetActive(false);
                    }
                    if (!_currentGameData.canCalzar) turnPanel.transform.Find("Calzar Button").gameObject.SetActive(false);
                }


                yield return new WaitForSeconds(updateCooldown);
            }
        }
        public void UploadToDB() {
            StartCoroutine(_onlineManager.PutGame(_currentGameData.id.ToString(), _currentGameData.Stringify()));

            foreach (PlayerData playerData in _currentGameData.players) {
                if (playerData.dados.Count >= 1) playerData.dado1 = playerData.dados[0];
                else playerData.dado1 = -1;
                if (playerData.dados.Count >= 2) playerData.dado2 = playerData.dados[1];
                else playerData.dado2 = -1;
                if (playerData.dados.Count >= 3) playerData.dado3 = playerData.dados[2];
                else playerData.dado3 = -1;
                if (playerData.dados.Count >= 4) playerData.dado4 = playerData.dados[3];
                else playerData.dado4 = -1;
                if (playerData.dados.Count >= 5) playerData.dado5 = playerData.dados[4];
                else playerData.dado5 = -1;
                StartCoroutine(_onlineManager.PutPlayer(playerData.id.ToString(), playerData.Stringify()));
            }
            StartCoroutine(_onlineManager.PutUser(_currentUserData.id.ToString(), _currentUserData.Stringify()));
        }
        public void UpdateData() {
            StartCoroutine(UpdateDataCoroutine());
        }

        #endregion

    }
}
