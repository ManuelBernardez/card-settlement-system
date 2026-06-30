<?php

$db_config = [
    'host' => 'tu_servidor',
    'db'   => 'tu_db',
    'user' => 'tu_usuario',
    'pass' => 'tu_password'
];

$conexion = new mysqli($db_config['host'], $db_config['user'], $db_config['pass'], $db_config['db']);

if ($conexion->connect_error) {
    die("Error de conexión: " . $conexion->connect_error);
}
$conexion->set_charset("utf8mb4");

?>