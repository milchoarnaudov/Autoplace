function getCarModelsOnChange() {
    let carManufacturer = document.getElementById('CarManufacturerId');
    let carModel = document.getElementById('ModelId');

    fetchData(carManufacturer.value);

    carManufacturer.addEventListener('change', (e) => {
        fetchData(e.target.value);
    });

    function fetchData(target) {
        fetch(`/autoparts/getModelsById?Id=${target}`)
            .then((response) => {
                return response.json();
            })
            .then((data) => {
                Array.prototype.forEach.call(data, function (item, i) {
                    carModel.innerHTML += `<option value="${item.Key}">${item.Value}</option>`
                });
            });
    }
}

