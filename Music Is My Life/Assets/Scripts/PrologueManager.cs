using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PrologueManager : MonoBehaviour
{
    public GameObject imagePrefab; // 이미지 프리팹
    public Transform imageContainer; // 이미지를 추가할 부모 컨테이너
    public Sprite[] sprites; // 스프라이트 배열
    public TMP_Text[] prologueTexts; // 텍스트 컴포넌트 배열
    public Button mainMenuButton; // 메인 화면으로 가는 버튼
    public GameObject panelPrefab;
    public GameObject poster;

    private List<IPrologueStep> steps = new List<IPrologueStep>(); // 모든 단계를 포함하는 리스트
    private int currentStepIndex = 0; // 현재 단계 인덱스
    private Coroutine prologueCoroutine; // 프롤로그 코루틴

    public AudioClip sadSound;
    public AudioClip waterSound;
    private AudioSource audioSource;
    private Coroutine audioSourceCoroutine;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        foreach (var text in prologueTexts)
        {
            if (text != null)
            {
                text.gameObject.SetActive(false);
            }
        }
        poster.gameObject.SetActive(false);
        mainMenuButton.gameObject.SetActive(false); // 시작 시 버튼 비활성화
        panelPrefab.gameObject.SetActive(false);
        InitializeSteps();
        prologueCoroutine = StartCoroutine(ExecuteStepsOverTime()); // 코루틴 시작
    }

    // 스킵 버튼 클릭 시 실행되는 스크립트
    public void SkipPrologue(GameObject skipButton)
  {
      if (prologueCoroutine != null)
      {
          StopCoroutine(prologueCoroutine);
          prologueCoroutine = null;
      }

      if (audioSourceCoroutine != null)
      {
          StopCoroutine(audioSourceCoroutine);
          audioSourceCoroutine = null;
      }

      // 포스터 이미지 활성화
      poster.gameObject.SetActive(true);

      // 패널을 지연시켜 활성화하기 위한 코루틴 시작
      StartCoroutine(ShowPanelAfterDelay(1.5f)); // 예를 들어, 0.5초 후에 패널을 표시

      // 메인 메뉴 버튼 활성화 및 스킵 버튼 비활성화

      skipButton.SetActive(false);
  }

  IEnumerator ShowPanelAfterDelay(float delay)
  {
      // 지정된 지연 시간 동안 대기
      yield return new WaitForSeconds(delay);

      // 패널 활성화
      panelPrefab.gameObject.SetActive(true);
      // 딜레이 후 닫기 버튼 활성화
      yield return new WaitForSeconds(delay);
      mainMenuButton.gameObject.SetActive(true);
  }


    // 각 단계를 시간 간격을 두고 실행하는 코루틴
    IEnumerator ExecuteStepsOverTime()
    {
        for (int i = 0; i < steps.Count; i++)
        {
            steps[i].Execute();

            // 'ClearTextStep' 다음에 'ShowSpriteStep'가 바로 오는 경우 대기하지 않음
            if (i < steps.Count - 1 && steps[i] is ClearTextStep && steps[i + 1] is ShowSpriteStep)
            {
                continue; // 대기 시간 없이 다음 단계로 진행
            }

            yield return new WaitForSeconds(1.3f); // 다른 경우에는 1초 대기
        }

        // 모든 단계가 완료되면 추가 작업 수행
        Debug.Log("프롤로그 완료");
        mainMenuButton.gameObject.SetActive(true); // 예: 메인 메뉴 버튼 활성화
    }

    private void InitializeSteps()
    {
        // 사용자의 이름에 따라 조사를 붙이는 메소드
        string caseMarker = KoreanPostposition.SubjectCaseMarkerEunNeun(PlayerPrefs.GetString("PlayerName"));
        string playerName = PlayerPrefs.GetString("PlayerName");
        // 스프라이트와 텍스트 추가 단계 정의
        steps.Add(new ShowSpriteStep(this, imageContainer, sprites[0], imagePrefab));
        steps.Add(new ShowSpriteStep(this, imageContainer, sprites[1], imagePrefab));
        steps.Add(new ShowTextStep(prologueTexts[0], "나 " + playerName + caseMarker + ",\n\n'해외 밴드' 야옹의 팬이다.")); // 플레이어 이름 출력
        steps.Add(new ShowSpriteStep(this, imageContainer, sprites[0], imagePrefab));
        steps.Add(new ClearTextStep(prologueTexts[0])); // 텍스트 2, 3 비활성화
        steps.Add(new ShowSpriteStep(this, imageContainer, sprites[2], imagePrefab));
        steps.Add(new ShowSpriteStep(this, imageContainer, sprites[3], imagePrefab));
        audioSourceCoroutine = StartCoroutine(PlaySound());
        steps.Add(new ShowTextStep(prologueTexts[1], "하\n\n지\n\n만\n..."));
        steps.Add(new ShowSpriteStep(this, imageContainer, sprites[4], imagePrefab));
        steps.Add(new ShowSpriteStep(this, imageContainer, sprites[5], imagePrefab));
        steps.Add(new ShowTextStep(prologueTexts[2], "아무도\n\n야옹을\n\n모른다\n..."));
        steps.Add(new ShowSpriteStep(this, imageContainer, sprites[6], imagePrefab));
        steps.Add(new ShowSpriteStep(this, imageContainer, sprites[7], imagePrefab));
        steps.Add(new ClearTextStep(prologueTexts[1], prologueTexts[2])); // 텍스트 2, 3 비활성화
        steps.Add(new ShowSpriteStep(this, imageContainer, sprites[8], imagePrefab));
        steps.Add(new ShowTextStep(prologueTexts[3], "그 어떤 역경이 닥치더라도..."));
        steps.Add(new ShowTextStep(prologueTexts[4], "나는 꼭\n\n야옹을 보고 말겠다."));
        steps.Add(new ClearTextStep(prologueTexts[3], prologueTexts[4]));
        steps.Add(new ShowSpriteStep(this, imageContainer, sprites[9], imagePrefab));
        steps.Add(new ShowSpriteStep(this, imageContainer, sprites[10], imagePrefab));
        steps.Add(new ShowSpriteStep(this, imageContainer, sprites[10], imagePrefab));
        steps.Add(new ShowPanelStep(panelPrefab));
        steps.Add(new ActivateButtonStep(mainMenuButton)); // 마지막 단계에서 버튼 활성화
    }

    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(7.8f);
        audioSource.clip = sadSound;
        audioSource.Play();
        yield return new WaitForSeconds(5.2f);
        audioSource.clip = waterSound;
        audioSource.Play();
        yield return new WaitForSeconds(1.3f);
        audioSource.Play();
    }

    private void ShowCurrentStep()
    {
        if (currentStepIndex < steps.Count)
        {
            steps[currentStepIndex].Execute();
        }
    }

    // 인터페이스 및 단계 클래스 정의
    private interface IPrologueStep
    {
        void Execute();
    }

    private class ShowSpriteStep : IPrologueStep
    {
        private PrologueManager manager;
        private Transform parent;
        private Sprite sprite;
        private GameObject prefab;

        public ShowSpriteStep(PrologueManager manager, Transform parent, Sprite sprite, GameObject prefab)
        {
            this.manager = manager;
            this.parent = parent;
            this.sprite = sprite;
            this.prefab = prefab;
        }

        public void Execute()
        {
            GameObject newImageGO = GameObject.Instantiate(prefab, parent);
            Image imageComponent = newImageGO.GetComponent<Image>();
            if (imageComponent != null)
            {
                imageComponent.sprite = sprite;
            }
        }
    }

    private class ShowTextStep : IPrologueStep
    {
        private TMP_Text textComponent;
        private string text;

        public ShowTextStep(TMP_Text textComponent, string text)
        {
            this.textComponent = textComponent;
            this.text = text;
        }

        public void Execute()
        {
            textComponent.text = text;
            textComponent.gameObject.SetActive(true);
        }
    }

    private class ClearTextStep : IPrologueStep
    {
        private TMP_Text[] textComponents;

        public ClearTextStep(params TMP_Text[] textComponents)
        {
            this.textComponents = textComponents;
        }

        public void Execute()
        {
            foreach (var textComponent in textComponents)
            {
                textComponent.gameObject.SetActive(false);
            }
        }
    }

    private class ActivateButtonStep : IPrologueStep
    {
        private Button button;

        public ActivateButtonStep(Button button)
        {
            this.button = button;
        }

        public void Execute()
        {
            button.gameObject.SetActive(true);
        }
    }

    private class ShowPanelStep : IPrologueStep
{
    private GameObject panel;

    public ShowPanelStep(GameObject panel)
    {
        this.panel = panel;
    }

    public void Execute()
    {
        panel.SetActive(true);
    }
}

}
