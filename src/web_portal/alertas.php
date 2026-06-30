<?php

function mostrarAlerta($mensaje, $destino = "ingreso.html", $tipo = "error")
{
    ?>
    <!DOCTYPE html>
    <html lang="es">
    <head>
        <meta charset="UTF-8">
        <script src="https://cdn.jsdelivr.net/npm/sweetalert2@11"></script>
    </head>
    <body>

    <script>
    document.addEventListener("DOMContentLoaded", function () {
        Swal.fire({
            icon: "<?= $tipo ?>",
            title: <?= json_encode($mensaje) ?>,
            confirmButtonColor: "#004691",
            confirmButtonText: "Aceptar"
        }).then(() => {
            window.location = <?= json_encode($destino) ?>;
        });
    });
    </script>

    </body>
    </html>
    <?php
    exit();
}