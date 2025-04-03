$(document).ready(function () {
    $('#tableSubject').bootstrapTable({
        url: '/class/getall',
        dataField: 'data',
        pagination: true,
        search: true,
        columns: [{
            field: 'id',
            title: 'ID'
        }, {
            field: 'shortName',
            title: 'Zkratka'
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

function actionFormatter(value, row, index) {
    return [
        '<a href="/subject/Edit?subjectId=' + row.idKey + '" class="btn btn-primary px-3">Upravit</a>',
        ' ',
        '<a href="/subject/Delete?subjectId=' + row.idKey + '" class="btn btn-danger px-3">Vymazat</a>',
        
    ].join('');
}

