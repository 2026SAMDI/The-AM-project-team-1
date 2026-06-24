using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class SpawnTarget : MonoBehaviour
{
    [SerializeField]private GameObject targetPrefab;
    [SerializeField]private Transform[] spawnPoints;
    [SerializeField]private ScoreManagement score;

    private Queue<int> indexQueue = new Queue<int>(); //스폰포인트 인덱스를 여기에 담아 먼저 넣은걸 꺼내준다.
    private int lastIndex = -1; // 직전에 있던 위치에 다시 소환하지 않게 하는 코드

    private void Start()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.Log("스폰포인트가 지정되지 않았습니다.");
            return;
        }
        SpawnNewTarget();
    }
    public void SpawnNewTarget()
    {

        if (indexQueue.Count == 0)
            RefillQueue(); // 큐가 비어있으면 다시 채움

        int index = indexQueue.Dequeue();

        if (spawnPoints[index] == null)
        {
            Debug.Log("스폰포인트[index]가 비어있습니다.");
            return;
        }
        lastIndex = index; //indexQueue의 앞값을 꺼내옴 예시) indexQueue = [2,0,3]이면 2 먼저 꺼내옴
        
        // Instantiate(targetPrefab, spawnPoints[index].position, Quaternion.identity); // 타겟을 생성함 [index]는 몇번째 위치에 있는지
        GameObject go = Instantiate(targetPrefab, spawnPoints[index].position, Quaternion.identity); 

        ApplyDifficultyScale(go);
    }

    private void RefillQueue()
    {
        List<int> indices = new List<int>(); // 임시리스트 생성 (섞기 편하라고)
        for (int i = 0; i < spawnPoints.Length; i++) 
            indices.Add(i); //인덱스를 추가함 (예를 들면 4개의 스폰포인트가 있으면 [0,1,2,3]이렇게 만들어짐)

        Shuffle(indices); // indices의 리스트를 랜덤으로 섞음

        if (indices.Count > 1 && indices[0] == lastIndex) // 연속된 숫자가 나오면 그 리스트 첫번째 값을 지우고 마지막 값으로 추가한다.
        {
            int first = indices[0];
            indices.RemoveAt(0);
            indices.Add(first);
        }
        foreach (int i in indices)
            indexQueue.Enqueue(i); // 리스트 indexQueue에 넣음
    }
    private void Shuffle(List<int> list)
    {
        for (int i = list.Count -1 ; i>0; i--) // 뒤에서부터 하나씩 순회
        {
            int j = Random.Range(0,i+1); // 0 ~ j+1만큼의 범위 선택
            (list[i], list[j]) = (list[j],list[i]); // 위치 교환
        }
    }
    private void ApplyDifficultyScale(GameObject targetObject)
{
    string currentDifficulty = PlayerPrefs.GetString("SelectedDifficulty", "Easy");
    
    // 기본 크기 지정 (Easy일 때의 배율)
    float sizeMultiplier = 1.0f;

    if (currentDifficulty == "Normal")
    {
        sizeMultiplier = 0.69f; // Easy의 80% 크기
        score.targetScore = 150;
    }
    else if (currentDifficulty == "Hard")
    {
        sizeMultiplier = 0.5f; // Easy의 40% 크기
        score.targetScore = 200;
    }

    // 타겟 오브젝트의 LocalScale에 배율 곱해주기
    Vector3 defaultScale = new Vector3(3f, 3f, 3f); 
    targetObject.transform.localScale = defaultScale * sizeMultiplier;
}
}
