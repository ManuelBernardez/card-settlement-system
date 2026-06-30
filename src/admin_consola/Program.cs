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

        static bool UsuarioDuplicado(MySqlConnection conn, string documento)
        {
            string sql = "SELECT COUNT(*) FROM usuarios WHERE documento = @doc";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@doc", documento);

            long count = (long)cmd.ExecuteScalar();
            return count > 0;
        }

        // EMITIR TARJETA
        static void EmitirTarjeta()
        {
            Console.Clear();
            Console.WriteLine("--- ALTA CLIENTE + TARJETA ---");

            string documento;
            do
            {
                Console.Write("Documento: ");
                documento = Console.ReadLine();

                if (!Validacion.DocumentoValido(documento))
                    Console.WriteLine("Documento inválido.");
            }
            while (!Validacion.DocumentoValido(documento));

            string tipo;
            do
            {
                Console.Write("Tipo Doc (DNI/PASAPORTE): ");
                tipo = Console.ReadLine();

                if (!Validacion.TipoDocumentoValido(tipo))
                    Console.WriteLine("Tipo inválido.");
            }
            while (!Validacion.TipoDocumentoValido(tipo));

            Console.Write("Nombre: ");
            string nombre = Console.ReadLine();

            Console.Write("Apellido: ");
            string apellido = Console.ReadLine();

            string email;
            do
            {
                Console.Write("Email: ");
                email = Console.ReadLine();

                if (!Validacion.EmailValido(email))
                    Console.WriteLine("Email inválido.");
            }
            while (!Validacion.EmailValido(email));

            string tarjeta;
            do
            {
                Console.Write("Numero Tarjeta (16 digitos): ");
                tarjeta = Console.ReadLine();

                if (!Validacion.TarjetaValida(tarjeta))
                    Console.WriteLine("Tarjeta inválida.");
            }
            while (!Validacion.TarjetaValida(tarjeta));

            string banco;
            do
            {
                Console.WriteLine("\nBanco Emisor:");
                Console.WriteLine("1. Banco Nación");
                Console.WriteLine("2. Banco Provincia");
                Console.WriteLine("3. Banco Galicia");
                Console.WriteLine("4. Banco Santander");
                Console.WriteLine("5. Banco BBVA");
                Console.WriteLine("6. Banco Macro");
                Console.Write("Seleccione una opción: ");

                banco = Console.ReadLine() switch
                {
                    "1" => "Banco Nación",
                    "2" => "Banco Provincia",
                    "3" => "Banco Galicia",
                    "4" => "Banco Santander",
                    "5" => "Banco BBVA",
                    "6" => "Banco Macro",
                    _ => null
                };

                if (banco == null)
                    Console.WriteLine("Opción inválida.\n");

            } while (banco == null);

            using var conn = ConexionBD.GetConnection();
            conn.Open();

            if (UsuarioDuplicado(conn, documento))
            {
                Console.WriteLine("Ya existe un usuario con ese documento.");
                Console.ReadKey();
                return;
            }

            if (TarjetaDuplicada(conn, documento))
            {
                Console.WriteLine("Este cliente ya tiene una tarjeta asignada.");
                Console.ReadKey();
                return;
            }

            string sqlUser = @"INSERT INTO usuarios 
            (documento, tipo_doc, nombre, apellido, fecha_nacimiento, email)
            VALUES (@doc,@tipo,@nom,@ape,'2000-01-01',@mail)";

            using (var cmd1 = new MySqlCommand(sqlUser, conn))
            {
                cmd1.Parameters.AddWithValue("@doc", documento);
                cmd1.Parameters.AddWithValue("@tipo", tipo);
                cmd1.Parameters.AddWithValue("@nom", nombre);
                cmd1.Parameters.AddWithValue("@ape", apellido);
                cmd1.Parameters.AddWithValue("@mail", email);
                cmd1.ExecuteNonQuery();
            }

            Console.WriteLine("BANCO INGRESADO: " + banco);

            string sqlTarjeta = @"INSERT INTO tarjetas 
            (numero_tarjeta, banco_emisor, estado, saldo, dni_titular)
            VALUES (@tarj,@banco,'Activa',0,@doc)";

            using (var cmd2 = new MySqlCommand(sqlTarjeta, conn))
            {
                cmd2.Parameters.AddWithValue("@tarj", tarjeta);
                cmd2.Parameters.AddWithValue("@banco", banco);
                cmd2.Parameters.AddWithValue("@doc", documento);
                cmd2.ExecuteNonQuery();
            }

            Console.WriteLine("Cliente y tarjeta creados correctamente.");
            Console.ReadKey();
        }

        // LISTAR
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
                Console.WriteLine(
                    $"Cuenta: {reader["num_cuenta"]} | DNI: {reader["dni_titular"]} | " +
                    $"{reader["numero_tarjeta"]} | {reader["banco_emisor"]} | {reader["estado"]}"
                );
            }

            Console.ReadKey();
        }

        // DETALLE
        static void VerDetalle()
        {
            Console.Clear();

            Console.Write("Num cuenta: ");
            if (!int.TryParse(Console.ReadLine(), out int cuenta))
            {
                Console.WriteLine("Número inválido.");
                Console.ReadKey();
                return;
            }

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
                Console.WriteLine($"Estado: {reader["estado"]}");
            }
            else
            {
                Console.WriteLine("No encontrado.");
            }

            Console.ReadKey();
        }

        // ELIMINAR
        static void EliminarTarjeta()
        {
            Console.Clear();

            Console.Write("Num cuenta: ");
            if (!int.TryParse(Console.ReadLine(), out int cuenta))
            {
                Console.WriteLine("Número inválido.");
                Console.ReadKey();
                return;
            }

            using var conn = ConexionBD.GetConnection();
            conn.Open();

            string sql = "DELETE FROM tarjetas WHERE num_cuenta=@id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", cuenta);

            int rows = cmd.ExecuteNonQuery();

            Console.WriteLine(rows > 0 ? "Eliminado" : "No existe");
            Console.ReadKey();
        }

        // LIQUIDACION
        static void EmitirLiquidacion()
        {
            Console.Clear();

            Console.Write("Num cuenta: ");
            if (!int.TryParse(Console.ReadLine(), out int cuenta))
            {
                Console.WriteLine("Número inválido.");
                Console.ReadKey();
                return;
            }

            Console.Write("Periodo (YYYY-MM): ");
            string periodo = Console.ReadLine();

            if (!Validacion.PeriodoValido(periodo))
            {
                Console.WriteLine("Periodo inválido.");
                Console.ReadKey();
                return;
            }

            Console.Write("Total a pagar: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal total) || total <= 0)
            {
                Console.WriteLine("Total inválido.");
                Console.ReadKey();
                return;
            }

            decimal minimo = total * 0.1m;

            using var conn = ConexionBD.GetConnection();
            conn.Open();

            string sqlEstado = "SELECT estado FROM tarjetas WHERE num_cuenta=@cuenta";

            using var cmdEstado = new MySqlCommand(sqlEstado, conn);
            cmdEstado.Parameters.AddWithValue("@cuenta", cuenta);

            string estado = cmdEstado.ExecuteScalar()?.ToString();

            if (estado == null)
            {
                Console.WriteLine("Cuenta inexistente.");
                Console.ReadKey();
                return;
            }

            if (estado != "Activa")
            {
                Console.WriteLine("Tarjeta bloqueada.");
                Console.ReadKey();
                return;
            }

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