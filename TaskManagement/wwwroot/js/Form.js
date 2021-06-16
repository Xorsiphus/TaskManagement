"use strict"

const intProps = ["predictRunTime", "curRunTime", "subTasksPredictTime", "subTasksCurTime"];

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

const createMainTask = (successMessage) => {

    const inputs = document.querySelectorAll(".form-inputs");
    const radioButtons = document.querySelectorAll(".from-radio-button");
    const uselessFields = ["status", "regTime", "subTasksPredictTime", "subTasksCurTime", "completionTime"];

    if (sessionStorage.getItem("Action") !== "CreateMain") {
        
        sessionStorage.removeItem("TaskId");

        for (let i = 0; i < inputs.length; i++) {
            document.getElementById("taskLabel").innerText = "";
            inputs[i].value = "";
            inputs[i].readOnly = false;
            if (uselessFields.indexOf(inputs[i].id) > -1)
                inputs[i].disabled = true;
        }

        for (let i = 0; i < radioButtons.length; i++) {
            radioButtons[i].disabled = true;
            if (radioButtons[i].checked)
                radioButtons[i].checked = false;
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
                if (reqData.error !== undefined) {
                    alert(reqData.error);
                    return;
                }
                
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

                alert(successMessage);
            },
            error: function (result) {
                console.log(result);
            }
        });
    }

    sessionStorage.setItem("Action", "CreateMain");
};

const createSubTask = (successMessage, errorMessage) => {

    const inputs = document.querySelectorAll(".form-inputs");
    const parentId = sessionStorage.getItem("TaskId");

    if (!parentId) {
        alert(errorMessage);
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
            success: (reqData) => {
                if (reqData.error !== undefined) {
                    alert(reqData.error);
                    return;
                }
                
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

                alert(successMessage);

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

const updateTask = (successMessage, errorMessage) => {

    const taskId = sessionStorage.getItem("TaskId");
    const taskParentId = sessionStorage.getItem("TaskParentId");

    if (!taskId) {
        alert(errorMessage);
        return;
    }

    let data = {
        id: taskId,
        parentId: taskParentId === "null" ? null : taskParentId
    };

    const inputs = document.querySelectorAll(".form-inputs");

    for (let i = 0; i < inputs.length; i++) {
        if (intProps.indexOf(inputs[i].id) > -1)
            data[inputs[i].id] = parseInt(inputs[i].value, 10);
        else
            data[inputs[i].id] = inputs[i].value;
    }

    if (document.getElementById("statusAppointed").checked)
        data["status"] = 0;
    if (document.getElementById("statusInProgress").checked)
        data["status"] = 1;
    if (document.getElementById("statusPaused").checked)
        data["status"] = 2;
    if (document.getElementById("statusCompleted").checked)
        data["status"] = 3;

    $.ajax({
        url: "https://localhost:5001/api/Task",
        data: JSON.stringify(data),
        type: "PUT",
        contentType: "application/json",
        success: (reqData) => {
            if (reqData.error !== undefined) {
                alert(reqData.error);
                return;
            }

            for (let i = 0; i < inputs.length; i++) {
                inputs[i].value = reqData[inputs[i].id];
            }

            const tree = document.querySelectorAll(".treeUL");
            const ul = tree[0];

            let result;
            const callback = (arg) => result = arg;
            treeDfs(ul, taskId, callback);


            if (result.children !== undefined)
                for (let i = 0; i < result.children.length; i++) {
                    if (result.children[i].classList.contains("point"))
                        result.children[i].textContent = reqData.name + ': ' + resolveStatus(reqData.status);
                    if (result.children[i].classList.contains("before-down"))
                        result.children[i].classList.toggle("before-down");
                    if (result.children[i].classList.contains("active")) {
                        result.children[i].classList.toggle("active");
                        while (result.children[i].firstChild) {
                            result.children[i].removeChild(result.children[i].firstChild);
                        }
                    }
                }

            alert(successMessage);
        },
        error: function (result) {
            console.log(result);
        }
    });
};

const removeTask = (successMessage, choiceErrorMessage, subErrorMessage) => {

    const taskId = sessionStorage.getItem("TaskId");

    if (!taskId) {
        alert(choiceErrorMessage);
        return;
    }

    const inputs = document.querySelectorAll(".form-inputs");

    $.ajax({
        url: "https://localhost:5001/api/Task",
        data: '"' + taskId + '"',
        type: "DELETE",
        contentType: "application/json",
        success: (reqData) => {
            if (reqData.error !== undefined) {
                alert(reqData.error);
                return;
            }

            if (reqData) {
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
                alert(successMessage);
            } else {
                alert(subErrorMessage);
            }
        },
        error: function (result) {
            console.log(result);
        }
    });
};
