<?php
session_start();
require_once "config/db.php";

$usuario = $_POST['usuario'];
$password = $_POST['password'];

$sql = "SELECT * FROM usuarios WHERE usuario = ? AND password = ?";
$stmt = $conexion->prepare($sql);
$stmt->bind_param("ss", $usuario, $password);
$stmt->execute();

$result = $stmt->get_result();

if ($result->num_rows == 1) {
    $user = $result->fetch_assoc();

    if ($user['usuario'] == null || $user['password'] == null) {
        echo "Cuenta no activada";
        exit();
    }

    $_SESSION['documento'] = $user['documento'];
    $_SESSION['usuario'] = $user['usuario'];

    header("Location: resumen.php");
    exit();
} else {
    echo "Usuario o contraseña incorrectos";
}
