using Botec.Domain.Entities;

namespace Botec.Domain.Interfaces;

public interface INicknameRepository
{
    Task UpdateNicknameOfTheDay(long accountId, Guid newNicknameId, CancellationToken cancellationToken);
    Task<IEnumerable<Nickname>> GetAllNicknames(CancellationToken cancellationToken);
    Task<IEnumerable<NicknameIntroductoryPhrase>> GetAllNicknameIntroductoryPhrases(CancellationToken cancellationToken);
}