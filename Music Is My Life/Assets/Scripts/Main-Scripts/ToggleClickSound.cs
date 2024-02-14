using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleClickSound : MonoBehaviour
{
    public Toggle toggle; // 토글 컴포넌트

    public AudioClip clickSound; // 클릭 사운드

    private AudioSource audioSource; // 오디오 소스 컴포넌트

    void Start()
    {
        // 오디오 소스 컴포넌트 가져오기
        audioSource = GetComponent<AudioSource>();

        // 토글 컴포넌트의 클릭 이벤트에 메소드 연결
        toggle.onValueChanged.AddListener(OnToggleClicked);
    }

    // 토글 컴포넌트가 클릭되었을 때 호출되는 메소드
    void OnToggleClicked(bool isOn)
    {
        // 토글 컴포넌트가 체크되었을 때만 사운드 재생
        if (isOn)
        {
            if (clickSound != null && audioSource != null)
            {
                // 사운드 재생
                audioSource.PlayOneShot(clickSound);
            }
        }
    }
}
