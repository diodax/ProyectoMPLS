$(function () {

    $.ajaxSetup({ cache: false });

    $("a[data-modal]").on("click", function (e) {

        // hide dropdown if any
        $(e.target).closest('.btn-group').children('.dropdown-toggle').dropdown('toggle');
        
        
        var modalSizeClass = $(this).attr("class");
        // extracts the selector class to determine the modals size
        
        switch (modalSizeClass) {
            case 'size-sm':
                $("#myModal").removeAttr("data-width");
                break;
            case 'size-lg':
                //sets data-width="860"
                $("#myModal").attr("data-width", "860");
                break;
            default:
                //default size
                //$("#myModal").attr("data-width", "660");
                $("#myModal").removeAttr("data-width");
        }
        
        

        $('#myModalContent').load(this.href, function () {


            $('#myModal').modal({
                /*backdrop: 'static',*/
                keyboard: true
            }, 'show');

            bindForm(this);
        });

        return false;
    });


});

function bindForm(dialog) {

    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    //Refresh
                    location.reload();
                } else {
                    $('#myModalContent').html(result);
                    bindForm();
                }
            }
        });
        return false;
    });
}