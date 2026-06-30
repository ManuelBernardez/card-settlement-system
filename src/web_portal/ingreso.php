<?php
session_start();
require_once "../../database/db.php";
require_once "../alertas.php";

$usuario = $_POST['usuario'] ?? null;
$password = $_POST['password'] ?? null;

if (!$usuario || !$password) {
    echo "<script>alert('Datos incompletos')
    ;window.location='ingreso.html';</script>";
    exit();
}

// Validar login
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


// LOGIN
$user = obtenerUsuarioPorLogin($conexion, $usuario, $password);

if (!$user) {
    mostrarAlerta('Usuario o contraseña incorrectos');    
}

//Verificación de activación
if ($user['usuario'] === null || $user['password'] === null) {
    echo "<script>alert('Cuenta no activada')
    ;window.location='ingreso.html';</script>";
    exit();
}

// Crear sesión y redirigir al resumen
$_SESSION['documento'] = $user['documento'];
$_SESSION['usuario'] = $user['usuario'];

$stmt = null;
$conexion->close();

header("Location: resumen.php");
exit();
?>