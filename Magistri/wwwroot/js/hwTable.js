$(document).ready(function () {
    $('#tableHW').bootstrapTable({
        url: '/homework/getallhomeworks',
        dataField: 'data',
        pagination: true,
        search: true,
        columns: [{
            field: 'id',
            title: 'ID'
        }, {
            field: 'dueDate',
            title: 'Do',
            formatter: dateFormatter
        }, {
            field: 'class.shortName',
            title: 'Třída',


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
        '<a href="/Homework/edit?hwId=' + row.id + '" class="btn btn-primary px-3">Upravit</a>',
        ' ',
        '<a href="/Homework/delete?hwId=' + row.id+ '" class="btn btn-danger px-3">Vymazat</a>',
        
    ].join('');
}

function dateFormatter(value, row, index) {
    if (!value) return "";
    var date = new Date(value);
    var day = date.getDate();
    var month = date.getMonth() + 1; 
    var year = date.getFullYear();
    
    if (day < 10) { day = "0" + day; }
    if (month < 10) { month = "0" + month; }
    return day + "." + month + "." + year;
}
