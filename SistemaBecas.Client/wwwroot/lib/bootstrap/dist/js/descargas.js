window.descargarArchivo = (nombre, tipo, base64) => {

    const bytes = atob(base64);

    const array = new Uint8Array(bytes.length);

    for (let i = 0; i < bytes.length; i++) {
        array[i] = bytes.charCodeAt(i);
    }

    const blob = new Blob([array], {
        type: tipo
    });

    const url = URL.createObjectURL(blob);

    const enlace = document.createElement("a");

    enlace.href = url;
    enlace.download = nombre;

    document.body.appendChild(enlace);

    enlace.click();

    enlace.remove();

    URL.revokeObjectURL(url);
};