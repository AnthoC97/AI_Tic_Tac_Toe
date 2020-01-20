using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IA : MonoBehaviour
{
    private int[,] matrix;
    List<int> choice = new List<int>();
    private GameObject gm;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int BestMove(int player, int[,] state, List<Image> grid)
    {
        matrix = state;
        int n = matrix.GetLength(0);
        choice.Clear();

        //Verification coup Gagnant
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if(matrix[i,j]==8)
                {
                    choice.Add(3 * i + j);
                    matrix[i, j] = player;
                    if(win(player))
                    {
                        return 3 * i + j;
                    }
                    matrix[i, j] = 8;
                }
            }
        }

        //Coup perdant
        int adversaire = 1;

        if(player == 1)
        {
            adversaire = 0;
        }
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (matrix[i, j] == 8)
                {

                    matrix[i, j] = adversaire;
                    if (win(adversaire))
                    {
                        return 3 * i + j;
                    }
                    matrix[i, j] = 8;
                }
            }
        }

        //Aléatoire
        Debug.Log(choice.Count);
        return choice[Random.Range(0, choice.Count)];
    }

    public bool win(int player)
    {
        if (matrix[0, 0] == player && matrix[0, 1] == player && matrix[0, 2] == player ||
            matrix[1, 0] == player && matrix[1, 1] == player && matrix[1, 2] == player ||
            matrix[2, 0] == player && matrix[2, 1] == player && matrix[2, 2] == player ||

            matrix[0, 0] == player && matrix[1, 1] == player && matrix[2, 2] == player ||
            matrix[0, 2] == player && matrix[1, 1] == player && matrix[2, 0] == player ||

            matrix[0, 0] == player && matrix[1, 0] == player && matrix[2, 0] == player ||
            matrix[0, 1] == player && matrix[1, 1] == player && matrix[2, 1] == player ||
            matrix[0, 2] == player && matrix[1, 2] == player && matrix[2, 2] == player)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
