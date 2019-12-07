# Typress
![image](https://user-images.githubusercontent.com/16419202/67943984-b0fb7a00-fc1e-11e9-8ffb-6be86ff058f9.png)
![image](https://user-images.githubusercontent.com/16419202/68527833-d11dee00-032e-11ea-9f00-80ea3b8a8815.png)

- **Typress**(활자인쇄소를 영어로 번역함).
- 동아리방 프린터멤버십제도 프로젝트인 **활자인쇄소(Typress)** 는 가톨릭대학교내 학회 및 동아리실을 이용하는 학생들을 대상으로 서비스를 런칭한 프로젝트이다.
- 현재 90% 진행되었음.(19-11-09 20:24)
- `typress-service` branch로 본격 진행중.
- `Socket` / `Multi-Process` / `Multi-Thread` / `.NET` / `WPF` / `MVVM Pattern` / `x64 Porting` / `OS`

# Typress 1.0.0 (19.11.12)
- 서비스로 세팅가능
- 수많은 버그들이 존재하지만 한걸음씩 한걸음씩.

:computer: **Server**<br>

- `Project`와 `Issues`를 참조
- 버전 명시
  

:book: **참고사항**<br><br>
**프로세스 검색 :** `` tasklist | find /i "string" ``<br>
**프로세스 종료 :** `` taskkill /pid num /f ``<br><br><br>


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
- ViewHandler 프로젝트 단위로로 참조하자

# service-app test
- Tutorial Ref. 
https://docs.microsoft.com/ko-kr/dotnet/framework/windows-services/walkthrough-creating-a-windows-service-application-in-the-component-designer
