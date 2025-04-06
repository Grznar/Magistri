$(document).ready(function () {
    $('#tableEntries').bootstrapTable({
        url: '/TimeTableEntry/getalltte',
        dataField: 'data',
        pagination: true,
        search: true,
        columns: [{
            field: 'id',
            title: 'ID'
        }, {
            field: 'class.shortName',
            title: 'Zkratka Třídy'
            }, {
            field: 'class.name',
            title: 'Název třídy'
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
        '<a href="/timetableentry/TableDetails?tableId=' + row.id + '" class="btn btn-primary px-3">Upravit</a>',
        
        
    ].join('');
}

