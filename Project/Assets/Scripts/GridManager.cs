using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    //1 = Occupied Spot. 0 = Unoccupied Spot.
    public static int[,] PlayGrid = new int[10, 22];
    private static List<int> FullRowIndices = new List<int>();
    public static GameObject[] RowObjects = new GameObject[22];
    private static int CubesReported = 0;

    public static GameObject GameOverObject;

    void Start()
    {
        Reset();
        GameOverObject = GameObject.FindGameObjectWithTag("GameOver");
        GameOverObject.SetActive(false);
    }
    void Reset()
    {
        PlayGrid = null;
        PlayGrid = new int[10, 22];
        FullRowIndices.Clear();
        RowObjects = null;
        RowObjects = new GameObject[22];
        CubesReported = 0;
    }
    //Every cube in the tetris block ghost will report back if they are blocked by either out of bounds or playgrid is full.
    public static bool RotationNotBlocked(float x, float y)
    {
        //Fix floating point precision errors.
        x = Mathf.Round(x);
        y = Mathf.Round(y);
        //Ghost is out of boundaries on the X-axis! Can't Rotate.
        if (x <= -1 || x >= 10) { return false; }
        //Ghost is out of boundaries on the Y-axis! Can't Rotate.
        if (y <= -1 || y >= 22) { return false; }
        //Ghost is intersecting with an occupied space! Can't Rotate.
        if (PlayGrid[(int)x, (int)y] == 1) { return false; }

        //Ghost is free and rotation is possible. Can Rotate.
        return true;
    }
    //Every cube in the tetris block will ask if they can move, orelse they will stop.
    public static bool CanMoveDown(float x, float y)
    {
        //Fix floating point precision errors.
        x = Mathf.Round(x);
        y = Mathf.Round(y);
        //If the block hits the floor, y will be equal to 0, thus the block stops.
        if (y == 0) { return false; }
        //If PlayGrid already contains a block there, stop the block.
        try
        {
            if (PlayGrid[(int)x, (int)y - 1] == 1) { return false; }
        }
        catch
        {
            Debug.Log(string.Format("X: {0} Y:{1}", x, y));
        }

        //If PlayGrid[x,y] == 0 and y != 0, you can move.
        return true;
    }
    //Check if the player can move the tetris block to the side, if not, stop him/her!
    public static bool CanMoveLeft(float x, float y)
    {
        //Fix floating point precision errors.
        x = Mathf.Round(x);
        y = Mathf.Round(y);
        //Can not move left!
        if (x == 0) { return false; }
        if (PlayGrid[(int)x - 1, (int)y] == 1) { return false; }
        //Can move left!
        return true;
    }
    public static bool CanMoveRight(float x, float y)
    {
        //Fix floating point precision errors.
        x = Mathf.Round(x);
        y = Mathf.Round(y);
        //Can't move right!
        if (x > 8) { return false; }
        //This grid square contains a block, so the player can't move there!
        if (PlayGrid[(int)x + 1, (int)y] == 1) { return false; }

        //If x > -1 && x < 22, and PlayGrid[x,y] == 0 the player can move!
        return true;
    }
    //Every cube in the tetris block will report their position if they stop.
    public static void ReportStoppedPosition(float x, float y, GameObject cube)
    {
        //Fix floating point precision errors.
        x = Mathf.Round(x);
        y = Mathf.Round(y);
        if (y == 20)
        {
            //Game Over, Block is out of bounds.
            GameOverObject.SetActive(true);
            return;
        }
        if (y < 0) { y = 0; }
        PlayGrid[(int)x, (int)y] = 1;
        if (RowObjects[(int)y] == null)
        {
            GameObject RowParent = new GameObject();
            RowParent.transform.position = new Vector2(RowParent.transform.position.x, (int)y);
            RowParent.name = "Row " + y.ToString();

            RowObjects[(int)y] = RowParent;
        }
        cube.name = string.Format("Cube - X:{0}, Row:{1}", (int)x, (int)y);
        cube.transform.SetParent(RowObjects[(int)y].transform, true);
        if (RowObjects[(int)y].transform.childCount == 10)
        {
            FullRowIndices.Add((int)y);
        }

        CubesReported++;

        if (CubesReported == 4)
        {
            CheckIfFullLine();
            CubesReported = 0;
        }
    }
    private static void CheckIfFullLine()
    {
        int lowestindex = 99;
        int countOfRows = 0;
        //If there are no full rows, then return.
        if (FullRowIndices.Count == 0) { return; }

        countOfRows = FullRowIndices.Count;
        ScoreManager.AddScore(countOfRows * 10);
        //However if there are any, go go!
        foreach (int index in FullRowIndices)
        {
            //Delete all the rows in fullrowindices.
            Destroy(RowObjects[index]);
            RowObjects[index] = null;
            //Clear the playgrid in that position.
            for (int i = 0; i < 10; i++)
            {
                PlayGrid[i, index] = 0;
            }
            //Set the lowest index.
            if (index < lowestindex) { lowestindex = index; }
        }
        for (int i = lowestindex+1; i < 22; i++)
        {
            if (RowObjects[i] != null)
            {
                bool PosFound = false;
                float PosY = 0;
                int modifier = 0;
                GameObject Row = RowObjects[i];
                while (!PosFound)
                {
                    if (Row.transform.position.y - countOfRows + modifier >= 0)
                    {
                        if (RowObjects[(int)(Row.transform.position.y - countOfRows + modifier)] == null)
                        {
                            PosFound = true;
                            PosY = Row.transform.position.y - countOfRows + modifier;
                        }
                        else
                        {
                            modifier++;
                        }
                    }
                    else
                    {
                        modifier++;
                    }
                }
                //Position is found, do something.
                Row.transform.position = new Vector2(Row.transform.position.x, PosY);
                RowObjects[(int)PosY] = Row;
                RowObjects[i] = null;
                Row.name = "Row " + ((int)PosY);

                for (int k = 0; k < 10; k++)
                {
                    PlayGrid[k, (int)PosY] = PlayGrid[k, i];
                    PlayGrid[k, i] = 0;
                }
            }
        }

        FullRowIndices.Clear();
    }
}

