namespace LoLFirewallManager.Localization
{
    public class KoreanLanguage : ILanguage
    {
        public string Name { get { return "한국어 (Korean)"; } }
        public string Code { get { return "ko"; } }

        public string StatusTitle { get { return "상태:"; } }
        public string StatusReady { get { return "준비됨"; } }
        public string StatusWaitingLoL { get { return "롤 대기 중..."; } }
        public string StatusWaitingClient { get { return "클라이언트 대기 중..."; } }
        public string StatusStasisActive { get { return "존야 활성화"; } }
        public string StatusInGame { get { return "게임 중"; } }
        public string StatusStopped { get { return "중지됨"; } }

        public string BtnStart { get { return "시작"; } }
        public string BtnStop { get { return "중지"; } }
        public string BtnReconnect { get { return "재연결"; } }
        public string ChkSounds { get { return "효과음"; } }
        public string ChkAutoRearm { get { return "자동 재준비"; } }

        public string MenuOptions { get { return "&옵션"; } }
        public string MenuHelp { get { return "&도움말"; } }
        public string MenuLanguage { get { return "&언어"; } }
        public string MenuOptGif { get { return "룰루 애니메이션 (CPU 절약)"; } }
        public string MenuOptSounds { get { return "사운드 알림"; } }
        public string MenuOptAutoRearm { get { return "다음 매치 자동 재준비"; } }
        public string MenuExit { get { return "종료"; } }
        public string MenuGuide { get { return "사용 가이드 (FAQ)"; } }
        public string MenuVanguard { get { return "안티치트 및 뱅가드 안전성"; } }
        public string MenuGithub { get { return "GitHub 프로젝트 방문..."; } }
        public string MenuAbout { get { return "Zhonyas 정보"; } }

        public string TipToggle { get { return "리그 오브 레전드 모니터링을 시작하거나 중지합니다."; } }
        public string TipReconnect { get { return "긴급 차단 해제. Zhonyas를 끄지 않고 즉시 연결합니다."; } }
        public string TipDuration { get { return "재연결하기 전 로딩 화면을 멈출 시간을 선택하세요 (3.5분 권장)."; } }
        public string TipSounds { get { return "정체 시작, 10초 카운트다운 및 재연결 시 소리를 재생합니다."; } }
        public string TipAutoRearm { get { return "현재 게임이 끝나면 다음 게임을 위해 자동으로 준비합니다."; } }
        public string TipLulu { get { return "룰루 애니메이션. 클릭하거나 옵션에서 꺼서 CPU를 절약하세요."; } }

        public string Dur1Min { get { return "1분 (매우 안전)"; } }
        public string Dur2Min { get { return "2분 (안전)"; } }
        public string Dur3Min { get { return "3분 (안전)"; } }
        public string Dur35Min { get { return "3.5분 (권장)"; } }
        public string Dur4Min { get { return "4분 (위험 - 게임이 시작될 수 있음)"; } }
        public string Dur5Min { get { return "5분 (위험 - 다시하기 위험)"; } }
        public string Dur15Sec { get { return "15초 (빠른 테스트)"; } }

        public string GuideContent
        {
            get
            {
                return "ZHONYAS 사용 방법:\n\n" +
                       "1. 원하는 지연 시간을 선택하세요 (3.5분을 권장합니다).\n" +
                       "2. '시작'을 클릭하세요 (또는 '자동 재준비' 유지).\n" +
                       "3. 게임 로딩 화면에서 Zhonyas가 일시적으로 연결을 멈춥니다.\n" +
                       "4. 카운트다운이 끝나면 미니언 생성 전에 자동으로 재연결됩니다.\n" +
                       "5. 언제든지 '재연결'을 클릭하여 즉시 해제할 수 있습니다.\n\n" +
                       "팁:\n" +
                       "- '옵션'에서 룰루 애니메이션을 끄면 CPU 점유율이 0%가 됩니다.\n" +
                       "- 버튼에 마우스를 올리면 상세 설명을 확인할 수 있습니다.";
            }
        }

        public string VanguardContent
        {
            get
            {
                return "안티치트 및 뱅가드 안전성:\n\n" +
                       "Zhonyas는 순수하게 Windows 방화벽 수준에서 작동합니다.\n\n" +
                       "- 롤 클라이언트에 코드를 주입하지 않습니다.\n" +
                       "- 게임 파일을 수정하지 않습니다.\n" +
                       "- 게임 메모리를 읽지 않습니다.\n" +
                       "- 공식 Windows 네트워킹 API만 사용합니다.\n\n" +
                       "따라서 뱅가드에 의한 정지 위험이 전혀 없습니다.";
            }
        }

        public string AboutContent
        {
            get
            {
                return "Zhonyas v1.0.0\n" +
                       "리그 오브 레전드 로딩 화면 유틸리티\n\n" +
                       "제작: kottaJS\n" +
                       "GPL-3.0 오픈 소스 라이선스";
            }
        }

        public string StopMsgBoth
        {
            get { return "중지하면 현재 존야 상태와 다음 게임 자동 준비가 모두 취소됩니다.\n\n조기 재연결을 원하시면 '재연결'을 누르세요."; }
        }

        public string StopMsgAutoRearm
        {
            get { return "중지하면 Zhonyas가 꺼지고 다음 게임 자동 준비가 취소됩니다.\n\n조기 재연결을 원하시면 '재연결'을 누르세요."; }
        }

        public string StopMsgStasis
        {
            get { return "중지하면 현재 존야 정체 상태가 취소됩니다.\n\n조기 재연결을 원하시면 '재연결'을 누르세요."; }
        }

        public string DontShowAgain { get { return "다시 표시하지 않음"; } }
        public string BtnCancel { get { return "취소"; } }

        public string TrayOpen { get { return "Zhonyas 열기"; } }
        public string TrayReconnected { get { return "재연결 완료! 게임 멈춤이 해제되었습니다."; } }
        public string TrayArmed { get { return "매치 종료. 다음 매치를 위해 준비되었습니다!"; } }
    }
}

