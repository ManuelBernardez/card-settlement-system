<?php

class Validacion
{
    public static function documentoValido($doc)
    {
        $len = strlen($doc);

        if ($len < 7 || $len > 8) return false;

        return ctype_digit($doc);
    }

    public static function tipoDocumentoValido($tipo)
    {
        return in_array($tipo, ['DNI', 'PASAPORTE']);
    }
}