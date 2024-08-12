using UnityEngine;

public class GameScript : MonoBehaviour
{

    public GameObject chesspiece;

    private GameObject[,] positions = new GameObject[8,8];
    private GameObject[] playerBlack  = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    private GameObject black_king;
    private GameObject white_king;

    public int index_black = 0;
    public int index_white = 0;

    private string currentPlayer = "white";

    private bool gameOver = false;


    public void FENtoPosition(string FEN)
    {
        int row = 7;
        int column = 0;
        int count = 0;

        foreach(char c in FEN)
        {
            switch (c)
            {
                    case 'r': { playerBlack[index_black] = Create("black_rook", column, row); index_black++; }; break;
                    case 'n': { playerBlack[index_black] = Create("black_knight", column, row); index_black++; }; break;
                    case 'b': { playerBlack[index_black] = Create("black_bishop", column, row); index_black++; }; break;
                    case 'q': { playerBlack[index_black] = Create("black_queen", column, row); index_black++; }; break;
                    case 'k': { playerBlack[index_black] = Create("black_king", column, row); index_black++; }; break;
                    case 'p': { playerBlack[index_black] = Create("black_pawn", column, row); index_black++; }; break;
                    case 'R': { playerWhite[index_white] = Create("white_rook", column, row); index_white++; }; break;
                    case 'N': { playerWhite[index_white] = Create("white_knight", column, row); index_white++; }; break;
                    case 'B': { playerWhite[index_white] = Create("white_bishop", column, row); index_white++; }; break;
                    case 'Q': { playerWhite[index_white] = Create("white_queen", column, row); index_white++; }; break;
                    case 'K': { playerWhite[index_white] = Create("white_king", column, row);  index_white++; }; break;
                    case 'P': { playerWhite[index_white] = Create("white_pawn", column, row);  index_white++; }; break;
                    case '/': { row--; column = 0;count++; } ;break;
                default: {
                        //c.ToString();
                        int num = c - '0';
                        column += num - 1;
                        }
                    break;
            }
            if (c!='/')column++;
            //if ()
        }
    }

    void Start()
    {
        FENtoPosition("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR"); 
        //FENtoPosition("4k2r/6r1/8/8/8/8/3R4/R3K3");

        for (int i = 0; i < index_black; i++)
        {
            SetPosition(playerBlack[i]);
        }

        for (int i = 0; i < index_white; i++)
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