////Clear the fullrowIndices before starting.
//       FullRowIndices.Clear();
//       bool rowFull = true;
//       //Check every row to see if one Y is full of X.
//       for (int i = 0; i < 22; i++)
//       {
//           rowFull = true;
//           for (int k = 0; k < 10; k++)
//           {
//               if (PlayGrid[k, i] == 0)
//               {
//                   rowFull = false;
//               }
//           }
//           if (rowFull)
//           {
//               FullRowIndices.Add(i);
//           }
//       }
////If atleast one full row exists, delete all full rows.
//      int highestindex = 0;
//      int countOfrows = FullRowIndices.Count;
//      ScoreManager.AddScore(countOfrows * 10);
//      if (FullRowIndices.Count > 0)
//      {
//          foreach (int index in FullRowIndices)
//          {
//              //Empty that PlayGrid row.
//              for (int i = 0; i < 10; i++)
//              {
//                  PlayGrid[i, index] = 0;
//              }

//              if (index > highestindex)
//              {
//                  highestindex = index;
//              }
//              Destroy(RowObjects[index]);
//          }
//          for (int i = highestindex + 1; i < 22; i++)
//          {
//              if (RowObjects[i] != null)
//              {
//                  GameObject Row = RowObjects[i];
//                  Row.name = "Row " + (i-countOfrows);
//                  Row.transform.position = new Vector2(Row.transform.position.x, Row.transform.position.y - countOfrows);
//                  RowObjects[i - countOfrows] = Row;
//                  RowObjects[i] = null;

//                  //Displace the corresponding PlayGrid Row.
//                  for (int k = 0; k < 10; k++)
//                  {
//                      PlayGrid[k, i - countOfrows] = PlayGrid[k, i];
//                      PlayGrid[k, i] = 0;
//                  }
//              }
//          }
//      }
