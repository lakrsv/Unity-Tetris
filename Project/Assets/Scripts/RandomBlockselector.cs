using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class RandomBlockselector : MonoBehaviour {

    //I need a list of the 7 blocks.
    //I need a function to pick one of the 7 blocks at random.
    //I want the block to be rotated randomly before the player gets it.

    //This contains all the Tetrisblocks.
    public static GameObject[] TetrisBlocks = new GameObject[7];
    public static Sprite[] TetrisBlockPreviews = new Sprite[7];
    public static Sprite NextBlockSprite;
    private static int NextBlockNumber = 0;
    private static Image BlockPreviewImage;
    //This simply creates the random that we use.
    public static System.Random Random = new System.Random();

    void Awake()
    {
        Reset();
        BlockPreviewImage = GameObject.FindGameObjectWithTag("BlockPreview").GetComponent<Image>();
        int Number = Random.Next(0, 7);
        NextBlockNumber = Number;
        NextBlockSprite = TetrisBlockPreviews[Number];
        BlockPreviewImage.sprite = NextBlockSprite;
        TetrisBlocks[0] = Resources.Load<GameObject>("Prefabs/Blocks/I-Block");
        TetrisBlocks[1] = Resources.Load<GameObject>("Prefabs/Blocks/O-Block");
        TetrisBlocks[2] = Resources.Load<GameObject>("Prefabs/Blocks/L-Block");
        TetrisBlocks[3] = Resources.Load<GameObject>("Prefabs/Blocks/J-Block");
        TetrisBlocks[4] = Resources.Load<GameObject>("Prefabs/Blocks/T-Block");
        TetrisBlocks[5] = Resources.Load<GameObject>("Prefabs/Blocks/S-Block");
        TetrisBlocks[6] = Resources.Load<GameObject>("Prefabs/Blocks/Z-Block");

        TetrisBlockPreviews[0] = Resources.Load<Sprite>("Preview/I-block");
        TetrisBlockPreviews[1] = Resources.Load<Sprite>("Preview/O-block");
        TetrisBlockPreviews[2] = Resources.Load<Sprite>("Preview/L-block");
        TetrisBlockPreviews[3] = Resources.Load<Sprite>("Preview/J-block");
        TetrisBlockPreviews[4] = Resources.Load<Sprite>("Preview/T-block");
        TetrisBlockPreviews[5] = Resources.Load<Sprite>("Preview/S-block");
        TetrisBlockPreviews[6] = Resources.Load<Sprite>("Preview/Z-block");
    }
    void Reset()
    {
        TetrisBlockPreviews = null;
        TetrisBlockPreviews = new Sprite[7];
        TetrisBlocks = null;
        TetrisBlocks = new GameObject[7];
        NextBlockSprite = null;
        NextBlockNumber = 0;
        Random = null;
        Random = new System.Random();
    }

    private static void NextRandomNumber()
    {
        //This chooses a random number from 0 to 6.
        int Number = Random.Next(0, 7);
        NextBlockNumber = Number;
        NextBlockSprite = TetrisBlockPreviews[Number];
        BlockPreviewImage.sprite = NextBlockSprite;
       
    }
    //This is a method, it has a return type of gameobject.
    public static GameObject GetNextBlock()
    {
        int Number = NextBlockNumber;
        NextRandomNumber();
        return TetrisBlocks[Number];
    }
}
