<?php
require_once "config/db.php";

$documento = $_POST['documento'];
$usuario = $_POST['usuario'];
$password = $_POST['password'];

// validar tarjeta existente
$sql = "SELECT * FROM tarjetas WHERE dni_titular = ?";
$stmt = $conexion->prepare($sql);
$stmt->bind_param("s", $documento);
$stmt->execute();

$res = $stmt->get_result();

if ($res->num_rows == 0) {
    die("No tenés tarjeta asociada.");
}

// activar usuario
$sql = "UPDATE usuarios SET usuario=?, password=? WHERE documento=?";
$stmt = $conexion->prepare($sql);
$stmt->bind_param("sss", $usuario, $password, $documento);

$stmt->execute();

echo "Cuenta activada correctamente";
?>