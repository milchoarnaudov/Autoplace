function setupAdmin(token) {
    function addEventListenerByClassName(className, func) {
        const elements = document.getElementsByClassName(className);
        for (let i = 0; i < elements.length; i++) {
            elements[i].addEventListener('click', function (e) {
                if (e.target.value.includes(", ")) {
                    let values = e.target.value.split(", ");
                    func(values);
                }
                else {
                    func(e.target.value);
                }
            })
        }
    }

    addEventListenerByClassName("selectItemBtn", selectItemType);

    function selectItemType(itemType) {
        let controllerName = itemType.replace(/\s/g, '');
        let itemsList = document.getElementById("itemsList");
        let createForm = document.getElementById("createForm");
        itemsList.innerHTML = "";
        fetch(`/api/administration/${controllerName}`)
            .then((response) => {
                return response.json();
            })
            .then((data) => {
                Array.prototype.forEach.call(data, function (item, i) {
                    itemsList.innerHTML += (`<div><p>${item.Value}</p><button class="deleteItemsBtn"  value="${controllerName}, ${item.Key}">Delete</button></div>`);
                    addEventListenerByClassName("deleteItemsBtn", deleteItem);
                });
            });
        createForm.innerHTML =
            `<label>Add Item</label>
                                        <input id="itemName"/>
                                        <button class="addItemBtn" value="${controllerName}">Add</button>`
        addEventListenerByClassName("addItemBtn", addItem);
    }


    function addItem(controllerName) {
        let name = document.getElementById("itemName");
        name.style.borderColor = "";
        if (!/^[a-zA-Z\s]*$/.test(name.value) || name.value.length < 3) {
            name.style.borderColor = 'Red';
            return;
        }
        const formData = new FormData();
        formData.append("name", name.value);
        fetch(`/api/administration/${controllerName}`,
            {
                method: "POST",
                headers: {
                    'Accept': 'application/json; charset=utf-8',
                    'Content-Type': 'application/json;charset=UTF-8',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({ name: name.value, __RequestVerificationToken: token, })
            }).then((response) => {
                selectItemType(controllerName);
            })
           
    }
    function deleteItem(values) {
        fetch(`/api/administration/${values[0]}/${values[1]}`,
            {
                method: "DELETE",
                headers: {
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({ __RequestVerificationToken: token, })
            }).then((response) => {
                selectItemType(values[0]);
            })
    }
}