using OutBoxPattern_POC.Entities;

namespace OutBoxPattern_POC.Services;

public interface IOutboxService
{
	Task ProcessOutboxAsync();
}
