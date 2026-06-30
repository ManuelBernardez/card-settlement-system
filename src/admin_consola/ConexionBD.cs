using MySql.Data.MySqlClient;

namespace Progra3Card.Administrativo
{
    public static class ConexionBD
    {
        public static string ConnectionString =
            "Server=localhost;Database=mi_banco_db;Uid=root;Pwd=;";

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}