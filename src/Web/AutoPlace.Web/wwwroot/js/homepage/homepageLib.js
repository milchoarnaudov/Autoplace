function setupHomePage() {
    let masterContainer = document.getElementById("masterContainer");
    let masterNavbar = document.getElementById("masterNavbar");
    let masterNavbarClasses = masterNavbar.classList;
    masterContainer.classList = [];
    masterContainer.className = "w-100 h-100";

    for (var i = 0; i < masterNavbarClasses.length; i++) {
        console.log(masterNavbarClasses[i])
        if (masterNavbarClasses[i] == "mb-3" || masterNavbarClasses[i] == "border-bottom")
            masterNavbarClasses.remove(masterNavbarClasses[i]);
    }
}