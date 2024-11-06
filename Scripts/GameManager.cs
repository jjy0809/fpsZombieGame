using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager gm;

    public GameObject gameLabel;

    Text gameText;

    public GameState gState;

    PlayerMove player;

    int i = 0;

    PlayerFire PF;

    int enemy = 30;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }

    public enum GameState
    {
        Ready,
        Run,
        Gameover
    }

    


    void Start()
    {
        gState = GameState.Ready;
        gameText = gameLabel.GetComponent<Text>();

        gameText.text = "Ready...";
        gameText.color = new Color32(255, 185, 0, 255);

        StartCoroutine(ReadyToStart());

        player = GameObject.Find("Player").GetComponent<PlayerMove>();

        PF = GameObject.Find("Player").GetComponent<PlayerFire>();

        Collider[] cols = Physics.OverlapSphere(transform.position, 1000, 1 << 10);
        enemy = cols.Length;

        Cursor.visible = false; // 커서 숨기기
        Cursor.lockState = CursorLockMode.Locked; // 커서를 화면 중앙에 고정
    }

    IEnumerator ReadyToStart()
    {
        yield return new WaitForSeconds(2.0f);

        gameText.text = "Go!";

        gameText.color = new Color32(0, 255, 0, 255);

        yield return new WaitForSeconds(0.5f);

        gameLabel.SetActive(false);

        gState = GameState.Run;
    }



    void Update()
    {
        if(player.hp <= 0)
        {
            player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);

            gameLabel.SetActive(true);

            gameText.text = "Game Over";

            gameText.color = new Color32(255, 0, 0, 255);

            gState = GameState.Gameover;
        }

        Collider[] cols = Physics.OverlapSphere(transform.position, 1000, 1 << 10);

        PF.kill = enemy - cols.Length;

        if (Input.GetKeyDown(KeyCode.C))
        {
            print(cols.Length + "  Zombies left");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            print(PF.kill + "  Zombies Killed");
        }
    }
}
