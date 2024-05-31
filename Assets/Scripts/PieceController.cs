using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;

    public float xBoard = -1f;
    public float yBoard = -1f;

    private string player;

    public Sprite black_king, black_knight, black_pawn;
    public Sprite white_king, white_knight, white_pawn;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        switch (this.name) 
        {
            case "black_king": this.GetComponent<SpriteRenderer>().sprite = black_king; break;
            case "black_knight": this.GetComponent<SpriteRenderer>().sprite = black_knight; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; break;
            case "white_king": this.GetComponent<SpriteRenderer>().sprite = white_king; break;
            case "white_knight": this.GetComponent<SpriteRenderer>().sprite = white_knight; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; break;
        }
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x *= 1f;
        y *= 1f;

        switch (x)
        {
            case 0f: x = -7.2f;break;
            case 1f: x = -5.8f;break;
            case 2f: x = -4.4f;break;
            case 3f: x = -2.95f;break;
            case 4f: x = -1.55f;break;
            case 5f: x = -0.15f;break;
            case 6f: x = 1.25f;break;
        }

        if (y==0) y += -4.25f;
        if (y == 7) y += -2.85f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    private void OnMouseUp()
    {
        DestroyMovePlates();
        InitiateMovePlates();
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void InitiateMovePlates() {
        switch (this.name)
        {
            /*case "black_knight":
            case "white_knight":
                LineMovePlate()*/
        }
    }

}
