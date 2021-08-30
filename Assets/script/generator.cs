using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        34,34,34,34,35,35,35,35,36,36,36,36,37,37,37,37,39,39,39,39,40,40,40,40
    }; // 三十七张普通牌，一张万能牌，一张随机牌，一张炸弹牌

    float generateCDcounter;

    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(makeStart());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator makeStart()
    {
        List<int> ind = new List<int>();
        for (int i = 0; i < typeList.Length; i++)
            ind.Add(i);
        while (ind.Count > 0)
        {
            int rand = Random.Range(0, ind.Count);
            GameObject obj = Instantiate(majiangPrefab);
            obj.transform.position = new Vector3(
                transform.position.x + Random.Range(-generateWidthRandomRange, generateWidthRandomRange),
                transform.position.y + Random.Range(-generateHeightRandomRange, generateHeightRandomRange),
                transform.position.z + Random.Range(-generateLengthRandomRange, generateLengthRandomRange)
            );
            obj.GetComponent<majiang>().setImg(typeList[ind[rand]]);
            yield return new WaitForSecondsRealtime(generateCD);
            ind.RemoveAt(rand);
        }
    }
}
