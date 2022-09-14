using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Misc
    GameObject cam;

    //Movement
    public float speed;
    public float shiftSpeed;
    public float maxScroll;
    float scroll;

    //Debugging

    //SelectingGUIMaterial          Debugging
    public GameObject debuggingUIObj;
    public float roundPositionByMultible;
    GameObject instDebuggingUIObj;
    bool uIObjSelected;


    // Start is called before the first frame update
    void Start()
    {
        //Misc
        cam = GameObject.Find("Main Camera");

        //Movement
        scroll = 0;

        //Debugging
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        Clicking();


        Debugging();
    }

    private void Movement()
    {
        bool shiftPressed = Input.GetKey(KeyCode.LeftShift);
        scroll += -Input.mouseScrollDelta.y;
        scroll = Mathf.Clamp(scroll, 1, maxScroll);
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 direction = input.normalized;
        Vector2 velocity = shiftPressed ? shiftSpeed * direction : speed * direction;

        transform.Translate(velocity * Time.deltaTime);
        cam.GetComponent<Camera>().orthographicSize = scroll;
    }
    private void Clicking()
    {
        if (Input.GetButtonDown("Fire1") && uIObjSelected)                  //This is using a temperary variable!!!!
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);

            if (hit2D && hit2D.transform.name.Contains("_Clickable")) print("Make inspect");
        }
    }

    private void Debugging()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit)) Debug.DrawLine(transform.position, hit.point, Color.red);
        else Debug.DrawLine(transform.position, transform.forward * 100, Color.green);

        if (uIObjSelected)
        {
            instDebuggingUIObj.transform.localPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            instDebuggingUIObj.transform.localPosition = new Vector3(Mathf.Round(instDebuggingUIObj.transform.localPosition.x / debuggingUIObj.transform.localScale.x) * debuggingUIObj.transform.localScale.x, Mathf.Round(instDebuggingUIObj.transform.localPosition.y / debuggingUIObj.transform.localScale.y) * debuggingUIObj.transform.localScale.y, 0);
            instDebuggingUIObj.GetComponent<Collider2D>().enabled = true;
            if (Input.GetButtonDown("Fire1")) uIObjSelected = false;
        }
    }

    public void OnButtonClicked()
    {
        instDebuggingUIObj = Instantiate(debuggingUIObj, GameObject.Find("BuildingParent").transform);
        instDebuggingUIObj.transform.localPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        uIObjSelected = true;

        print("Player clicked UI");
    }
}
