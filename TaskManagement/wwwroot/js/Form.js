const intProps = ["predictRunTime", "curRunTime", "subTasksPredictTime", "subTasksCurTime"];

const createTask = (isMainTask) => {

    const inputs = document.querySelectorAll(".form-inputs");
    const uselessFields = ["status", "regTime", "subTasksPredictTime", "subTasksCurTime", "completionTime"];

    const parentId = sessionStorage.getItem("TaskId");

    if (!isMainTask && !parentId){
        alert("Необходимо выбрать задачу - родителя");
        return;
    }

    if (isMainTask && sessionStorage.getItem("Action") !== "Create") {

        for (let i = 0; i < inputs.length; i++) {
            inputs[i].value = "";
            inputs[i].readOnly = false;
            if (uselessFields.indexOf(inputs[i].id) > -1)
                inputs[i].disabled = true;
        }
        
        sessionStorage.setItem("Action", "Create");
    } else {
        
        let data = {
            status: 0,
            parentId: isMainTask ? null : parentId
        };

        for (let i = 0; i < inputs.length; i++) {
            if (intProps.indexOf(inputs[i].id) > -1)
                data[inputs[i].id] = parseInt(inputs[i].value, 10);
                // const value = parseInt(inputs[i].value, 10);
                // data[inputs[i].id] = isNaN(value) ? 0 : value;
            else
                data[inputs[i].id] = inputs[i].value;
        }

        data = {
            ...data, subTasksCurTime: 0, subTasksPredictTime: 0,
            completionTime: null, regTime: null
        };

        $.ajax({
            url: "https://localhost:5001/api/Task",
            data: JSON.stringify(data),
            type: "POST",
            contentType: "application/json",
            success: (reqData) => {
                console.log(reqData);
                const tree = document.querySelectorAll(".treeUL");
                const ul = tree[0];
                const li = document.createElement('li');
                ul.appendChild(li);

                const span = document.createElement('span');
                span.setAttribute('class', 'point');
                span.setAttribute('onclick', 'ShowTaskDetails("' + reqData.id + '");');
                span.textContent = reqData.name + ': ' + reqData.status;
                li.appendChild(span);
            },
            error: function (result) {
                console.log(result);
            }
        });
    }
};


const updateTask = () => {

    const taskId = sessionStorage.getItem("TaskId");
    const taskStatus = sessionStorage.getItem("TaskStatus");
    const taskParentId = sessionStorage.getItem("TaskParentId");

    let data = {
        id: taskId,
        status: parseInt(taskStatus, 10),
        parentId: taskParentId === "null" ? null : taskParentId
    };

    if (!taskId) {
        alert("Необходимо выбрать задачу");
        return;
    }

    const inputs = document.querySelectorAll(".form-inputs");

    for (let i = 0; i < inputs.length; i++) {
        if (intProps.indexOf(inputs[i].id) > -1)
            data[inputs[i].id] = parseInt(inputs[i].value, 10);
        else
            data[inputs[i].id] = inputs[i].value;
    }

    console.log(data);

    $.ajax({
        url: "https://localhost:5001/api/Task",
        data: JSON.stringify(data),
        type: "PUT",
        contentType: "application/json",
        success: (reqData) => {
            for (let i = 0; i < inputs.length; i++) {
                inputs[i].value = reqData[inputs[i].id];
            }

            alert("Задача обновлена!");
        },
        error: function (result) {
            console.log(result);
        }
    });
};
