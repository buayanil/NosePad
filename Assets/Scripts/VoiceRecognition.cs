using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using System;
using UnityEngine.UIElements;

public class VoiceRecognition : MonoBehaviour
{


    private KeywordRecognizer keyword;
    private Dictionary<string, Action> actions = new Dictionary<string, Action>();
    private GameObject square;
    private Button toggle;







    void Start()
    {

        square = GameObject.Find("Square");
        actions.Add("red", ChangeRed);
        actions.Add("green", ChangeGreen);
        actions.Add("blue", ChangeBlue);
        actions.Add("yellow", ChangeYellow);
        actions.Add("black", ChangeBlack);
        actions.Add("light Blue", Changelightblue);
        actions.Add("pink", Changepink);
        actions.Add("gray", ChangeGray);



        keyword = new KeywordRecognizer(actions.Keys.ToArray());
        keyword.OnPhraseRecognized += RecognizedSpeech;
        keyword.Start();
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }

    private void ChangeRed()
    {
        square.GetComponent<Renderer>().material.color = Color.red;
    }

    private void ChangeGreen()
    {
        square.GetComponent<Renderer>().material.color = Color.green;
    }

    private void ChangeBlue()
    {
        square.GetComponent<Renderer>().material.color = Color.blue;
    }

    private void ChangeYellow()
    {
        square.GetComponent<Renderer>().material.color = Color.yellow;
    }

    private void ChangeBlack()
    {
        square.GetComponent<Renderer>().material.color = Color.black;
    }
    private void ChangeGray()
    {
        square.GetComponent<Renderer>().material.color = Color.gray;

    }
    private void Changelightblue()
    {
        square.GetComponent<Renderer>().material.color = Color.cyan;

    }
    private void Changepink()
    {
        square.GetComponent<Renderer>().material.color = Color.magenta;

    }

}
