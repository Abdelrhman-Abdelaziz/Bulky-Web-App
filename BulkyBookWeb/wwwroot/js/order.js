var dataTable;


$(document).ready(function () {
    var status = window.location.search.split('=')[1];
    if (status != null) {
        loadDataTable(status);
    } else {
        loadDataTable("all");
    }
});


function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll?status=" + status
        },
        "columns": [
            { "data": "id", "width": "10%" },
            { "data": "name", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "applicationUser.email", "width": "15%" },
            { "data": "orderStatus", "width": "15%" },
            { "data": "orderTotal", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group " role="group">
                            <a href="/Admin/Order/Details?orderId=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i></a>
                        </div>
                    `
                },
                "width": "15%"
            },
        ]
    });
}
