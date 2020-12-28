ASP.NET CORE로 API 제작하기
======
## 개요
  ASP.NET Core로 웹 API를 빌드하는 과정을 설명합니다.  
  해당 예제는 visual Studio 2019으로 진행됩니다.
## 시작하기
  1. ASP.NET CORE 프로젝트 생성
  2. Model로 사용할 Class와 EntityFrameWork DataBase Context 추가
  3. CRUD를 위한 메서드로 스캐폴드 컨트롤러 생성
  4. 라우팅, URL 경로 및 메서드 반환 값 구성
  5. PostMan을 이용한 API 호출 테스트
  
  |API|기술|요청 본문|응답 본문|
  |------|---|---|---|
  |GET /api/|모든 할 일 항목 가져 오기|없음|할 일 항목 배열|
  |GET /api/TodoItems/{id}|ID로 항목 받기|없음|할 일 항목|
  |POST /api/TodoItems|새 항목 추가|할 일 항목|할 일 항목|
  |PUT /api/TodoItems/{id}|기존 항목 업데이트|할 일 항목|없음|
  |DELETE /api/TodoItems/{id}|항목 삭제|없음|없음|
  
  ### 1. ASP.NET CORE 프로젝트 생성
  * 비주얼 스튜디오에서 새로 만들기 > 새 프로젝트  
  ![생성](https://github.com/LimChaeJune/ASP.NET-CORE-Web-API/blob/master/1.png)
  
  
  
  
