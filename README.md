
## 3D 핵엔슬래시 게임 Team Project
- 담당: Player

## 주요 기술
- Handler Pattern 기반 모듈형 아키텍처
- Command Pattern 입력 버퍼 시스템
- State Pattern 플레이어 상태 관리
- ScriptableObject 데이터 설계

## 핵심 폴더
02.Scripts/Player/

└── Player/

    ├── Handler/           # 5개 Handler 클래스
    ├── State/            # 6개 State 클래스  
    ├── Command/          # Command 구현체
    ├── Context/          # Context 객체들
    └── Config/           # 플레이어 설정 데이터

## 주요 클래스
- 'Player.cs': Handler를 관리하는 Facade 클래스
- 'PlayerStatsHandler.cs': 상태 및 데이터 관리
- 'PlayerCombatHandler.cs': 전투 시스템
- 'PlayerMovementHandler.cs': 플레이어 이동 및 물리 처리
