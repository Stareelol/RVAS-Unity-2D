using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MySql.Data.MySqlClient;

public class GameScript : MonoBehaviour
{

    public GameObject chesspiece;

    private GameObject[,] positions = new GameObject[8,8];
    private GameObject[] playerBlack  = new GameObject[8];
    private GameObject[] playerWhite = new GameObject[8];

    private GameObject black_king;
    private GameObject white_king;

    private string currentPlayer = "white";

    private bool gameOver = false;


    void Start()
    {

        playerWhite = new GameObject[] {
            // classical chess
            //Create("white_rook", 0, 0), Create("white_knight", 1, 0), Create("white_bishop", 2, 0), Create("white_queen", 3, 0), Create("white_king", 4, 0), Create("white_bishop", 5, 0), Create("white_knight", 6, 0), Create("white_rook", 7, 0), Create("white_pawn", 0, 1), Create("white_pawn", 1, 1), Create("white_pawn", 2, 1), Create("white_pawn", 3, 1), Create("white_pawn", 4, 1), Create("white_pawn", 5, 1), Create("white_pawn", 6, 1), Create("white_pawn",7, 1)
            
            Create("white_king", 0, 3),Create("white_rook",1,4)
        };

        playerBlack = new GameObject[] {
            // clasical chess
            //Create("black_rook", 0, 7), Create("black_knight", 1, 7), Create("black_bishop", 2, 7), Create("black_king", 3, 7), Create("black_queen", 4, 7), Create("black_bishop", 5, 7), Create("black_knight", 6, 7), Create("black_rook", 7, 7), Create("black_pawn", 0, 6), Create("black_pawn", 1, 6), Create("black_pawn", 2, 6), Create("black_pawn", 3, 6), Create("black_pawn", 4, 6), Create("black_pawn", 5, 6), Create("black_pawn", 6, 6), Create("black_pawn",7, 6)
            
            // testing positions
            
        };

        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
        }

        for (int i = 0; i < playerWhite.Length; i++)
        {
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

        positions[controller.xBoard, controller.yBoard] = obj;

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

        //if (black_king == null)
        //{
        //    gameOver = true;
        //}
        //if (white_king == null)
        //{
        //    gameOver = true;
        //}

        //if (gameOver == true)
        //{
        //    SceneManager.LoadScene("Main Menu");
        //    gameOver = false;
        //}
    }
}
