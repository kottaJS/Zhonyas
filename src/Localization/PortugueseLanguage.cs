namespace LoLFirewallManager.Localization
{
    public class PortugueseLanguage : ILanguage
    {
        public string Name { get { return "Português"; } }
        public string Code { get { return "pt"; } }

        public string StatusTitle { get { return "Status:"; } }
        public string StatusReady { get { return "Pronto"; } }
        public string StatusWaitingLoL { get { return "Aguardando o LoL..."; } }
        public string StatusWaitingClient { get { return "Aguardando o Cliente..."; } }
        public string StatusStasisActive { get { return "Estase Ativa"; } }
        public string StatusInGame { get { return "Em Jogo"; } }
        public string StatusStopped { get { return "Parado"; } }

        public string BtnStart { get { return "Iniciar"; } }
        public string BtnStop { get { return "Parar"; } }
        public string BtnReconnect { get { return "Reconectar"; } }
        public string ChkSounds { get { return "Sons"; } }
        public string ChkAutoRearm { get { return "Auto-rearmar"; } }

        public string MenuOptions { get { return "&Opções"; } }
        public string MenuHelp { get { return "&Ajuda"; } }
        public string MenuLanguage { get { return "&Idioma"; } }
        public string MenuOptGif { get { return "Animação da Lulu (Desativar p/ poupar CPU)"; } }
        public string MenuOptSounds { get { return "Alertas Sonoros"; } }
        public string MenuOptAutoRearm { get { return "Auto-rearmar Próxima Partida"; } }
        public string MenuExit { get { return "Sair"; } }
        public string MenuGuide { get { return "Como Usar (FAQ)"; } }
        public string MenuVanguard { get { return "Segurança e Vanguard Anti-Cheat"; } }
        public string MenuGithub { get { return "Visitar projeto no GitHub..."; } }
        public string MenuAbout { get { return "Sobre o Zhonyas"; } }

        public string TipToggle { get { return "Inicia ou para o monitoramento do League of Legends."; } }
        public string TipReconnect { get { return "Descongelamento de emergência. Desbloqueia o jogo imediatamente sem parar o Zhonyas."; } }
        public string TipDuration { get { return "Escolha quanto tempo pausar a tela de carregamento antes de reconectar (3.5 min recomendado)."; } }
        public string TipSounds { get { return "Toca sons ao iniciar estase, contagem de 10s e reconexão."; } }
        public string TipAutoRearm { get { return "Prepara automaticamente o Zhonyas para a próxima partida quando esta acabar."; } }
        public string TipLulu { get { return "Animação da Lulu. Clique ou desative em Opções para poupar CPU."; } }

        public string Dur1Min { get { return "1 min (Muito seguro)"; } }
        public string Dur2Min { get { return "2 min (Seguro)"; } }
        public string Dur3Min { get { return "3 min (Seguro)"; } }
        public string Dur35Min { get { return "3.5 min (Recomendado)"; } }
        public string Dur4Min { get { return "4 min (Arriscado - Pode iniciar)"; } }
        public string Dur5Min { get { return "5 min (Perigo - Risco de remake)"; } }
        public string Dur15Sec { get { return "15 seg (Teste rápido)"; } }

        public string GuideContent
        {
            get
            {
                return "COMO USAR O ZHONYAS:\n\n" +
                       "1. Escolha a duração desejada (3.5 min é o recomendado).\n" +
                       "2. Clique em 'Iniciar' (ou mantenha o 'Auto-rearmar' ativado).\n" +
                       "3. Na tela de carregamento, o Zhonyas segura sua conexão.\n" +
                       "4. Quando a contagem terminar, você reconecta automaticamente antes dos minions nascerem.\n" +
                       "5. Clique em 'Reconectar' a qualquer momento para desbloquear imediatamente.\n\n" +
                       "DICAS:\n" +
                       "- Em 'Opções', desative a animação da Lulu para zerar o uso de CPU.\n" +
                       "- Passe o cursor sobre os botões para ver suas explicações.";
            }
        }

        public string VanguardContent
        {
            get
            {
                return "SEGURANÇA E VANGUARD ANTI-CHEAT:\n\n" +
                       "O Zhonyas opera estritamente no Firewall do Windows.\n\n" +
                       "- NÃO injeta código no League of Legends.\n" +
                       "- NÃO modifica arquivos do jogo.\n" +
                       "- NÃO lê a memória do jogo.\n" +
                       "- Usa APIs oficiais de rede do Windows.\n\n" +
                       "Por isso, é 100% seguro contra banimentos do Vanguard.";
            }
        }

        public string AboutContent
        {
            get
            {
                return "Zhonyas v1.0.0\n" +
                       "Utilitário de Tela de Carregamento para League of Legends\n\n" +
                       "Criado por kottaJS\n" +
                       "Licenciado sob GPL-3.0";
            }
        }

        public string StopMsgBoth
        {
            get { return "Parar cancelará tanto a estase ativa quanto o auto-rearme para partidas futuras.\n\nUse Reconectar se quiser apenas desbloquear o jogo mais cedo."; }
        }

        public string StopMsgAutoRearm
        {
            get { return "Parar desligará o Zhonyas e cancelará o auto-rearme para partidas futuras.\n\nUse Reconectar se quiser apenas desbloquear o jogo mais cedo."; }
        }

        public string StopMsgStasis
        {
            get { return "Parar cancelará sua estase ativa.\n\nUse Reconectar se quiser apenas desbloquear o jogo mais cedo."; }
        }

        public string DontShowAgain { get { return "Não mostrar isso novamente"; } }
        public string BtnCancel { get { return "Cancelar"; } }

        public string TrayOpen { get { return "Abrir Zhonyas"; } }
        public string TrayReconnected { get { return "Reconectado! Jogo descongelado."; } }
        public string TrayArmed { get { return "Partida encerrada. Armado para a próxima!"; } }
    }
}

