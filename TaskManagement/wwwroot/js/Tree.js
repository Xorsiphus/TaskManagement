﻿"use strict"

const getTaskId = (context) => {
    return context.parentElement.id;
};

const getParentUl = (context) => {
    return context.parentElement.children[2];
};


$(document).ready(() => {
    $.ajax({
        url: "https://localhost:5001/api/TreeItem/Tree",
        type: "GET",
        contentType: "application/json",
        success: (data) => {
            if (data) {
                const ul = document.createElement('ul');
                ul.setAttribute('class', 'treeUL');
                DrawLayer(data, ul);
                document.getElementById("tree-box").appendChild(ul);
            }
        },
        error: function (result) {
            console.log(result);
        }
    });
});


const DrawLayer = (children, ul) => {
    for (let i = 0; i < children.length; i++) {

        const li = document.createElement('li');
        li.setAttribute("id", children[i].id)
        ul.appendChild(li);

        const span = document.createElement('span');
        span.setAttribute('class', 'point');
        span.setAttribute('onclick', 'ShowTaskDetails(this);');
        span.textContent = children[i].name + ': ' +  children[i].status;

        if (!children[i].isParent) {
            li.appendChild(span);
            continue;
        }

        const before = document.createElement('span');
        before.setAttribute('onclick', 'ShowChildren(this);');
        before.setAttribute('class', 'before');
        before.textContent = "❯";
        li.appendChild(before);
        li.appendChild(span);

        const ulLower = document.createElement('ul');
        ulLower.setAttribute('class', 'hidden');
        li.appendChild(ulLower);
    }
}

// arrow, parentUl, parentId
const ShowChildren = (context) => {

    const parentId = getTaskId(context);
    const parentUl = getParentUl(context);

    context.classList.toggle("before-down");

    if (parentUl.classList.contains("active")) {
        while (parentUl.firstChild) {
            parentUl.removeChild(parentUl.firstChild);
        }
        parentUl.classList.toggle("active");
        return;
    }

    $.ajax({
        url: "https://localhost:5001/api/TreeItem/Children",
        data: {
            id: parentId,
        },
        type: "GET",
        contentType: "application/json",
        success: (data) => {
            if (data) {
                parentUl.classList.toggle("active");
                DrawLayer(data, parentUl);
            }
        },
        error: function (result) {
            console.log(result);
        }
    });
};

const ShowTaskDetails = (context) => {
    
    const taskId = getTaskId(context);

    const inputs = document.querySelectorAll(".form-inputs");
    const readFields = ["regTime", "subTasksPredictTime", "subTasksCurTime", "completionTime"];

    $.ajax({
        url: "https://localhost:5001/api/Task",
        data: {
            id: taskId,
        },
        type: "GET",
        contentType: "application/json",
        success: (data) => {
            if (data) {
                if (sessionStorage.getItem("Action") !== "Edit"){
                    const inputs = document.querySelectorAll(".form-inputs");
                    for (let i = 0; i < inputs.length; i++){
                        inputs[i].disabled = false;
                        if (readFields.indexOf(inputs[i].id) > -1)
                            inputs[i].readOnly = true;
                    }
                    sessionStorage.setItem('Action', "Edit");
                }

                for(let i = 0; i < inputs.length; i++){
                    inputs[i].value = data[inputs[i].id];
                }

                document.getElementById("taskLabel").innerText = data.name;

                sessionStorage.setItem('TaskId', taskId);
                sessionStorage.setItem('TaskStatus', data.status);
                sessionStorage.setItem('TaskParentId', data.parentId);
            }
        },
        error: function (result) {
            alert(result);
        }
    });
};
