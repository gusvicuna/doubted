using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDice : MonoBehaviour
{
    public GameObject die1;
    public GameObject die2;
    public GameObject die3;
    public GameObject die4;
    public GameObject die5;

    public Sprite dice0Sprite;
    public Sprite dice1Sprite;
    public Sprite dice2Sprite;
    public Sprite dice3Sprite;
    public Sprite dice4Sprite;
    public Sprite dice5Sprite;
    public Sprite dice6Sprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDice(PlayerData player, bool show)
    {
        if (player.dice.Count == 5)
        {
            die5.SetActive(true);
            if (show) SetDieSprite(die5.GetComponentInChildren<Image>(), player.dice[4]);
            else SetDieSprite(die5.GetComponentInChildren<Image>());
        }
        else die5.SetActive(false);
        if (player.dice.Count >= 4) {
            die4.SetActive(true);
            if (show) SetDieSprite(die4.GetComponentInChildren<Image>(), player.dice[3]);
            else SetDieSprite(die4.GetComponentInChildren<Image>());
        }
        else die4.SetActive(false);
        if (player.dice.Count >= 3) {
            die3.SetActive(true);
            if (show) SetDieSprite(die3.GetComponentInChildren<Image>(), player.dice[2]);
            else SetDieSprite(die3.GetComponentInChildren<Image>());
        }
        else die3.SetActive(false);
        if (player.dice.Count >= 2) {
            die2.SetActive(true);
            if (show) SetDieSprite(die2.GetComponentInChildren<Image>(), player.dice[1]);
            else SetDieSprite(die2.GetComponentInChildren<Image>());
        }
        else die2.SetActive(false);
        if (player.dice.Count >= 1) {
            die1.SetActive(true);
            if (show) SetDieSprite(die1.GetComponentInChildren<Image>(), player.dice[0]);
            else SetDieSprite(die1.GetComponentInChildren<Image>());
        }
        else die1.SetActive(false);
    }

    public void SetDieSprite(Image die, pinta pinta = pinta.Unknown)
    {
        die.sprite = pinta switch
        {
            pinta.As => dice1Sprite,
            pinta.Tonto => dice2Sprite,
            pinta.Tren => dice3Sprite,
            pinta.Cuarta => dice4Sprite,
            pinta.Quinta => dice5Sprite,
            pinta.Sexta => dice6Sprite,
            _ => dice0Sprite
        };
    }
}
