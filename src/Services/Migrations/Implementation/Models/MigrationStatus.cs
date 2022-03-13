using System;

namespace HelpLine.Services.Migrations.Models
{
    public abstract class MigrationStatus
    {
        public DateTime DateTime { get; protected set; }

        protected MigrationStatus()
        {
            DateTime = DateTime.UtcNow;
        }

        protected MigrationStatus(DateTime dateTime)
        {
            DateTime = dateTime;
        }
    }

    public class MigrationInQueueStatus : MigrationStatus { }
    public class MigrationExecutingStatus : MigrationStatus { }
    public class MigrationAppliedStatus : MigrationStatus { }
    public class MigrationRollbackStatus : MigrationStatus { }
    public class MigrationRollbackSuccessStatus : MigrationStatus { }

    public class MigrationAppliedAndSavedStatus : MigrationStatus
    {
        public MigrationAppliedAndSavedStatus(DateTime dateTime) : base(dateTime)
        {
        }
    }
    public class MigrationErrorStatus : MigrationStatus
    {
        public Exception Exception { get; }

        public MigrationErrorStatus(Exception exception)
        {
            Exception = exception;
        }
    }

    public class MigrationRollbackErrorStatus : MigrationStatus
    {
        public Exception Exception { get;  }

        public MigrationRollbackErrorStatus(Exception exception) : base()
        {
            Exception = exception;
        }
    }
}
