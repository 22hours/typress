# Typress
Typress(활자인쇄소를 영어로 번역함).

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


<br><br>
# typress-service installation
- typress 레포지를 pull 
- `typress-service` branch 로 변환
- Visual Studio [관리자 모드] 실행 후, typress-service에 있는 솔루션 빌드
  - x64, Debug | Release로 빌드
  - 프로젝트 속성에서 매니패스트 생성하지 않고 빌드
- vs 2017용 x64 네이티브 도구 명령프롬프트에서, 빌드된 .exe가 있는 디렉토리로 이동
- `installutil MyService.exe`
- **서비스**에서 `22HoursService` 실행


# service-app test
- Tutorial 
  - https://docs.microsoft.com/ko-kr/dotnet/framework/windows-services/walkthrough-creating-a-windows-service-application-in-the-component-designer

- Service Delete (PowerShell)
  - https://docs.microsoft.com/ko-kr/dotnet/framework/windows-services/how-to-install-and-uninstall-services
  
- Application Loader
  - https://darbyy.tistory.com/20
  - https://www.codeproject.com/Articles/35773/Subverting-Vista-UAC-in-Both-32-and-64-bit-Archite
