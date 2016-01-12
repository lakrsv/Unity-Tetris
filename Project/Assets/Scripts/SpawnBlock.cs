using UnityEngine;
using System.Collections;

public class SpawnBlock : MonoBehaviour {

    //The Y modifier for O-block and I-block.
    float SpawnModifier = 0.5f;
    //Is the block ready to spawn?
    public static bool ReadyToSpawn = true;

	// Use this for initialization
	void Start () 
    {
        ReadyToSpawn = true;
	}
	
	// Update is called once per frame
	void LateUpdate () 
    {
        //Loop and do this method all the time.
        SpawnABlock();
	}

    void SpawnABlock()
    {
        //If we are ready to spawn, start the method.
        if (ReadyToSpawn)
        {
            //Set Readytospawn equal to false, so no more blocks spawn until ready
            ReadyToSpawn = false;

            //Set an empty vector2 for the spawn position, and set xPosition and yPosition
            Vector2 spawnPosition;
            float xPos = transform.position.x;
            float yPos = transform.position.y;

            //Choose a random tetris block to spawn.
            GameObject ChosenBlock = RandomBlockselector.GetNextBlock();

            //If the name of chosenblock is equal to i-block or o-block, add 0.5 to yPos.
            if (ChosenBlock.name == "I-block" || ChosenBlock.name == "O-block")
            {
                yPos += SpawnModifier;
            }
            else
            {
                xPos += SpawnModifier;
            }
            //Make the vector2 with xpos and ypos.
            spawnPosition = new Vector2(xPos, yPos);
            //Make the gameobject.
            Instantiate(ChosenBlock, spawnPosition, Quaternion.identity);

        }
    }
}
