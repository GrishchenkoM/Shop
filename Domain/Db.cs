using System.Configuration;

namespace Domain
{
    public class Db
    {
        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString; }
        }
    }
}
