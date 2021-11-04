using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInfo : MonoBehaviour
{
    public Text turnText;
    public Text playersLeftText;
    public Text playersTotalText;
    public Text diceLeftText;
    public Text diceTotalText;
    public GameObject PlayersInfoPanel;

    public GameObject playerInfoPrefab;

    public List<Sprite> dice;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInitialInfo(int playersTotal, int playersLeft, int diceTotal, int diceLeft, int turn)
    {
        turnText.text = turn.ToString();
        playersLeftText.text = playersLeft.ToString();
        playersTotalText.text = playersTotal.ToString();
        diceLeftText.text = diceLeft.ToString();
        diceTotalText.text = diceTotal.ToString();
    }

    public void SetPlayers(List<PlayerData> players, int playerTurn)
    {
        foreach (Transform child in PlayersInfoPanel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var playerData in players)
        {
            GameObject playerInfoPanel = Instantiate(playerInfoPrefab, PlayersInfoPanel.transform);
            if(playerData.turnNumber==playerTurn) playerInfoPanel.GetComponent<Image>().color = Color.green;
            playerInfoPanel.transform.Find("PlayerName Text").GetComponent<Text>().text = playerData.name;
            playerInfoPanel.transform.Find("NumberDice Text").GetComponent<Text>().text = playerData.dice.Count.ToString();
            if (playerData.lastPrediction != null)
            {
                if (playerData.lastPrediction.type == "prediction") {
                    playerInfoPanel.transform.Find("LastAction Text").GetComponent<Text>().text =
                        playerData.lastPrediction.quantity.ToString();
                    playerInfoPanel.transform.Find("Image").gameObject.SetActive(true);
                    playerInfoPanel.transform.Find("Image").GetComponent<Image>().sprite =
                        dice[(int)playerData.lastPrediction.number];
                }
                else {
                    playerInfoPanel.transform.Find("LastAction Text").GetComponent<Text>().text =
                        playerData.lastPrediction.type;
                }
            }
        }
    }
}
