using RethinkDb.Driver;
using RethinkDb.Driver.Net;
using System.Threading.Tasks;

namespace Choukai
{
    public class Db
    {
        public Db()
        {
            r = RethinkDB.R;
        }

        public async Task InitAsync()
        {
            conn = await r.Connection().ConnectAsync();
            if (!r.DbList().Contains(dbName).Run<bool>(conn))
                r.DbCreate(dbName).Run(conn);
            if (!r.Db(dbName).TableList().Contains("Users").Run<bool>(conn))
                r.Db(dbName).TableCreate("Users").Run(conn);
        }

        private RethinkDB r;
        private Connection conn;

        private readonly string dbName = "Choukai"; 
    }
}
