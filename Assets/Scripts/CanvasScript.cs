using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{

    public Sprite QueenWhitePromote, QueenBlackPromote, BishopWhitePromote, BishopBlackPromote, RookWhitePromote, RookBlackPromote, KnightWhitePromote, KnightBlackPromote;

    public bool QueenPress, RookPress, KnightPress, BishopPress;

    GameObject controller;

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Canvas>().enabled = false;
        controller = GameObject.FindGameObjectWithTag("GameController");
    }

    public void ShowPromoteScreen(string player)
    {
        Button QueenPromote, RookPromote, BishopPromote, KnightPromote;

        QueenPromote = GameObject.Find("QueenPromote").GetComponent<Button>();
        RookPromote = GameObject.Find("RookPromote").GetComponent<Button>();
        BishopPromote = GameObject.Find("BishopPromote").GetComponent<Button>();
        KnightPromote = GameObject.Find("KnightPromote").GetComponent<Button>();

        switch (player)
        {
            case "white":
                {
                    QueenPromote.image.sprite = QueenWhitePromote;
                    BishopPromote.image.sprite = BishopWhitePromote;
                    RookPromote.image.sprite = RookWhitePromote;
                    KnightPromote.image.sprite = KnightWhitePromote;
                }
                break;
            case "black":
                {
                    QueenPromote.image.sprite = QueenBlackPromote;
                    BishopPromote.image.sprite = BishopBlackPromote;
                    RookPromote.image.sprite = RookBlackPromote;
                    KnightPromote.image.sprite = KnightBlackPromote;
                }
                break;
        }
    }

    public void OnQueenPromoteClick()
    {
        QueenPress = true;
        GameScript gs = controller.GetComponent<GameScript>();
        gs.button_pressed = true;
        this.GetComponent<Canvas>().enabled = false; 
    }

    public void OnRookPromoteClick()
    {
        RookPress = true;
        GameScript gs = controller.GetComponent<GameScript>();
        gs.button_pressed = true;
        this.GetComponent<Canvas>().enabled = false;
    }

    public void OnBishopPromoteClick()
    {
        BishopPress = true;
        GameScript gs = controller.GetComponent<GameScript>();
        gs.button_pressed = true;
        this.GetComponent<Canvas>().enabled = false;
    }

    public void OnKnightPromoteClick()
    {
        KnightPress = true;
        GameScript gs = controller.GetComponent<GameScript>();
        gs.button_pressed = true;
        this.GetComponent<Canvas>().enabled = false;
    }
}
