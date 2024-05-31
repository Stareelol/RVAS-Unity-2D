using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeUIScript : MonoBehaviour
{
    public Sprite[] numbers;

    public GameObject piece;

    private void Start()
    {
        int health = GetComponentInParent<PieceController>().health;
        int attack = GetComponentInParent<PieceController>().attack;

        if (this.name == "health")
        {
            switch (health)
            {
                case 0: this.GetComponent<SpriteRenderer>().sprite = numbers[0]; break;
                case 1: this.GetComponent<SpriteRenderer>().sprite = numbers[1]; break;
                case 2: this.GetComponent<SpriteRenderer>().sprite = numbers[2]; break;
                case 3: this.GetComponent<SpriteRenderer>().sprite = numbers[3]; break;
                case 4: this.GetComponent<SpriteRenderer>().sprite = numbers[4]; break;
                case 5: this.GetComponent<SpriteRenderer>().sprite = numbers[5]; break;
                case 6: this.GetComponent<SpriteRenderer>().sprite = numbers[6]; break;
                case 7: this.GetComponent<SpriteRenderer>().sprite = numbers[7]; break;
                case 8: this.GetComponent<SpriteRenderer>().sprite = numbers[8]; break;
                case 9: this.GetComponent<SpriteRenderer>().sprite = numbers[9]; break;
            }
        }
        else {
            switch (attack)
            {
                case 0: this.GetComponent<SpriteRenderer>().sprite = numbers[0]; break;
                case 1: this.GetComponent<SpriteRenderer>().sprite = numbers[1]; break;
                case 2: this.GetComponent<SpriteRenderer>().sprite = numbers[2]; break;
                case 3: this.GetComponent<SpriteRenderer>().sprite = numbers[3]; break;
                case 4: this.GetComponent<SpriteRenderer>().sprite = numbers[4]; break;
                case 5: this.GetComponent<SpriteRenderer>().sprite = numbers[5]; break;
                case 6: this.GetComponent<SpriteRenderer>().sprite = numbers[6]; break;
                case 7: this.GetComponent<SpriteRenderer>().sprite = numbers[7]; break;
                case 8: this.GetComponent<SpriteRenderer>().sprite = numbers[8]; break;
                case 9: this.GetComponent<SpriteRenderer>().sprite = numbers[9]; break;
            }

        }
    }

    private void Update()
    {
        int health = GetComponentInParent<PieceController>().health;

        if (this.name == "health")
            switch (health)
        {
            case 0: this.GetComponent<SpriteRenderer>().sprite = numbers[0]; break;
            case 1: this.GetComponent<SpriteRenderer>().sprite = numbers[1]; break;
            case 2: this.GetComponent<SpriteRenderer>().sprite = numbers[2]; break;
            case 3: this.GetComponent<SpriteRenderer>().sprite = numbers[3]; break;
            case 4: this.GetComponent<SpriteRenderer>().sprite = numbers[4]; break;
            case 5: this.GetComponent<SpriteRenderer>().sprite = numbers[5]; break;
            case 6: this.GetComponent<SpriteRenderer>().sprite = numbers[6]; break;
            case 7: this.GetComponent<SpriteRenderer>().sprite = numbers[7]; break;
            case 8: this.GetComponent<SpriteRenderer>().sprite = numbers[8]; break;
            case 9: this.GetComponent<SpriteRenderer>().sprite = numbers[9]; break;
        }
    }
}
