using UnityEngine;
using System.Collections;

public class BlockController : MonoBehaviour
{

    float BlockSpeed;
    bool Done = false;
    bool DoneReport = false;
    bool Collided = false;
    bool PlacementCollided = false;
    bool CanMoveLeft = true;
    bool CanMoveRight = true;
    bool CanRotate = true;
    bool CanFastForward = true;
    public float zRotation = -90;
    GameObject[] Children;
    //This is the rotation ghost of the block.
    GameObject Ghost;
    public GameObject PlacementGhost;
    GameObject[] PlacementGhostChildren;
    GameObject[] GhostChildren;
    // Use this for initialization
    void Start()
    {
        GetChildren();
        InitializePlacementGhost();
        BlockSpeed = GameManager.Speed;
        StartCoroutine(MoveBlock());
    }

    // Update is called once per frame
    void Update()
    {
        //Only allow movement while the block has not collided.
        if (!Collided)
        {
            BlockMovement();
        }
        if (!Collided)
        {
            UpdatePlacementGhost();
        }
    }
    void InitializePlacementGhost()
    {
        if (PlacementGhost == null) { return; }
        PlacementGhost = (GameObject)Instantiate(PlacementGhost, transform.position, Quaternion.identity);
        PlacementGhostChildren = new GameObject[PlacementGhost.transform.childCount];

        for (int i = 0; i < PlacementGhost.transform.childCount; i++)
        {
            PlacementGhostChildren[i] = PlacementGhost.transform.GetChild(i).gameObject;
        }
    }
    void UpdatePlacementGhost()
    {
        if (PlacementGhost == null) { return; }
        while (!PlacementCollided)
        {
            if (PlacementGhost == null) { break; }
            foreach (GameObject child in PlacementGhostChildren)
            {
                if (!GridManager.CanMoveDown(child.transform.position.x, child.transform.position.y))
                {
                    PlacementCollided = true;
                }
            }
            if (!PlacementCollided)
            {
                if (PlacementGhost == null) { return; }
                float yPos = PlacementGhost.transform.position.y - 1;
                PlacementGhost.transform.position = new Vector2(PlacementGhost.transform.position.x, yPos);
            }
        }
    }
    void ResetPlacementGhost()
    {
        if (PlacementGhost == null) { return; }
        PlacementGhost.transform.position = transform.position;
        PlacementCollided = false;
    }
    void BlockMovement()
    {
        //Fast Forward the block down.
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            while (CanFastForward)
            {

                foreach (GameObject child in Children)
                {
                    if (!GridManager.CanMoveDown(child.transform.position.x, child.transform.position.y -1))
                    {
                        CanFastForward = false;
                    }
                }
                if (CanFastForward)
                {
                    float yPos = transform.position.y - 1;
                    transform.position = new Vector2(transform.position.x, yPos);
                }
            }
        }
        //Try to move to the left if all conditions are fulfilled.
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CanMoveLeft = true;
            //Move block to the left if within playgrid and squares are not occupied.
            foreach (GameObject child in Children)
            {
                //If one of the blocks can not move, do not let the player move.
                if (!GridManager.CanMoveLeft(child.transform.position.x, child.transform.position.y))
                {
                    CanMoveLeft = false;
                    Debug.Log(child.name);
                }
            }
            if (CanMoveLeft)
            {
                //The space is not occupied, move the tetris block to the left.
                transform.position = new Vector2(transform.position.x - 1, transform.position.y);
                ResetPlacementGhost();
            }
        }
        //Try to move to the right if all conditions are fulfilled.
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            CanMoveRight = true;
            //Move block to the left if within playgrid and squares are not occupied.
            foreach (GameObject child in Children)
            {
                //If one of the blocks can not move, do not let the player move.
                if (!GridManager.CanMoveRight(child.transform.position.x, child.transform.position.y))
                {
                    CanMoveRight = false;
                }
            }
            if (CanMoveRight)
            {
                //The space is not occupied, move the tetris block to the right.
                transform.position = new Vector2(transform.position.x + 1, transform.position.y);
                ResetPlacementGhost();
            }
        }
        //Try to rotate if all the conditions are fulfilled.
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            CanRotate = true;
            //Check all the children of the ghost block to see if they are blocked or outside of boundaries.
            foreach (GameObject child in GhostChildren)
            {
                //Vector3 newChildPos = transform.TransformPoint(child.transform.position);
                if (!GridManager.RotationNotBlocked(child.transform.position.x, child.transform.position.y))
                {
                    CanRotate = false;
                }
            }
            //No ghost children are blocked, and thus we can rotate!
            if (CanRotate)
            {
                //0, -90, -180, 0
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
                if (PlacementGhost != null)
                {
                    PlacementGhost.transform.eulerAngles = transform.eulerAngles;
                    ResetPlacementGhost();
                }

                if (zRotation != -270)
                {
                    zRotation -= 90;
                }
                else
                {
                    zRotation = 0;
                }
            }
        }
    }

    IEnumerator MoveBlock()
    {
        while (!Collided)
        {
            DoneReport = false;
            Done = false;
            yield return new WaitForSeconds(BlockSpeed);
            if (!Done)
            {
                Done = true;
                foreach (GameObject child in Children)
                {
                    if (!GridManager.CanMoveDown(child.transform.position.x, child.transform.position.y))
                    {
                        Collided = true;
                        ChildrenReportPosition();
                    }
                }
            }
            if (!Collided)
            {
                float yPos = transform.position.y - 1;
                transform.position = new Vector2(transform.position.x, yPos);
            }
            else
            {
                SpawnBlock.ReadyToSpawn = true;
            }
        }
    }
    void GetChildren()
    {
        Children = new GameObject[gameObject.transform.childCount - 1];
        for (int i = 0; i < Children.Length; i++)
        {
            Children[i] = gameObject.transform.GetChild(i).gameObject;
        }
        //Get the rotation ghost child.
        Ghost = gameObject.transform.GetChild(gameObject.transform.childCount - 1).gameObject;

        GhostChildren = new GameObject[Ghost.transform.childCount];
        for (int i = 0; i < GhostChildren.Length; i++)
        {
            GhostChildren[i] = Ghost.transform.GetChild(i).gameObject;
        }
    }
    void ChildrenReportPosition()
    {
        if (!DoneReport)
        {
            foreach (GameObject child in Children)
            {
                GridManager.ReportStoppedPosition(child.transform.position.x, child.transform.position.y, child);
                if (child.transform.position.y == 20)
                {
                    GameManager manager = GameObject.FindGameObjectWithTag("TetrisManager").GetComponent<GameManager>();
                    manager.GoToMainMenu();
                }
            }
            gameObject.transform.DetachChildren();
            DoneReport = true;

            Destroy(Ghost);
            Destroy(gameObject);
            Destroy(PlacementGhost);
        }
    }
}
