"**GEEKS, GGC(글로벌 게임 챌린지) 2024**"에 출품 및 시연을 진행한 "사이드뷰 플랫포머 형식 퍼즐 어드벤쳐 - Time Player" 입니다.

---

Youtube: [Walkthrough Video](https://youtu.be/eWNVUPxRnD8) (컷편집 & 자막)

### 1. 프로젝트 소개

- Unity Engine을 기반으로 제작된 2D 플랫포머 형식 퍼즐 어드벤쳐 게임 제작 프로젝트입니다.


### 2. 프로젝트 개요

- 개발 기간: 6주
- 개발 도구: Unity 2022.3.45f1
- 개발 언어: C#

- **프로젝트 최종 클래스 다이어그램**
  
<div align="center">
<img src="https://github.com/user-attachments/assets/67bcbc0a-a2d7-47d3-8f70-8727d0201c54" alt="image">
</div>


### 3. 기술

- Unity
  - 보스 및 플레이어를 FSM으로 관리하였습니다.
  
- Git
  - Terminal-GitBash를 활용했고, Branch는 Git-Flow 형식으로 관리하였습니다.
  - VS Code의 Git Graph Extension을 활용하였습니다.
    
- 생성형 AI
  - SUNO AI Music
  - Midjourney
  - Chat GPT 개발 참고용
    
 - 디자인
    - adobe photoshop
    - adobe illustrator
    - pixelable

### 4. 플레이 설명

- 조작법
>이동: WASD 이동
>
>점프: Space bar

  - 스킬:
>Q : 시간 정지
>
>LCTRL : 대쉬
>
>마우스 좌/우 클릭 : 해당 영역 시간을 느리고/빠르게 흐르도록 지정

### 5. 구현 내용

- 레벨 디자인 요소

	- **카메라 구간별 이동**: 체크포인트 위치에 도달하면 카메라가 구간별로 이동합니다.
   
  - **포탈**: 포탈을 통해 다음 씬으로 이동할 수 있습니다.
    
  - **함정(떨어지는 나무)**: 특정 구역에 들어가면 떨어지는 나무를 구현하였습니다. 떨어지는 나무에 닿으면 가장 최근에 도달했던 체크포인트 위치로 이동합니다. 

  - **점프로 도달할 수 없는 높은 영역**: 어린 나무의 빠른 성장을 유도해, 성장된 나무를 밟고 높은 구역으로 이동할 수 있습니다.

  - **점프나 대쉬를 통해 이동할 수 없는 영역 사이**: 오래된 나무의 시간을 빠르게 돌려 나무의 수명을 다하게 만들어 쓰러지게 만듭니다. 다리 역할을 하게 된 나무를 밟고 이동할 수 있습니다.

  - **낙하데미지**: 플레이어가 높은 곳에서 떨어지면 낙하데미지를 받게 되어 가장 최근 체크포인트 지점으로 이동하게 됩니다. 따라서 플레이어가 지니고 있는 스킬을 적절히 활용해 게임을 해결해나가야 합니다.
    
- 인게임 내 설정

  - 시작 및 옵션 버튼
    
  - bgm, effect sound 조절 slider
    
  - esc 일시정지 및 back, restart, exit 버튼

    
### 6. 스테이지별 설명

#### a. 일반맵

- 일반맵 최종 숲 배경
  
<div align="center">
<img src="https://github.com/user-attachments/assets/81ce5bb0-460e-4b3b-9d67-aaac727f34f8" alt="image">
</div>

 - 일반맵(숲)

	- **떨어지는 나무**: 머리에 맞으면 가장 최근에 도달했던 체크포인트 위치로 이동합니다.
   
	- **자라는 나무**: 나무의 성장을 촉진시켜 나무를 통해 높은 곳으로 도달 가능합니다.
   
	- **쓰러지는 나무**: 금이 간 나무의 노화를 가속시켜 쓰러뜨리고, 나무를 밟고 이동 할 수 있습니다.
   
#### b. 보스맵


- 보스전 최종 숲 배경
  
<div align="center">
<img src="https://github.com/user-attachments/assets/0a01d436-64f1-4064-83d1-9a1ec9b547ae" alt="image">
</div>

- 보스맵(숲)
  
  - **떨어지는 나무 및 날아오는 돌**: 일정 횟수 이상 맞으면 재시작되며, 날아오기 전, 날아올 오브젝트의 궤적이 표시됩니다.
    
  - **씨앗**: 먹으면 자라는 나무가 생성됩니다.
    
  - **자라는 나무(보스버전)**: 시간을 빠르게 돌려 나무를 성장시켜 보스에게 찌르기 공격이 가능합니다.
    
  - **상황 별 보스 애니메이션**: 스켈레톤 애니메이션이 적용되었습니다.


### 7. 상세 내용

<div align="center">
<img src="https://github.com/user-attachments/assets/6dd01fc3-04bc-4dfc-9e66-4b38416cd913" alt="image">
</div>

#### 일반맵

- 낙하 데미지 감소
<div align="center">
<img src="https://github.com/user-attachments/assets/14445a0e-bea4-4e6d-b109-8b7bc236618e" alt="image" width="500">
</div>

<br>

- 시간 조절
<div align="center">
<img src="https://github.com/user-attachments/assets/0588f3b4-c9be-43b1-91f0-e6961601b660" alt="image" width="300">
</div>

<br>

- 시간 정지
<div align="center">
<img src="https://github.com/user-attachments/assets/b09bc695-0e87-45ce-a713-4644ed9c9c89" alt="image" width="500">
</div>

<br>

- 떨어지는 나무 밟고 이동
<div align="center">
<img src="https://github.com/user-attachments/assets/51911dd8-1146-452d-9ef2-500476aef4ea" alt="image" width="300">
</div>

<br>

- 일시정지 화면
<div align="center">
<img src="https://github.com/user-attachments/assets/d3dbbcd6-6af2-4fec-994d-af5ce91385e7" alt="image" width="500">
</div>

<br>

#### 보스전

- 씨앗

<div align="center">
<img src="https://github.com/user-attachments/assets/355a4ae9-7e7b-487e-97a2-0209fd0a463f" alt="보스전-씨앗" width="250">
</div>

<br>

- 떨어지는 나무 궤적

<div align="center">
<img src="https://github.com/user-attachments/assets/e8b4871c-a2a6-4584-8bf5-d923c3e1763a" alt="보스전-떨어지는 나무 궤적" width="500">
</div>

<br>

- 날아오는 돌 궤적

<div align="center">
<img src="https://github.com/user-attachments/assets/91e686f8-499a-4bd9-b8fa-3ba00775fe06" alt="보스전-날아오는 돌 궤적" width="500">
</div>

<br>

- 공격용 묘목 성장

<div align="center">
<img src="https://github.com/user-attachments/assets/5ee8f443-402f-4fb0-b4ae-7d2b25d592c8" alt="보스전-공격용 묘목 성장" width="250">
</div>

#### 공통


- 메인 화면

<div align="center">
<img src="https://github.com/user-attachments/assets/7405a7d3-eb20-4a88-b170-d4ceb4e9d03d" alt="메인화면" width="500">
</div>

<br>

- setting slider
<div align="center">
<img src="https://github.com/user-attachments/assets/45ec370b-19e2-4f80-93d4-9257485e1122" alt="메인화면" width="500">
</div>
<br>

- 플레이어 대쉬

<div align="center">
<img src="https://github.com/user-attachments/assets/03c48049-ca49-413a-9c29-0fb77c245c60" alt="공통-플레이어 대쉬" width="300">
</div>

<br>

- Ending

<div align="center">
<img src="https://github.com/user-attachments/assets/29529239-a7be-4166-a67e-33a32db9e697" alt="엔딩" width="500">
</div>


### 8. 팀 정보

 > 프로젝트 : Time Player
<br>
<table align="center" width="788">
<thead>
<tr>
<th width="130" align="center">성명</th>
<th width="270" align="center">소속</th>
<th width="150" align="center">역할</th>
<th width="100" align="center">깃허브</th>
<th width="180" align="center">이메일</th>
</tr> 
</thead>
<tbody>
<tr>
<td width="130" align="center">윤지석<br/>(팀장)</td>
<td width="270" align="center">Ajou Univ - Software</td>
<td width="150">게임 메인 기획<br/>PM</td>
<td width="100" align="center">
   <a href="https://github.com/Cayde-Yun">
      <img src="http://img.shields.io/badge/CaydeYun-655ced?style=social&logo=github"/>
   </a>
</td>
<td width="175" align="center">
   <a href="mailto:jiseoky@ajou.ac.kr">
                <img src="https://img.shields.io/badge/jiseoky-655ced?style=social&logo=gmail"/>
   </a>
</td>
</tr>
<tr>
<td width="130" align="center">손현진</td>
<td width="270" align="center">Ajou Univ - Digital Media</td>
<td width="150">클라이언트 개발</td>
<td width="100" align="center">
   <a href="https://github.com/sonyrainy">
      <img src="http://img.shields.io/badge/sonyrainy-655ced?style=social&logo=github"/>
   </a>
</td>
<td width="175" align="center">
   <a href="mailto:thsguswls610@gmail.com">
                <img src="https://img.shields.io/badge/thsguswls610-655ced?style=social&logo=gmail"/>
   </a>
</td>
</tr>
<tr>
<td width="130" align="center">오현택</td>
<td width="270" align="center">Ajou Univ - Digital Media</td>
<td width="150">클라이언트 개발</td>
<td width="100" align="center">
   <a href="https://github.com/Ohhyuntaek">
      <img src="http://img.shields.io/badge/Ohhyuntaek-655ced?style=social&logo=github"/>
   </a>
</td>
<td width="175" align="center">
   <a href="mailto:penguin4404@ajou.ac.kr">
                <img src="https://img.shields.io/badge/penguin4404-655ced?style=social&logo=gmail"/>
   </a>
</td>
</tr>
<tr>
<td width="130" align="center">김하연</td>
<td width="270" align="center">Ajou Univ - Department of Sociology</td>
<td width="150">디자인</td>
<td width="100" align="center">
  x
</td>
<td width="175" align="center">
   <a href="mailto:gkdus10@ajou.ac.kr">
                <img src="https://img.shields.io/badge/gkdus10-655ced?style=social&logo=gmail"/>
   </a>
</td>
</tr>
</tbody>
</table>
<br>

