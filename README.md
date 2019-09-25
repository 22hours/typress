# Typress
Typress(활자인쇄소를 영어로 번역함.)

# To-do(19.09.25)

**Server**<br>
===

- **인쇄 요청매수를 반환해온다. (NumberOfCopies)**
- ControlBlock ~ LoginView, ControlBlock ~ MainView 연결해야함
  - Packet.IsLogin에 따라 LoginView or MainView
  - (마일리지) DB 업데이트.
- Service App
- 인쇄 후 로그아웃에 대한 usecase(3가지의 갈래, 이어서 인쇄할 경우, 바로 종료하는 경우)
- 로그인 성공 후 MainView가 아닌 ControlBlock(마일리지차감)
  - 로그인폼이 꺼지는 시기는 일단 '로그인 성공' 및 '사용자가 창닫기.'
  - 인쇄가 되는 시점은 ControlBlock에서 마일리지 차감에 성공했을 때.
- MVVM에 맞는 패턴화 유지

**Client**<br>

- Client 기능수정(Login 성공->창 닫기)
- license page 

**---> 기본적인 버그찾아내기**<br>
**---> 배포**<br>

<br>

**추후추후 정말나중에 일단 잘되고나서**<br>
-----> 'AdminPage' 를 만들고 싶어졌다.

- '각 동아리회장용' <- Java 웹페이지로 투닥투닥할 수 있는. DB서버만 건드리면 되는.
- 지금 정환이가 만들어 놓은 것은 '22Hours'(Typress회사꺼)가 각 동아리 회장들을 관리하는 것. 'C#'
- 우리 DB에 접속하기 위함 
