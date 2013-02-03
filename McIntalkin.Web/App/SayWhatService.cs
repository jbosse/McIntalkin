using System.Configuration;
using System.Linq;
using Raven.Client.Document;
using Raven.Client.Linq;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace McIntalkin.Web.App
{
    public class SayWhatService : Service
    {
        private readonly ISayWhatRepository _sayWhatRepository;

        public SayWhatService(ISayWhatRepository sayWhatRepository)
        {
            _sayWhatRepository = sayWhatRepository;
        }

        public object Post(SayWhat request)
        {
            if (_sayWhatRepository.Exists(request.Quote))
            {
                return new SayWhatResult {Status = "exists"};
            }
            _sayWhatRepository.Add(request);
            return new SayWhatResult { Status = "ok" };
        }

        public object Get(SayWhat request)
        {
            var sayWhat = _sayWhatRepository.Random();
            return sayWhat ?? new SayWhat() {Quote = "o_O"};
        }
    }

    public interface ISayWhatRepository
    {
        bool Exists(string quote);
        SayWhat Random();
        void Add(SayWhat request);
    }

    public class SayWhatRepository : ISayWhatRepository
    {
        public bool Exists(string quote)
        {
            using (var session = new DocumentStore { Url = ConfigurationManager.AppSettings["RAVENHQ_CONNECTION_STRING"].ToString().Replace("Url=","") }.Initialize().OpenSession())
            {
                var sayWhat = (from d in session.Query<SayWhat>() where d.Quote == quote select d).FirstOrDefault();
                return sayWhat != null;
            }
        }
        public SayWhat Random()
        {
            using (var session = new DocumentStore { Url = ConfigurationManager.AppSettings["RAVENHQ_CONNECTION_STRING"].ToString().Replace("Url=", "") }.Initialize().OpenSession())
            {
                return session.Advanced.LuceneQuery<SayWhat>().RandomOrdering().FirstOrDefault();
            }
        }
        public void Add(SayWhat request)
        {
            using (var session = new DocumentStore { Url = ConfigurationManager.AppSettings["RAVENHQ_CONNECTION_STRING"].ToString().Replace("Url=", "") }.Initialize().OpenSession())
            {
                session.Store(request);
                session.SaveChanges();
            }
        }
    }

    public class SayWhatResult
    {
        public string Status { get; set; }
    }

    [Route("/saywhat")]
    public class SayWhat
    {
        public string Quote { get; set; }
    }
}