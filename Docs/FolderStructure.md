# 폴더 구조 규칙

Where-Are-You-Looking-At 프로젝트의 번호 방식(`00.` 접두사)을 따른다.

## Client/Assets
| 폴더 | 내용 |
|---|---|
| `00.Scenes` | 씬 |
| `01.Scripts` | 스크립트 (아래 참고) |
| `02.Prefabs` | 프리팹 |
| `03.Images` | 스프라이트, 텍스처 |
| `04.Data` | JSON 데이터 테이블, 설정 파일 |
| `05.ScriptableObject` | SO 에셋 |
| `06.SpriteAtlas` | 스프라이트 아틀라스 |
| `07.VFX` | 파티클, 이펙트 |
| `08.Sounds` | 프로젝트용 사운드, 믹서 |
| `09.Fonts` | 폰트 에셋 (TMP SDF) |
| `10.Animations` | 애니메이션, 컨트롤러 |
| `11.Materials` | 머티리얼, 셰이더 |
| `99.ThirdParty` | 외부 에셋 원본 (**유료 포함, 공개 시 제외**) |
| `Plugins` | DOTween 등 플러그인 |

## Client/Assets/01.Scripts
| 폴더 | 내용 |
|---|---|
| `00.Common` | Define, Enum, Interface, ObjectPool 등 공용 |
| `01.Unit` | 아군 유닛 |
| `02.Summon` | 랜덤 소환, 합성 |
| `03.Projectile` | 투사체 |
| `05.UI` | UI |
| `08.Monster` | 몬스터 |
| `10.Wave` | 웨이브, 스테이지 진행 |
| `12.Reward` | 보상 |
| `20.FloatingText` | 데미지 텍스트 |
| `22.Effect` | 이펙트 제어 |
| `91.Sounds` | 사운드 제어 |
| `92.Network` | HTTP, WebSocket, 패킷 |
| `94.Save` | 로컬 저장 |
| `95.GameRule` | 게임 규칙 데이터 |
| `97.Converter` | 데이터 변환 |
| `98.Extension` | 확장 메서드 |
| `99.Manager` | 전역 매니저 (Game, Pool, Data, UI, EventBus) |
| `Editor` | 에디터 툴 (DataTool) |

## 규칙
- 상수는 `Define`, 열거형은 `CasualGame.Enum` 네임스페이스에 `#region`으로 묶어 정의한다.
- 매니저는 `Instance` 싱글톤 패턴을 사용한다.
- 시스템 간 통신은 `EventBus`를 사용하고, 구독한 쪽에서 반드시 해제한다.
- 새 기능 폴더가 필요하면 비어 있는 번호를 사용한다.
