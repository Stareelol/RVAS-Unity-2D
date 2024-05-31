using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{

    public GameObject chesspiece;

    private GameObject[,] positions = new GameObject[7,7];
    private GameObject[] playerBlack  = new GameObject[7];
    private GameObject[] playerWhite = new GameObject[7];

    private GameObject black_king;
    private GameObject white_king;

    private string currentPlayer = "white";

    private bool gameOver = false;

    // Start is called before the first frame update


    void Start()
    {
        playerWhite = new GameObject[] {
            Create("white_pawn",0,0), Create("white_pawn",1,0), Create("white_knight",2,0), Create("white_king",3,0), Create("white_knight",4,0), Create("white_pawn",5,0), Create("white_pawn",6,0)
        };

        playerBlack = new GameObject[] {
            Create("black_pawn",0,6), Create("black_pawn",1,6), Create("black_knight",2,6), Create("black_king",3,6), Create("black_knight",4,6), Create("black_pawn",5,6), Create("black_pawn",6,6)
        };

        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        } 
    }

    public GameObject Create(string name, int x, int y) 
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        PieceController controller = obj.GetComponent<PieceController>();
        controller.name = name;
        controller.xBoard = x;
        controller.yBoard = y;
        controller.Activate();
        return obj;
    }

    

    public void SetPosition(GameObject obj) 
    {
        PieceController controller = obj.GetComponent<PieceController>();

        positions[(int) controller.xBoard, (int) controller.yBoard] = obj;

    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x , y ];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if (currentPlayer == "white") currentPlayer = "black";
        else currentPlayer = "white";
    }

    public void Update()
    {
        black_king = GameObject.Find("black_king");
        white_king = GameObject.Find("white_king");

        if (black_king == null)
        {
            gameOver = true;
        }
        else if (white_king = null)
        {
            gameOver = true;
        }

        if (gameOver == true)
        {
            SceneManager.LoadScene("SampleScene");
            gameOver = false;
        }

    }
}
