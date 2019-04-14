
var table;
$(function () {

    //Datatable template
    table = $('#candidates').DataTable({
        "ajax": {
            "url": "/api/candidate",
            "dataSrc": ""
        },
        "columns": [
            {
                "data": "Id",
                'visible': false
            },
            { "data": "FirstName" },
            { "data": "LastName" },
            { "data": "Email" },
            { "data": "Mobile" },
            {
                "bSortable": false,
                "data": "Resume",
                "render": function (data, type, row) {

                    //if user attached resume then display
                    if (row.Resume)
                        return "<a href='Download?file=" + row.Resume + "' target='_blank'>Download</a>";

                    return data;
                }
            },
            {
                "bSortable": false,
                "render": function (data, type, row) {   //edit and delete buttons

                    //console.log(data, type, row);

                    return "<button class='btn btn-primary btn-xs pull-left edit' data-id='" + row.Id + "' data-first='" + row.FirstName + "' data-last='" + row.LastName + "' data-email='" + row.Email + "' data-mobile='" + row.Mobile + "'  data-resume='" + row.Resume + "' ><span class='glyphicon glyphicon-pencil'></span></button>" +
                        "<button class='btn btn-danger btn-xs pull-right delete' data-id='" + row.Id + "'><span class='glyphicon glyphicon-trash'></span></button>";
                }
            }

        ]
    });

});

//Edit Candidate
$(document).on('click', 'button.edit', function () {
    clearValues();

    $("#title").text('Edit Candidate');

    var id = $(this).data('id');

    $('#firstname').val($(this).data('first'));
    $('#lastname').val($(this).data('last'));
    $('#email').val($(this).data('email'));
    $('#mobile').val($(this).data('mobile'));
    $('#id').val(id);

    var resume = $(this).data('resume');

    //if already attached
    if (resume) {
        $('#uploadedResume').text(resume);
        $('#uploadedResume').removeClass('hidden');

        $('#uploadedResume').attr("href", "Home/Download?file=" + resume);

        $('#deleteResume').removeClass('hidden');
        $('#deleteResume').attr("href", "/api/candidate/deletefile?Id=" + id);

        $('#resume').addClass('hidden');

    }
    else {
        $('#resume').removeClass('hidden');
        $('#uploadedResume').addClass('hidden');
        $('#deleteResume').addClass('hidden');
    }

    $('#editModal').modal('show');
});


//Delete alert 
$(document).on('click', 'button.delete', function () {
    $('#deleteId').val($(this).data('id'));
    $('#confirm-delete').modal('show');

});


//New Candidate
$('.new').click(function () {

    clearValues();
    $("#title").text('Create Candidate');
    $('#editModal').modal('show');

});


//Clears values in modal
function clearValues() {

    $('#firstname').val('');
    $('#lastname').val('');
    $('#email').val('');
    $('#mobile').val('');
    $('#resume').val('');
    $('#id').val('0');

}

//On Save validate if inputs are empty or not
function validateInputs() {
    var empty = true;

    $('.validate').each(function () {
        if ($(this).val() == "") {
            console.log($(this));
            empty = false;
            return false;
        }
    });
    return empty;
}

//Save changes in modal
$('#Save').click(function () {

    var test = validateInputs();
    console.log("validate", test);

    if (!test)
        return;

    var data = new FormData();

    var files = $("#resume").get(0).files;

    // Add the uploaded image content to the form data collection
    if (files.length > 0) {
        data.append("UploadedImage", files[0]);
    }

    data.append("Id", $('#id').val());
    data.append("FirstName", $('#firstname').val());
    data.append("LastName", $('#lastname').val());
    data.append("Email", $('#email').val());
    data.append("Mobile", $('#mobile').val());
    data.append("Resume", files[0]);


    $.ajax({
        type: "POST",
        url: "/api/candidate/save",
        contentType: false,
        processData: false,
        data: data

    })
        .done(function (data) {
            $('#editModal').modal('hide');
            table.ajax.reload();
        })
        .fail(function (data) {
            console.log("error", data);
        });


});

//Delete confirmed
$('#Delete').click(function () {

    $.post('/api/candidate/delete?Id=' + $('#deleteId').val(), function (data, status) {
        console.log(status);
    }).done(function (d) {
        $('#confirm-delete').modal('hide');
        table.ajax.reload();
    });
});

