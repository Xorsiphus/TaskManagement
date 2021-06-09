"use strict"

// $.ajax({
//     dataType: "json",
//     url: "tmp.json",
//     success: function (data) {
//         if (data) {
//             const ul = document.createElement('ul')
//             ul.setAttribute('class', 'treeUL')
//             recursion([data], ul)
//             document.getElementById("tree-box").appendChild(ul)
//             drawTree();
//         }
//     },
//
// });

$(document).ready(() => {
    $.ajax({
        url: "https://localhost:5001/api/TreeItem/Children",
        type: "GET",
        data: {
            id: "8A0BA0D9-6243-45DD-26EF-08D92B518025",
        },
        contentType: "application/json",
        success: (data) => {
            console.log(data);
        },
        error: function (result) {
            console.log(result);
        }
    });
});
