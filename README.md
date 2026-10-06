# CasualGameProject

랜덤 디펜스 장르의 캐주얼 모바일 게임 + 자그마한 서버(HTTP + WebSocket) + 데이터 파이프라인 툴

| 폴더 | 내용 |
|---|---|
| `Client/` | Unity 6 클라이언트 (6000.3.11f1) |
| `Server/` | ASP.NET Core 서버 (Web API + WebSocket) |
| `Shared/` | 클라·서버 공용 패킷 정의, 에러 코드 |
| `DataTable/` | Google Sheets에서 변환한 JSON (원본은 Sheets, 직접 수정 금지) |
| `Docs/` | 기획, 설계, 트러블슈팅 문서 ([목차](Docs/README.md)) |

> Unity Hub에서는 `Client/` 폴더를 프로젝트로 열어야 합니다.
