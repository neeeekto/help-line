using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpLine.BuildingBlocks.Infrastructure.Data;
using HelpLine.BuildingBlocks.Infrastructure.Storage;
using Serilog;

namespace HelpLine.Modules.UserAccess.Application.Identity.Services
{
    public class SessionManager
    {
        private readonly IStorage<UserSession?> _storage;
        private readonly IRepository<UserSession> _repository;
        private ILogger _logger;

        public SessionManager(IStorage<UserSession?> storage, IRepository<UserSession> repository,
            ILogger logger)
        {
            _storage = storage;
            _repository = repository;
            _logger = logger;
        }

        public async Task Save(UserSession session, TimeSpan expire)
        {
            await _repository.Add(session);
            try
            {
                await _storage.Set(session.SessionId, session, expire);
            }
            catch (Exception e)
            {
                _logger.Warning(e, "Can't save session [{SessionId}] in cache storage", session.SessionId);
            }
        }

        public Task<UserSession?> Get(Guid sessionId)
        {
            try
            {
                return _storage.Get(sessionId);
            }
            catch (Exception e)
            {
                _logger.Warning(e , "Can't get session [{SessionId}] from cache storage", sessionId);
                return GetFromDb(sessionId);
            }
        }

        public Task<IEnumerable<UserSession>> GetList(Guid userId)
        {
            return _repository.Find(x => x.UserId == userId);
        }

        public Task<IEnumerable<UserSession>> GetList()
        {
            return _repository.Find(x => true);
        }

        public async Task ClearAll(Guid userId)
        {
            var sessions = await GetList(userId);
            await _repository.Remove(x => x.UserId == userId);
            await SelfAction(() => _storage.RemoveOne(sessions.Select(s => s.SessionId)),
                async e => _logger.Warning(e, $"Can't clear sessions for user {userId}"));
        }

        public async Task Clear(Guid sessionId)
        {
            await Clear(new[] {sessionId});
        }

        public async Task Clear(params Guid[] sessionsIds)
        {
            await _repository.Remove(x => sessionsIds.Contains(x.SessionId));
            await SelfAction(() => _storage.RemoveMany(sessionsIds.Select(x => (object)x)),
                async e => _logger.Warning(e, $"Can't clear sessions {string.Join(",", sessionsIds)}"));
        }

        public async Task SelfAction(Func<Task> action, Func<Exception, Task> errorHandler)
        {
            try
            {
                await action();
            }
            catch (Exception e)
            {
                await errorHandler(e);
            }
        }

        public async Task Sync()
        {
            var sessionsInDb = await _repository.Find(x => true);
            try
            {
                var sessionsInStorage = await _storage.GetList();
                var sessionsIds = sessionsInStorage.Select(x => (object)x.SessionId);
                await _storage.RemoveMany(sessionsIds);
                foreach (var session in sessionsInDb)
                    await Sync(session);
            }
            catch (Exception e)
            {
                _logger.Warning(e, "Can't sync sessions");
            }
        }

        private async Task Sync(UserSession session)
        {
            try
            {
                if (session.Expired > DateTime.UtcNow)
                    await _storage.Set(session.SessionId, session, session.Expired - session.CreateDate);
            }
            catch (Exception e)
            {
                _logger.Warning(e, $"Can't sync session [{session.SessionId}:{session.UserId}]");
            }
        }


        private async Task<UserSession?> GetFromDb(Guid sessionId)
        {
            var session = await _repository.FindOne(x => x.SessionId == sessionId);
            if (session?.Expired < DateTime.UtcNow)
            {
                return null;
            }

            return session;
        }
    }
}
