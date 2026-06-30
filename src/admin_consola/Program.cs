using System;
using MySql.Data.MySqlClient;

namespace Progra3Card.Administrativo
{
    class Program
    {
        static void Main(string[] args)
        {
            bool salir = false;

            while (!salir)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("    SISTEMA PROGRA3CARD - ADMIN         ");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Emitir Nueva Tarjeta (Alta Cliente)");
                Console.WriteLine("2. Listar Tarjetas");
                Console.WriteLine("3. Ver Detalle Tarjeta/Cliente");
                Console.WriteLine("4. Eliminar Tarjeta");
                Console.WriteLine("5. Emitir Liquidación Mensual");
                Console.WriteLine("6. Salir");
                Console.Write("Seleccione: ");

                switch (Console.ReadLine())
                {
                    case "1": EmitirTarjeta(); break;
                    case "2": ListarTarjetas(); break;
                    case "3": VerDetalle(); break;
                    case "4": EliminarTarjeta(); break;
                    case "5": EmitirLiquidacion(); break;
                    case "6": salir = true; break;
                }
            }
        }

        static bool TarjetaDuplicada(MySqlConnection conn, string documento)
        {
            string sql = "SELECT COUNT(*) FROM tarjetas WHERE dni_titular = @doc";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@doc", documento);

            long count = (long)cmd.ExecuteScalar();

            return count > 0;
        }

        static void EmitirTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- ALTA CLIENTE + TARJETA ---");

            Console.Write("Documento: ");
            string documento = Console.ReadLine();

            Console.Write("Tipo Doc (DNI/PASAPORTE): ");
            string tipo = Console.ReadLine();

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Apellido: ");
            string apellido = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Numero Tarjeta (16 digitos): ");
            string tarjeta = Console.ReadLine();

            Console.Write("Banco Emisor: ");
            string banco = Console.ReadLine();

            using var conn = ConexionBD.GetConnection();
            conn.Open();

            // Insert usuario
            if (TarjetaDuplicada(conn, documento))
            {
                Console.WriteLine("Este cliente ya tiene una tarjeta asignada.");
                Console.ReadKey();
                return;
            }

            string sqlUser = @"INSERT INTO usuarios (documento, tipo_doc, nombre, apellido, fecha_nacimiento, email)
                VALUES (@doc,@tipo,@nom,@ape,'2000-01-01',@mail)";

            using var cmd1 = new MySqlCommand(sqlUser, conn);
            cmd1.Parameters.AddWithValue("@doc", documento);
            cmd1.Parameters.AddWithValue("@tipo", tipo);
            cmd1.Parameters.AddWithValue("@nom", nombre);
            cmd1.Parameters.AddWithValue("@ape", apellido);
            cmd1.Parameters.AddWithValue("@mail", email);
            cmd1.ExecuteNonQuery();

            // Insert tarjeta
            string sqlTarjeta = @"INSERT INTO tarjetas (numero_tarjeta, banco_emisor, estado, saldo, dni_titular)
            VALUES (@tarj,@banco,'Activa',0,@doc)";

            using var cmd2 = new MySqlCommand(sqlTarjeta, conn);
            cmd2.Parameters.AddWithValue("@tarj", tarjeta);
            cmd2.Parameters.AddWithValue("@banco", banco);
            cmd2.Parameters.AddWithValue("@doc", documento);
            cmd2.ExecuteNonQuery();

            Console.WriteLine("Cliente y tarjeta creados correctamente.");
            Console.ReadKey();
        }

        static void ListarTarjetas()
        {
            Console.Clear();
            Console.WriteLine("--- TARJETAS ---");

            using var conn = ConexionBD.GetConnection();
            conn.Open();

            string sql = "SELECT num_cuenta, numero_tarjeta, banco_emisor, dni_titular, estado FROM tarjetas";

            using var cmd = new MySqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine($"Cuenta: {reader["num_cuenta"]} | DNI titular: {reader["dni_titular"]} | {reader["numero_tarjeta"]} | {reader["banco_emisor"]}  | {reader["estado"]}");
            }

            Console.ReadKey();
        }

        static void VerDetalle()
        {
            Console.Clear();
            Console.Write("Num cuenta: ");
            int cuenta = Convert.ToInt32(Console.ReadLine());

            using var conn = ConexionBD.GetConnection();
            conn.Open();

            string sql = @"
                SELECT t.*, u.nombre, u.apellido, u.email
                FROM tarjetas t
                JOIN usuarios u ON u.documento = t.dni_titular
                WHERE t.num_cuenta=@id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", cuenta);

            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Console.WriteLine($"Tarjeta: {reader["numero_tarjeta"]}");
                Console.WriteLine($"Banco: {reader["banco_emisor"]}");
                Console.WriteLine($"Saldo: {reader["saldo"]}");
                Console.WriteLine($"Cliente: {reader["nombre"]} {reader["apellido"]}");
                Console.WriteLine($"Estado tarjeta: {reader["estado"]}");
            }
            else
            {
                Console.WriteLine("No encontrado.");
            }

            Console.ReadKey();
        }

        static void EliminarTarjeta()
        {
            Console.Clear();
            Console.Write("Num cuenta: ");
            int cuenta = Convert.ToInt32(Console.ReadLine());

            using var conn = ConexionBD.GetConnection();
            conn.Open();

            string sql = "DELETE FROM tarjetas WHERE num_cuenta=@id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", cuenta);

            int rows = cmd.ExecuteNonQuery();

            Console.WriteLine(rows > 0 ? "Eliminado" : "No existe");
            Console.ReadKey();
        }

        static void EmitirLiquidacion()
        {
            Console.Clear();

            Console.Write("Num cuenta: ");
            int cuenta = Convert.ToInt32(Console.ReadLine());

            Console.Write("Periodo (YYYY-MM): ");
            string periodo = Console.ReadLine();

            Console.Write("Total a pagar: ");
            decimal total = Convert.ToDecimal(Console.ReadLine());

            if (total <= 0)
            {
                Console.WriteLine("El total debe ser mayor que cero.");
                Console.ReadKey();
                return;
            }

            decimal minimo = total * 0.1m;

            using var conn = ConexionBD.GetConnection();
            conn.Open();

            // Validar estado de la tarjeta
            string sqlEstado = "SELECT estado FROM tarjetas WHERE num_cuenta = @cuenta";

            using var cmdEstado = new MySqlCommand(sqlEstado, conn);
            cmdEstado.Parameters.AddWithValue("@cuenta", cuenta);

            string estado = cmdEstado.ExecuteScalar()?.ToString();

            if (estado == null)
            {
                Console.WriteLine("La cuenta no existe.");
                Console.ReadKey();
                return;
            }

            if (estado != "Activa")
            {
                Console.WriteLine("La tarjeta se encuentra bloqueada. No es posible emitir una liquidación.");
                Console.ReadKey();
                return;
            }

            // Emitir liquidación
            string sql = @"INSERT INTO liquidaciones
                (num_cuenta, periodo, fecha_vencimiento, total_a_pagar, pago_minimo)
                VALUES (@c,@p,DATE_ADD(CURDATE(), INTERVAL 10 DAY),@t,@m)";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@c", cuenta);
            cmd.Parameters.AddWithValue("@p", periodo);
            cmd.Parameters.AddWithValue("@t", total);
            cmd.Parameters.AddWithValue("@m", minimo);

            cmd.ExecuteNonQuery();

            Console.WriteLine("Liquidación emitida.");
            Console.ReadKey();
        }
    }
}