$(document).ready(function () {
    $('#tableClass').bootstrapTable({
        url: '/class/getall',
        dataField: 'data',
        pagination: true,
        search: true,
        columns: [{
            field: 'idKey',
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
        '<a href="/class/Edit?classId=' + row.idKey + '" class="btn btn-primary px-3">Upravit</a>',
        ' ',
        '<a href="/class/Delete?classId=' + row.idKey + '" class="btn btn-danger px-3">Vymazat</a>',
        '<a href="/class/WhichStudent?classId=' + row.idKey + '" class="btn btn-success px-3">Přidat Studenta</a>',
    ].join('');
}

