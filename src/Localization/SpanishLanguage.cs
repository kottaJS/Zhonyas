namespace LoLFirewallManager.Localization
{
    public class SpanishLanguage : ILanguage
    {
        public string Name { get { return "Español"; } }
        public string Code { get { return "es"; } }

        public string StatusTitle { get { return "Estado:"; } }
        public string StatusReady { get { return "Listo"; } }
        public string StatusWaitingLoL { get { return "Esperando al LoL..."; } }
        public string StatusWaitingClient { get { return "Esperando al Cliente..."; } }
        public string StatusStasisActive { get { return "Stasis Activa"; } }
        public string StatusInGame { get { return "En Partida"; } }
        public string StatusStopped { get { return "Detenido"; } }

        public string BtnStart { get { return "Iniciar"; } }
        public string BtnStop { get { return "Detener"; } }
        public string BtnReconnect { get { return "Reconectar"; } }
        public string ChkSounds { get { return "Sonidos"; } }
        public string ChkAutoRearm { get { return "Auto-rearmar"; } }

        public string MenuOptions { get { return "&Opciones"; } }
        public string MenuHelp { get { return "&Ayuda"; } }
        public string MenuLanguage { get { return "&Idioma"; } }
        public string MenuOptGif { get { return "Animación de Lulu (Desactivar p/ ahorrar CPU)"; } }
        public string MenuOptSounds { get { return "Alertas de Sonido"; } }
        public string MenuOptAutoRearm { get { return "Auto-rearmar Próxima Partida"; } }
        public string MenuExit { get { return "Salir"; } }
        public string MenuGuide { get { return "Cómo Usar (FAQ)"; } }
        public string MenuVanguard { get { return "Seguridad y Vanguard Anti-Cheat"; } }
        public string MenuGithub { get { return "Visitar proyecto en GitHub..."; } }
        public string MenuAbout { get { return "Acerca de Zhonyas"; } }

        public string TipToggle { get { return "Inicia o detiene el monitoreo de League of Legends."; } }
        public string TipReconnect { get { return "Descongelar de emergencia. Desbloquea el juego de inmediato sin detener Zhonyas."; } }
        public string TipDuration { get { return "Elige cuánto tiempo pausar la pantalla de carga antes de reconectar (3.5 min recomendado)."; } }
        public string TipSounds { get { return "Reproduce sonidos al iniciar stasis, cuenta regresiva de 10s y reconexión."; } }
        public string TipAutoRearm { get { return "Prepara automáticamente Zhonyas para la siguiente partida cuando termine la actual."; } }
        public string TipLulu { get { return "Animación de Lulu. Haz clic o desactívala en Opciones para ahorrar CPU."; } }

        public string Dur1Min { get { return "1 min (Muy seguro)"; } }
        public string Dur2Min { get { return "2 min (Seguro)"; } }
        public string Dur3Min { get { return "3 min (Seguro)"; } }
        public string Dur35Min { get { return "3.5 min (Recomendado)"; } }
        public string Dur4Min { get { return "4 min (Riesgoso - Puede iniciar)"; } }
        public string Dur5Min { get { return "5 min (Peligro - Riesgo remake)"; } }
        public string Dur15Sec { get { return "15 seg (Prueba rápida)"; } }

        public string GuideContent
        {
            get
            {
                return "CÓMO USAR ZHONYAS:\n\n" +
                       "1. Elige la duración deseada para pausar la carga (3.5 min recomendado).\n" +
                       "2. Dale a 'Iniciar' (o deja 'Auto-rearmar' activado).\n" +
                       "3. En la pantalla de carga del juego, Zhonyas congela temporalmente tu conexión.\n" +
                       "4. Al terminar el contador, te reconectas automáticamente antes de que salgan los súbditos.\n" +
                       "5. Puedes darle a 'Reconectar' en cualquier momento para descongelar al instante.\n\n" +
                       "CONSEJOS:\n" +
                       "- En 'Opciones', desactiva la animación de Lulu para bajar el consumo de CPU al 0%.\n" +
                       "- Pasa el cursor por encima de cualquier botón para ver su descripción.";
            }
        }

        public string VanguardContent
        {
            get
            {
                return "SEGURIDAD Y VANGUARD ANTI-CHEAT:\n\n" +
                       "Zhonyas opera estrictamente a nivel del Cortafuegos de Windows.\n\n" +
                       "- NO inyecta código en League of Legends.\n" +
                       "- NO modifica archivos del juego.\n" +
                       "- NO lee la memoria del juego.\n" +
                       "- Usa las APIs oficiales de red de Windows.\n\n" +
                       "Por esta razón, es 100% seguro y no provoca sanciones de Vanguard.";
            }
        }

        public string AboutContent
        {
            get
            {
                return "Zhonyas v1.0.0\n" +
                       "Utilidad de Pantalla de Carga para League of Legends\n\n" +
                       "Creado por kottaJS\n" +
                       "Licenciado bajo GPL-3.0";
            }
        }

        public string StopMsgBoth
        {
            get { return "Detener cancelará tanto la stasis activa como el auto-rearme para futuras partidas.\n\nUsa Reconectar si solo quieres desbloquear la partida antes."; }
        }

        public string StopMsgAutoRearm
        {
            get { return "Detener apagará Zhonyas y cancelará el auto-rearme para futuras partidas.\n\nUsa Reconectar si solo quieres desbloquear la partida antes."; }
        }

        public string StopMsgStasis
        {
            get { return "Detener cancelará la stasis activa.\n\nUsa Reconectar si solo quieres desbloquear la partida antes."; }
        }

        public string DontShowAgain { get { return "No volver a mostrar esto"; } }
        public string BtnCancel { get { return "Cancelar"; } }

        public string TrayOpen { get { return "Abrir Zhonyas"; } }
        public string TrayReconnected { get { return "¡Reconectado! Juego descongelado."; } }
        public string TrayArmed { get { return "Partida terminada. ¡Armado para la siguiente!"; } }
    }
}

