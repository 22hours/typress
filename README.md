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
- 로그인 되어있는 상태에서 -> 프린트요청 -> MainView 띄움.
- DB 
- ViewModel에서 View제어.(this.Close()) **(Client)**
- MainView에서 Logout구현 필요.(this.Close()) **(Client)**

  
:book: **참고사항**<br><br>
**프로세스 검색 :** `` tasklist | find /i "string" ``<br>
**프로세스 종료 :** `` taskkill /pid num /f ``<br><br><br>

:book: **개선사항**
- ViewHandler 프로젝트 단위로로 참조하자
