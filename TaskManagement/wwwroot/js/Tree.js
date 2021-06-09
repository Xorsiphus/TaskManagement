"use strict"


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
        ul.appendChild(li);

        const span = document.createElement('span');
        span.setAttribute('class', 'point');
        span.textContent = children[i].name;

        if (!children[i].isParent) {
            li.appendChild(span);
            continue;
        }

        const before = document.createElement('span');
        before.setAttribute('onclick', 'ShowChildren(this, this.parentElement.children[2], "'
            + children[i].id + '");');
        before.setAttribute('class', 'before');
        before.textContent = "❯";
        li.appendChild(before);
        li.appendChild(span);

        const ulLower = document.createElement('ul');
        ulLower.setAttribute('class', 'hidden');
        li.appendChild(ulLower);
    }
}


const ShowChildren = (arrow, parentUl, parentId) => {

    arrow.classList.toggle("before-down");

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
