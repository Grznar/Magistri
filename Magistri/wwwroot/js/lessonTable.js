$(document).ready(function () {
    $('#tableLesson').bootstrapTable({
        url: '/lesson/getall',
        dataField: 'data',
        pagination: true,
        search: true,
        columns: [{
            field: 'id',
            title: 'ID'
        }, {
            field: 'applicationUser.name',
            title: 'Učitel'
        }, {
            field: 'subject.shortName',
            title: 'Předmět'
            },
            
        {
            field: 'actions',
            title: 'Akce',
            align: 'center',
            formatter: actionFormatter,  
                    
        }]
    });
});

function actionFormatter(value, row, index) {
    return [
        '<a href="/lesson/Edit?lessonId=' + row.id + '" class="btn btn-primary px-3">Upravit</a>',
        ' ',
        '<a href="/lesson/Delete?lessonId=' + row.id+ '" class="btn btn-danger px-3">Vymazat</a>',
        
    ].join('');
}

