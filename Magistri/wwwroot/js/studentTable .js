$(document).ready(function () {
    $('#tableStudent').bootstrapTable({
        url: '/class/getallstudents',
        dataField: 'data',
        pagination: true,
        search: true,
        columns: [{
            field: 'id',
            title: 'Id'
        }, {
            field: 'name',
            title: 'Celé jméno'
        }, {
            field: 'actions',
            title: 'Akce',
            align: 'center',
            formatter: actionFormatter,  
                    
        }]
    });
});
function addStudent(userId, classId) {
    $.ajax({
        url: '/class/AddStudent',
        type: 'POST',
        data: { userId: userId, classId: classId },
        success: function (result) {
            
            window.location.reload();
        },
        error: function (err) {
            console.error('Chyba při přidávání studenta:', err);
        }
    });
}
function actionFormatter(value, row, index) {
    var classId = $("#classId").val();
    return [
        
        '<button class="btn btn-primary" onclick="addStudent(\'' + row.id + '\',' + classId + ')">Pridat</button>'
    ].join('');
}
