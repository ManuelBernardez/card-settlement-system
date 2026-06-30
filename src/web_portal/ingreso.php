<?php
session_start();
require_once "../../database/db.php";
require_once "alertas.php";
require_once "validacion.php";

$usuario = $_POST['usuario'] ?? null;
$password = $_POST['password'] ?? null;

// Validar e iniciar sesión
function obtenerUsuarioPorLogin($conexion, $usuario, $password)
{
    $sql = "
        SELECT documento, usuario, password
        FROM usuarios
        WHERE usuario = ? AND password = ?
    ";

    $stmt = $conexion->prepare($sql);
    $stmt->bind_param("ss", $usuario, $password);
    $stmt->execute();

    $resultado = $stmt->get_result();
    $user = $resultado->fetch_assoc();

    $stmt->close();

    return $user;
}


$user = obtenerUsuarioPorLogin($conexion, $usuario, $password);

if (!$user) {
    mostrarAlerta('Usuario o contraseña incorrectos');    
}

// Verificar cuenta activa (cliente registrado BD mediante admin de C#)
if ($user['usuario'] === null || $user['password'] === null) {
    mostrarAlerta('Cuenta no activada');     
}

// Crear sesión y redirigir al resumen
$_SESSION['documento'] = $user['documento'];
$_SESSION['usuario'] = $user['usuario'];

$stmt = null;
$conexion->close();

header("Location: resumen.php");
exit();
?>