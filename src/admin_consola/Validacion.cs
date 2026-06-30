using System.Text.RegularExpressions;

namespace Progra3Card.Administrativo
{
    public static class Validacion
    {
        // Documento de 7-8 dígitos
        public static bool DocumentoValido(string doc)
        {
            return !string.IsNullOrWhiteSpace(doc) && doc.Length >= 7 && doc.Length <= 8 && doc.All(char.IsDigit);
        }

        // Tipo documento
        public static bool TipoDocumentoValido(string tipo)
        {
            tipo.ToUpper();
            return tipo == "DNI" || tipo == "PASAPORTE";
        }

        // Email
        public static bool EmailValido(string email)
        {
            return !string.IsNullOrWhiteSpace(email)
                   && email.Contains("@");
        }

        // Tarjeta: 16 dígitos
        public static bool TarjetaValida(string num)
        {
            if (string.IsNullOrWhiteSpace(num) || num.Length != 16)
                return false;

            foreach (char c in num)
                if (c < '0' || c > '9')
                    return false;
            
            return true;
        }
        
        // Banco
        public static bool BancoValido(string banco)
        {
            return !string.IsNullOrWhiteSpace(banco)
                   && banco.Trim().Length >= 2;
        }

        // Total a pagar
        public static bool TotalValido(decimal total)
        {
            return total > 0;
        }

        public static bool PeriodoValido(string periodo)
        {
            if (string.IsNullOrWhiteSpace(periodo)) return false;
            if (periodo.Length != 7) return false;
            if (periodo[4] != '-') return false;

            var month = periodo.Substring(5, 2);
            if (!month.All(char.IsDigit)) return false;

            int m = int.Parse(month);
            return m >= 1 && m <= 12;
        }

    }
}