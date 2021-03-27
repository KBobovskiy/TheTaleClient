using DataBaseContext.DTO;

namespace DataBaseContext
{
    public interface IEfCoreDao
    {
        public CookieDto LoadCookie(int AccountId);

        public void SaveCookie(int AccountId, CookieDto Cookie);

        public void SaveTurnAsync(TurnDto heroInfo);

        public HeroInfoDto[] SelectLatestHeroInfosAsync(int entryNumber);

        public void SaveHeroInfoAsync(HeroInfoDto heroInfo);

        public void SaveLogEventAsync(LogEventDto logEvent);
    }
}