using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;

    public int xBoard = -1;
    public int yBoard = -1;

    private string player;

    public Sprite black_king, black_bishop, black_pawn;
    public Sprite white_king, white_bishop, white_pawn;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        switch (this.name) 
        {
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; break;
            case "black_bishop": this.GetComponent<SpriteRenderer>().sprite = black_bishop; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; break;
            case "white_bishop": this.GetComponent<SpriteRenderer>().sprite = white_bishop; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; break;
        }
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 1f;
        y *= 1f;

        x += -5f;
        y += -5f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

}
