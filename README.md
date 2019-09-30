# Typress
Typress(활자인쇄소를 영어로 번역함.)

# To-do(19.09.26)

:computer: **Server**<br>


- **인쇄 요청매수를 반환해온다.**
  - PrintTicket : 
  - https://docs.microsoft.com/ko-kr/dotnet/api/system.printing.printticket?view=netframework-4.8
- ControlBlock ~ MainView 연결해야함
  - 어떤식으로 할지 고민.
  - Packet.IsLogin에 따라 LoginView or MainView, 
    - printLogger Class에서 Packet에 대한 공유가 이뤄져야 함.
  - (마일리지) DB 업데이트.
- Service App
- 로그인 성공 후 MainView가 아닌 ControlBlock(마일리지차감)
  - 로그인폼이 꺼지는 시기는 일단 '로그인 성공' 및 '사용자가 창닫기.'
  - 인쇄가 되는 시점은 ControlBlock에서 마일리지 차감에 성공했을 때.
- MVVM에 맞는 패턴화 유지.
  - ObjectModuling Branch 진행 중
  - 5000(로그인), 5001(메인), 5002(인쇄안내창), 5003(프린트) 수신대기

# 
:computer: **Client**<br>

- 마일리지를 업데이트 하고나서, 창을 다시 띄워야하는????
- (Delay) ☆ClickLogin V와 VM로 패턴화<br>
<br>
**---> 기본적인 버그찾아내기**<br>
**---> 배포**<br>

<br>

# 
**추후추후 정말나중에 일단 잘되고나서**<br>
-----> 'AdminPage' 를 만들고 싶어졌다.<br>
-----> '역공학'을 추가하고 싶어졌다.(모듈화가 잘 안지켜진 프로젝트)<br>

- '각 동아리회장용' <- Java 웹페이지로 투닥투닥할 수 있는. DB서버만 건드리면 되는.
- 지금 정환이가 만들어 놓은 것은 '22Hours'(Typress회사꺼)가 각 동아리 회장들을 관리하는 것. 'C#'
- 우리 DB에 접속하기 위함 
