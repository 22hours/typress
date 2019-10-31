# Typress
![image](https://user-images.githubusercontent.com/16419202/67943984-b0fb7a00-fc1e-11e9-8ffb-6be86ff058f9.png)
- Typress(활자인쇄소를 영어로 번역함).
- 동아리방 프린터멤버십제도 프로젝트인 **활자인쇄소(Typress)** 는 가톨릭대학교내 학회 및 동아리실을 이용하는 학생들을 대상으로 서비스를 런칭한 프로젝트이다.



# To-do ObjectModuling (19.09.30)

:computer: **Server**<br>

- 로그인 되어있는 상태에서 -> 프린트요청(5003번포트 미사용했기 떄문.) -> X
- 로그인 되어있는 상태에서 -> serverstop.
- 팬실에서 점검해보기 <- dll포함 컴파일이 안되고 있음.
- ViewModel에서 View제어.(this.Close()) **(Client)**
- MainView에서 Logout구현 필요.(this.Close()) **(Client)**
<br>
  
:book: **참고사항**<br><br>
**프로세스 검색 :** `` tasklist | find /i "string" ``<br>
**프로세스 종료 :** `` taskkill /pid num /f ``<br><br><br>

:book: **개선사항**
- ViewHandler 프로젝트 단위로로 참조하자
