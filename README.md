# Typress
Typress(활자인쇄소를 영어로 번역함.)

# To-do ObjectModuling (19.09.30)

:computer: **Server**<br>

- 항시대기할 수 있는 4개의 포트활용.
  - 5000 : Login **(OK)**
  - 5001 : Main 
  - 5002 : ControlBlock
  - 5003 : Printer
  <br>
- Main을 띄워야할지, Block을 띄워야할지, 어떻게 프로그램이 실행됬는지를 알아야 할 듯하다.
  - 내가 Window에서 직접실행 시켰을 경우 : MainView
  - PrintInterrupt 가 됬을 경우 : ControlBlock
  - Args 변수활용 Idea? 
