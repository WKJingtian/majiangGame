using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class generator : MonoBehaviour
{
    public GameObject majiangPrefab;
    public float generateCD;
    public float generateWidthRandomRange;
    public float generateHeightRandomRange;
    public float generateLengthRandomRange;
    public int[] typeList =
    {
        0,0,0,0,1,1,1,1,2,2,2,2,3,3,3,3,4,4,4,4,5,5,5,5,6,6,6,6,7,7,7,7,8,8,8,8,9,9,9,9,
        10,10,10,10,11,11,11,11,12,12,12,12,13,13,13,13,14,14,14,14,15,15,15,15,
        16,16,16,16,17,17,17,17,18,18,18,18,19,19,19,19,20,20,20,20,21,21,21,21,
        22,22,22,22,23,23,23,23,24,24,24,24,25,25,25,25,26,26,26,26,27,27,27,27,
        28,28,28,28,29,29,29,29,30,30,30,30,31,31,31,31,32,32,32,32,33,33,33,33,
        34,34,34,34,35,35,35,35,36,36,36,36,36,36,36,36
    }; // 三十三张普通牌，一张万能牌，一张随机牌，一张炸弹牌，一张占位符
    public Image[] playerScore = new Image[13];
    public Image[] playerScoreFinal = new Image[13];
    public int scoreCount = 0;
    public GameObject gameUICanvas;
    public GameObject gameoverCanvas;
    public GameObject settingCanvas;
    public float volunm = 100;
    public bool isExploding = false;

    float generateCDcounter;
    bool stopped = false;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(makeStart());
        Time.timeScale = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            setting();
        if (isExploding)
        {
            StartCoroutine(explosionSlow());
            isExploding = false;
        }
    }

    IEnumerator makeStart()
    {
        List<int> ind = new List<int>();
        for (int i = 0; i < typeList.Length; i++)
            ind.Add(i);
        while (ind.Count > 0)
        {
            if (stopped)
            {
                yield return new WaitForSecondsRealtime(generateCD);
                continue;
            }
            int rand = Random.Range(0, ind.Count);
            GameObject obj = Instantiate(majiangPrefab);
            obj.transform.position = new Vector3(
                transform.position.x + Random.Range(-generateWidthRandomRange, generateWidthRandomRange),
                transform.position.y + Random.Range(-generateHeightRandomRange, generateHeightRandomRange),
                transform.position.z + Random.Range(-generateLengthRandomRange, generateLengthRandomRange)
            );
            majiang mj = obj.GetComponent<majiang>();
            mj.setImg(typeList[ind[rand]]);
            mj.gameManager = this;
            yield return new WaitForSecondsRealtime(generateCD);
            ind.RemoveAt(rand);
        }
    }

    public void addScore(int scoreID) // 不处理随机牌和炸弹
    {
        if (scoreID > 36 || scoreID < 0)
        {
            Debug.LogWarning("invalid majiang ID: " + scoreID.ToString());
            return;
        }

        if (scoreCount >= 13) return;
        playerScore[scoreCount].sprite = Resources.Load<Sprite>("majiang/" + scoreID.ToString());
        playerScoreFinal[scoreCount].sprite = Resources.Load<Sprite>("majiang/" + scoreID.ToString());
        playerScore[scoreCount].color = Color.white;
        playerScoreFinal[scoreCount].color = Color.white;
        scoreCount += 1;
        Debug.Log("player scored! majiangID: " + scoreID.ToString() + ", total score: " + scoreCount.ToString());
        if (scoreCount >= 13) gameover();
    }

    void gameover()
    {
        Debug.Log("game over!");
        gameUICanvas.SetActive(false);
        settingCanvas.SetActive(false);
        gameoverCanvas.SetActive(true);
        Cursor.visible = true;
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void mainmenuGo()
    {
        SceneManager.LoadScene("Start");
    }
    public void setting()
    {
        if (scoreCount >= 13)
            gameoverCanvas.SetActive(false);
        else
            gameUICanvas.SetActive(false);
        settingCanvas.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0;
        stopped = true;
    }

    public void endSetting()
    {
        if (scoreCount >= 13)
            gameoverCanvas.SetActive(true);
        else
        {
            gameUICanvas.SetActive(true);
            Cursor.visible = false;
        }
        settingCanvas.SetActive(false);
        Time.timeScale = 2;
        stopped = false;
    }

    IEnumerator explosionSlow()
    {
        Time.timeScale = 0.3f;
        yield return new WaitForSecondsRealtime(1.2f);
        Time.timeScale = 2.0f;
    }
}
