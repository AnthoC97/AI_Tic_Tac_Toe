using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField]
    private List<Image> grid;
    [SerializeField]
    private Button play;
    [SerializeField]
    private Text status;
    [SerializeField]
    private Sprite circle;
    [SerializeField]
    private Sprite cross;
    [SerializeField]
    private Sprite neutral;
    [SerializeField]
    private Text scoreJ1;
    private int score_j1;
    [SerializeField]
    private Text scoreJ2;
    private int score_j2;
    private bool gameLaunched = false;
    private bool turn = true;
    private Ray ray;
    private int[,] state;
    // int : index d'une case, bool : est elle occupee ? , bool : par qui ?
    private Dictionary<int, (bool, bool)> gridmap;
    private bool firstPlay;
    private int nb_turn;
    private IA ia = new IA();
    private bool ia_turn;

    // Start is called before the first frame update
    void Start()
    {
        play.onClick.AddListener(Launch);
        state = new int[3, 3] { { 8, 8, 8 }, { 8, 8, 8 }, { 8, 8, 8 } };
        gridmap = new Dictionary<int, (bool, bool)>();
        score_j1 = 0;
        score_j2 = 0;
        firstPlay = true;
        nb_turn = 0;
    }

    public void Launch()
    {
        play.gameObject.SetActive(false);
        status.text = "Joueur 1, commencez";
        gameLaunched = true;
        play.onClick.RemoveListener(Launch);
        if(firstPlay == false)
            ResetGrid();
        firstPlay = false;
    }

    public void ComputePlay(Image img)
    {
        if(gameLaunched)
        {
            nb_turn++;
            int index = grid.IndexOf(img);
            //Human turn
            Debug.Log("OK");
            if (gridmap.ContainsKey(index))
                return;
            UpdateGrid(turn, img);
            gridmap.Add(index, (true, turn));
            gridMapToMat(gridmap);
            turn = !turn;
            //IA turn
            int player;
            if (turn)
                player = 1;
            else
                player = 0;
            int iaPlay = ia.BestMove(player, state, grid);
            Image img2 = grid[iaPlay];
            if (gridmap.ContainsKey(iaPlay))
                return;
            UpdateGrid(turn, img2);
            gridmap.Add(iaPlay, (true, turn));
            gridMapToMat(gridmap);
            turn = !turn;
        }
    }

    public void UpdateGrid(bool turn, Image image)
    {
        if (turn)
        {
            image.GetComponent<Image>().sprite = circle;
        }
        else
        {
            image.GetComponent<Image>().sprite = cross;
        }
    }

    public void gridMapToMat(Dictionary<int, (bool, bool)> gridmap)
    {
        int i = 0;
        int j = 0;
        foreach (KeyValuePair<int, (bool, bool)> cell in gridmap)
        {
            int x = cell.Key;
            i = (x - (x % 3)) / 3;
            j = x % 3;
            if (cell.Value.Item2 == true)
            {
                state[i, j] = 1;
            }
            else
            {
                state[i, j] = 0;
            }
        }
        CheckState(turn, i, j);
        //for (int k = 0; k < 3; k++)
        //{
        //    for (int l = 0; l < 3; l++)
        //    {
        //        Debug.Log("state "+k+", "+l+" : "+state[k, l]);
        //    }
        //}
    }

    //player : si true, joueur 1 (circle) sinon j2 (cross)
    public void CheckState(bool turn, int x, int y)
    {
        int player;
        int nb = 0;
        if (turn)
            player = 1;
        else
            player = 0;
        int n = state.GetLength(0);
        //check row
        Debug.Log("CHECK ROW");
        for (int i = 0; i < n; i++)
        {
            if (state[x, i] == player)
            {
                nb++;
                continue;
            }
            else
            {
                nb = 0;
                break;
            }
        }
        Debug.Log("N : " + n);
        if(nb != 3)
        {
            //check column
            Debug.Log("CHECK COLUMN");
            for (int i = 0; i < n; i++)
            {
                if (state[i, y] == player)
                {
                    nb++;
                    continue;
                }
                else
                {
                    nb = 0;
                    break;
                }
            }
        }
        //check diag
        if (x == y && nb != 3)
        {
            Debug.Log("CHECK DIAG");
            for (int i = 0; i < n; i++)
                if (state[i, i] == player)
                {
                    nb++;
                    continue;
                }
                else
                {
                    nb = 0;
                    break;
                }
        }
        //check anti-diag
        else if (x + y == n - 1 && nb != 3)
        {
            Debug.Log("CHECK ANTI-DIAG");
            int j = 0;
            Debug.Log("Yeah");
            for(int i = 2; i >= 0; i--)
            {
                if (state[i, j] == player)
                {
                    nb++;
                    j++;
                    continue;
                }
                else
                {
                    nb = 0;
                    j = 0;
                    break;
                }
            }
        }
        if (nb == 3)
            Win(player);
        if (nb_turn == 9)
            ResetGrid();
    }

    public void Win(int player)
    {
        if(player == 1)
        {
            score_j1++;
            scoreJ1.text = "Victoires J1: " + score_j1;
        }
        else
        {
            score_j2++;
            scoreJ2.text = "Victoires J2: " + score_j2;
        }
        gameLaunched = false;
        play.gameObject.SetActive(true);
        play.onClick.AddListener(Launch);
    }

    public void ResetGrid()
    {
        foreach (Image img in grid)
        {
            img.GetComponent<Image>().sprite = neutral;
        }
        state = new int[3, 3] { { 8, 8, 8 }, { 8, 8, 8 }, { 8, 8, 8 } };
        gridmap = new Dictionary<int, (bool, bool)>();
        nb_turn = 0;
    }
}

//Adresse 1 : rue