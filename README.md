# Typress
Typress(활자인쇄소를 영어로 번역함.)

# To-do ObjectModuling (19.09.30)

:computer: **Server**<br>

- 항시대기할 수 있는 4개의 포트활용.
  - 5000 : Login **(OK)**
  - 5001 : Main **(OK)**
  - 5002 : ControlBlock **(OK)**
  - 5003 : Printer
  <br>
- Main -> Login -> Main이 켜지게끔<br><br>
  
:book: **참고사항**<br><br>
**프로세스 검색 :** `` tasklist | find /i "string" ``<br>
**프로세스 종료 :** `` taskkill /pid num /f ``<br>
