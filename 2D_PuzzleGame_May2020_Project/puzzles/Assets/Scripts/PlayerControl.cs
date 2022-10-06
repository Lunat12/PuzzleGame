using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour
{
    #region MoveVariables
    public float jumpForce, speed;
    public LayerMask groundMasK, wallMask;
    private Rigidbody2D rigid;
    private bool isGrounded, isRightDetected, isLeftDetected;
    private int totalJumps, currentJumps;
    public GameObject floorPoint1, floorPoint2, left1, left2, right1, right2;


    private Vector3 initScale;
    bool isReduced;
    private BoxCollider2D playerCollider;
    #endregion
    #region CloneVariables
    public bool cloned;
    public GameObject clone;
    private GameObject clonePal;
    public float counter;
    public float TimeLeft;
    public GameObject TimePanel;
    #endregion
    #region GrabVariable
    private RaycastHit2D pickUp;
    private Vector2 lastDir = new Vector2(1, 0);
    public LayerMask objectsLayer;
    public GameObject Slot;
    private GameObject CurrentObject;
    private bool hasItem;
    #endregion
    #region GunVariables
    public bool hasGun, hasUpgrade;
    public LayerMask StickyWall;
    private bool StickyWallDetected;
    private Vector2 finalpos;
    private GameObject gizmoRope;
    private bool throwRope, playerMove;

    #endregion
    public int hasKey;
    public GameObject currentDoor;
    private SystemsManager manager;
    bool send;
    public Animator anim;
    private bool EndGame;
    // Start is called before the first frame update
    void Start()
    {
        counter = 60f;
        TimePanel.SetActive(false);
        cloned = false;
        totalJumps = 1;
        rigid = GetComponent<Rigidbody2D>();

        //initPos = playerCollider;
        isReduced = false;

        playerCollider = gameObject.GetComponent<BoxCollider2D>();
        initScale = playerCollider.size;

        manager = GameObject.Find("GameManager").GetComponent<SystemsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {

            lastDir = Vector2.right;
            transform.localRotation = Quaternion.Euler(0, 0, 0);


        }
        if (Input.GetAxis("Horizontal") < 0)
        {

            lastDir = Vector2.left;
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }



        isGrounded = Physics2D.OverlapArea(floorPoint1.transform.position, floorPoint2.transform.position, groundMasK);
        isRightDetected = Physics2D.OverlapArea(right1.transform.position, right2.transform.position, wallMask);

        isLeftDetected = Physics2D.OverlapArea(left1.transform.position, left2.transform.position, wallMask);


        if (isGrounded == true)
        {

            anim.SetBool("jump", false);


            if (Input.GetAxis("Horizontal") == 0)
            {
                anim.SetBool("walk", false);
            }
            else
            {
                anim.SetBool("walk", true);
            }

            currentJumps = 0;
            rigid.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rigid.velocity.y);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rigid.velocity *= 0;
                rigid.AddForce(Vector2.up * jumpForce);
            }



        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space) && currentJumps < totalJumps)
            {
                rigid.velocity *= 0;
                currentJumps++;
                rigid.AddForce(Vector2.up * jumpForce);
            }
            if (isRightDetected == false && isLeftDetected == false)
            {
                rigid.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rigid.velocity.y);
            }
            if (isRightDetected == true && Input.GetAxis("Horizontal") < 0)
            {
                rigid.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rigid.velocity.y);
            }
            if (isLeftDetected == true && Input.GetAxis("Horizontal") > 0)
            {
                rigid.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rigid.velocity.y);
            }

            anim.SetBool("jump", true);
            anim.SetBool("walk", false);

        }


        if (Input.GetKey(KeyCode.S) && isGrounded == true && isReduced == false)
        {

            Vector2 size = playerCollider.size;
            size = new Vector2(size.x, size.y / 2);
            playerCollider.size = size;
            anim.SetBool("crawl", true);
            anim.SetBool("walk", false);
           
           

            isReduced = true;

            Slot.SetActive(false);
            anim.SetBool("pick up anim", false);
          
        }
        else if (Input.GetKeyUp(KeyCode.S) && isReduced == true)
        {
            playerCollider.size = initScale;
            anim.SetBool("crawl", false);
            isReduced = false;

            Slot.SetActive(true);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector2 newPos = transform.position;
            newPos.x += 2;
            if (cloned == false)
            {
                GameObject newClone = Instantiate(clone, newPos, transform.rotation);

                speed = 0f;
                jumpForce = 0f;
                cloned = true;
                clonePal = newClone;
                GameObject.Find("CameraPoint").GetComponent<CameraControl>().target2 = clonePal;
                if (hasItem == true)
                {
                    CurrentObject.transform.parent = null;
                    CurrentObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    CurrentObject.GetComponent<BoxCollider2D>().isTrigger = false;
                    print("cccc");
                    hasItem = false;
                }
            }
            else
            {
                if (clonePal.GetComponent<CloneControl>().hasItem == true)
                {
                    clonePal.GetComponent<CloneControl>().CurrentObject.transform.SetParent(null);
                    clonePal.GetComponent<CloneControl>().CurrentObject.GetComponent<BoxCollider2D>().isTrigger = false;
                    clonePal.GetComponent<CloneControl>().CurrentObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    clonePal.GetComponent<CloneControl>().hasItem = false;
                    clonePal.GetComponent<CloneControl>().Slot.SetActive(true);
                }
                if (clonePal.GetComponent<CloneControl>().hasGun == true)
                {
                    hasGun = true;

                }
                if (clonePal.GetComponent<CloneControl>().hasUpgrade == true)
                {
                    hasUpgrade = true;

                }
                speed = 5f;
                jumpForce = 300f;
                Destroy(clonePal);
                counter = 60f;
                TimePanel.SetActive(false);
                cloned = false;
                Slot.SetActive(true);

            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (lastDir != Vector2.zero)
            {
                Debug.DrawRay(transform.position, lastDir, Color.red);
                if (hasItem == false)
                {


                    pickUp = Physics2D.Raycast(transform.position, lastDir, 1, objectsLayer);
                    if (pickUp.collider.tag == "Object")
                    {

                        pickUp.collider.GetComponent<Rigidbody2D>().isKinematic = true;
                        pickUp.collider.GetComponent<BoxCollider2D>().isTrigger = true;

                        pickUp.collider.transform.SetParent(Slot.transform);
                        pickUp.collider.transform.localPosition = Vector2.zero;
                        CurrentObject = pickUp.collider.gameObject;
                        pickUp.collider.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                     
                        hasItem = true;
                    }

                }
                else
                {
                    CurrentObject.transform.parent = null;
                    CurrentObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    CurrentObject.GetComponent<BoxCollider2D>().isTrigger = false;
                    
                    hasItem = false;

                }




            }
        }

        if (hasItem == true)
        {
            if(isReduced == true)
            {
                anim.SetBool("pick up anim", false);
            }
            else
            {
                anim.SetBool("pick up anim", true);
            }
        
        }
        else
        {
            anim.SetBool("pick up anim", false);
        }

        if (Input.GetMouseButton(1) && hasGun == true && cloned == false && hasUpgrade == true)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.x < gameObject.transform.position.x)
            {
                rigid.AddForce(Vector2.right * 50f, ForceMode2D.Impulse);
                print("111");

                anim.SetBool("poweredBackward", true);
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            if (mousePos.x > gameObject.transform.position.x)
            {

                rigid.AddForce(Vector2.left * 50f, ForceMode2D.Impulse);

                anim.SetBool("poweredForward", true);
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }

        }

        else if (Input.GetMouseButtonDown(0) && hasGun == true)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            StickyWallDetected = Physics2D.OverlapCircle(mousePos, 0.1f, StickyWall);
            if (StickyWallDetected == true && cloned == false)
            {
                finalpos = mousePos;
                Destroy(gizmoRope);
                gizmoRope = new GameObject("gizmoRope");
                gizmoRope.transform.position = transform.position;
                throwRope = true;
                anim.SetBool("GunShoot", true);
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {

            anim.SetBool("GunShoot", false);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            anim.SetBool("poweredForward", false);
            anim.SetBool("poweredBackward", false);
        }

        if (throwRope == true)
        {
            gizmoRope.transform.position = Vector2.MoveTowards(gizmoRope.transform.position, finalpos, 50 * Time.deltaTime);
            GetComponent<LineRenderer>().SetPosition(0, Slot.transform.position);
            GetComponent<LineRenderer>().SetPosition(1, gizmoRope.transform.position);
            if ((Vector2)gizmoRope.transform.position == finalpos)
            {
                throwRope = false;
                playerMove = true;
                GetComponent<Rigidbody2D>().isKinematic = true;
            }

        }
        if (playerMove == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, finalpos, 15 * Time.deltaTime);
            float dist = Vector2.Distance(transform.position, finalpos);
            GetComponent<LineRenderer>().SetPosition(0, Slot.transform.position);
            if (dist < 0.7f)
            {
                playerMove = false;
                GetComponent<Rigidbody2D>().isKinematic = false;
                GetComponent<LineRenderer>().SetPosition(0, Vector2.zero);
                GetComponent<LineRenderer>().SetPosition(1, Vector2.zero);
            }

        }




        if (cloned == true)
        {
            counter -= Time.deltaTime;
            TimeLeft = Mathf.Floor(counter);
            TimePanel.SetActive(true);
            TimePanel.transform.Find("Text").GetComponent<Text>().text = "00:" + TimeLeft.ToString();
            if (counter <= 0f)
            {
                if (clonePal.GetComponent<CloneControl>().hasItem == true)
                {
                    clonePal.GetComponent<CloneControl>().CurrentObject.transform.SetParent(null);
                    clonePal.GetComponent<CloneControl>().CurrentObject.GetComponent<BoxCollider2D>().isTrigger = false;
                    clonePal.GetComponent<CloneControl>().CurrentObject.GetComponent<Rigidbody2D>().isKinematic = false;
                    clonePal.GetComponent<CloneControl>().hasItem = false;
                    clonePal.GetComponent<CloneControl>().Slot.SetActive(true);
                }
                speed = 5f;
                jumpForce = 300f;
                Destroy(clonePal);
                cloned = false;
                TimePanel.SetActive(false);
                counter = 60f;
                Slot.SetActive(true);
            }

        }

        if (send == true)
        {
            pass();
        }

        if (cloned == true)
        {
            anim.SetBool("pick up anim", false);
            anim.SetBool("poweredForward", false);
            anim.SetBool("poweredBackward", false);
            anim.SetBool("GunShoot", false);
            anim.SetBool("crawl", false);
            anim.SetBool("jump", false);
            anim.SetBool("walk", false);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            playerCollider.size = initScale;
            isReduced = false;
        }

        if(EndGame == true)
        {
            SceneManager.LoadScene(2);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            transform.SetParent(collision.transform);
        }
        if (collision.gameObject.tag == "Gun")
        {
            Destroy(collision.gameObject);
            hasGun = true;
        }
        if (collision.gameObject.tag == "Chip")
        {
            Destroy(collision.gameObject);
            hasUpgrade = true;
        }
        if (collision.gameObject.tag == "key")
        {
            Destroy(collision.gameObject);
            hasKey += 1;
        }
        if (collision.gameObject.tag == "Barrier")
        {
            currentDoor = collision.gameObject;
           
            send = true;
        }
        if(collision.gameObject.tag == "End")
        {
            EndGame = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            transform.SetParent(null);
        }
    }

  
    private void pass()
    {
        manager.DoorStaysOpen(currentDoor);
       
        send = false;
    }


}

