﻿const intProps = ["predictRunTime", "curRunTime", "subTasksPredictTime", "subTasksCurTime"];

const treeDfs = (parent, id, callback) => {

    if (parent.children !== undefined)
        if (parent.tagName === "UL" || parent.tagName === "LI")
            for (let i = 0; i < parent.children.length; i++) {
                if (parent.id === id) {
                    callback(parent);
                }
                treeDfs(parent.children[i], id, callback);
            }
};

const resolveStatus = (statusCode) => {
    switch (statusCode) {
        case 0:
            return "★";
        case 1:
            return "▶";
        case 2:
            return "◷";
        case 3:
            return "✓";
    }
};

const createMainTask = () => {

    const inputs = document.querySelectorAll(".form-inputs");
    const uselessFields = ["status", "regTime", "subTasksPredictTime", "subTasksCurTime", "completionTime"];

    if (sessionStorage.getItem("Action") !== "CreateMain") {

        for (let i = 0; i < inputs.length; i++) {
            document.getElementById("taskLabel").innerText = "";
            inputs[i].value = "";
            inputs[i].readOnly = false;
            if (uselessFields.indexOf(inputs[i].id) > -1)
                inputs[i].disabled = true;
        }
    } else {

        let data = {
            status: 0,
            parentId: null
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
            ...data,
            subTasksCurTime: 0,
            subTasksPredictTime: 0,
            completionTime: null,
            regTime: null
        };


        $.ajax({
            url: "https://localhost:5001/api/Task",
            data: JSON.stringify(data),
            type: "POST",
            contentType: "application/json",
            success: (reqData) => {
                let tree = document.querySelectorAll(".treeUL");
                let ul = tree[0];

                const li = document.createElement('li');
                li.setAttribute('id', reqData.id);

                const span = document.createElement('span');
                span.setAttribute('class', 'point');
                span.setAttribute('onclick', 'ShowTaskDetails(this);');
                span.textContent = reqData.name + ': ' + resolveStatus(reqData.status);

                ul.appendChild(li);
                li.appendChild(span);
            },
            error: function (result) {
                console.log(result);
            }
        });
    }

    sessionStorage.setItem("Action", "CreateMain");
};

const createSubTask = () => {

    const inputs = document.querySelectorAll(".form-inputs");

    const parentId = sessionStorage.getItem("TaskId");

    if (!parentId) {
        alert("Необходимо выбрать задачу - родителя");
        return;
    }

    if (sessionStorage.getItem("Action") === "Edit") {

        let data = {
            status: 0,
            parentId: parentId
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
            ...data,
            subTasksCurTime: 0,
            subTasksPredictTime: 0,
            completionTime: null,
            regTime: null
        };

        $.ajax({
            url: "https://localhost:5001/api/Task",
            data: JSON.stringify(data),
            type: "POST",
            contentType: "application/json",
            success: () => {
                let tree = document.querySelectorAll(".treeUL");
                let ul = tree[0];

                let li;
                let flag = false;
                const callback = (arg) => li = arg;

                treeDfs(ul, parentId, callback);
                if (li.children !== undefined)
                    for (let i = 0; i < li.children.length; i++) {
                        if (li.children[i].classList.contains("before"))
                            flag = true;
                        if (li.children[i].classList.contains("before-down"))
                            li.children[i].classList.toggle("before-down");
                        if (li.children[i].classList.contains("active")) {
                            li.children[i].classList.toggle("active");
                            while (li.children[i].firstChild) {
                                li.children[i].removeChild(li.children[i].firstChild);
                            }
                        }
                    }

                alert("Подадача создана!");

                if (flag) return;

                const before = document.createElement('span');
                before.setAttribute('onclick', 'ShowChildren(this);');
                before.setAttribute('class', 'before');
                before.textContent = "❯";
                li.insertBefore(before, li.firstChild);

                const ulLower = document.createElement('ul');
                ulLower.setAttribute('class', 'hidden');
                li.appendChild(ulLower);
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

    if (!taskId) {
        alert("Необходимо выбрать задачу");
        return;
    }

    let data = {
        id: taskId,
        status: parseInt(taskStatus, 10),
        parentId: taskParentId === "null" ? null : taskParentId
    };

    const inputs = document.querySelectorAll(".form-inputs");

    for (let i = 0; i < inputs.length; i++) {
        if (intProps.indexOf(inputs[i].id) > -1)
            data[inputs[i].id] = parseInt(inputs[i].value, 10);
        else
            data[inputs[i].id] = inputs[i].value;
    }

    $.ajax({
        url: "https://localhost:5001/api/Task",
        data: JSON.stringify(data),
        type: "PUT",
        contentType: "application/json",
        success: (reqData) => {
            for (let i = 0; i < inputs.length; i++) {
                inputs[i].value = reqData[inputs[i].id];
            }

            const tree = document.querySelectorAll(".treeUL");
            const ul = tree[0];

            let result;
            const callback = (arg) => result = arg;
            treeDfs(ul, taskId, callback);

            if (result.children !== undefined)
                for (let i = 0; i < result.children.length; i++)
                    if (result.children[i].classList.contains("point")) {
                        result.children[i].textContent = reqData.name + ': ' + resolveStatus(reqData.status);
                        break;
                    }

            alert("Задача обновлена!");
        },
        error: function (result) {
            console.log(result);
        }
    });
};

const removeTask = () => {

    const taskId = sessionStorage.getItem("TaskId");

    if (!taskId) {
        alert("Необходимо выбрать задачу");
        return;
    }

    const inputs = document.querySelectorAll(".form-inputs");

    $.ajax({
        url: "https://localhost:5001/api/Task",
        data: '"' + taskId + '"',
        type: "DELETE",
        contentType: "application/json",
        success: (reqData) => {
            console.log(reqData);
            
            if (reqData){
                for (let i = 0; i < inputs.length; i++) {
                    inputs[i].value = "";
                }

                let tree = document.querySelectorAll(".treeUL");
                let ul = tree[0];
                let li;
                const callback = (arg) => li = arg;

                treeDfs(ul, taskId, callback);

                const parent = li.parentElement.parentElement;
                const parentUl = li.parentElement;

                if (parentUl.children.length === 1) {
                    for (let i = 0; i < parent.children.length; i++)
                        if (parent.children[i].classList.contains("before") ||
                            parent.children[i].classList.contains("hidden"))
                            parent.children[i].remove();

                }

                li.remove();

                document.getElementById("taskLabel").innerText = "";
                alert("Задача удалена!");
            } else {
                alert("У этой задачи есть подзадачи!");
            }
        },
        error: function (result) {
            console.log(result);
        }
    });
};