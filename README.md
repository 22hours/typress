# Typress
Typress(활자인쇄소를 영어로 번역함.)

# To-do ObjectModuling (19.09.30)

:computer: **Server**<br>

- 항시대기할 수 있는 4개의 포트활용.
  - 5000 : Login **(OK)**
  - 5001 : Main **(OK)**
  - 5002 : ControlBlock **(OK)**
  - 5003 : Printer **(OK)**
  <br><br>
- **Thread들이 같은 변수를 공유해야함!!!!!**<br>
- Main -> Login -> Main이 켜지게끔<br>
- Login -> MemberViewx(로그인이 되어있지 않음) -> Login
- (현재 로그인packet을 가지고 있지 못하는 듯?)
- Controlblock 로그인 안되어있어도 그냥 뜸. db못불러옴.<br><br>

  
:book: **참고사항**<br><br>
**프로세스 검색 :** `` tasklist | find /i "string" ``<br>
**프로세스 종료 :** `` taskkill /pid num /f ``<br><br><br>

:book: **개선사항**
- ViewHandler 프로젝트 단위로로 참조하자
