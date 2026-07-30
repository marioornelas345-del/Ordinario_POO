// JavaScript para Mi Salón de Belleza Nicté-Ha

document.addEventListener("DOMContentLoaded", function () {
    console.log("Sistema Nicté-Ha inicializado.");

    // Vista previa de imagen en formularios
    const imageInput = document.getElementById("FotoInput");
    const imagePreview = document.getElementById("FotoPreview");
    if (imageInput && imagePreview) {
        imageInput.addEventListener("change", function () {
            const file = this.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    imagePreview.src = e.target.result;
                    imagePreview.style.display = "block";
                };
                reader.readAsDataURL(file);
            }
        });
    }
});
