using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    public UserData userData;

    public Text winsText;
    public Text losesText;
    public Text dudasCorrectasText;
    public Text dudasIncorrectasText;
    public Text calzadasCorrectasText;
    public Text calzadasIncorrectasText;

    // Start is called before the first frame update
    private void OnEnable() {
        winsText.text = userData.wins.ToString();
        losesText.text = userData.loses.ToString();
        dudasCorrectasText.text = userData.dudasCorrectas.ToString();
        dudasIncorrectasText.text = userData.dudasIncorrectas.ToString();
        calzadasCorrectasText.text = userData.calzadasCorrectas.ToString();
        calzadasIncorrectasText.text = userData.calzadasIncorrectas.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
